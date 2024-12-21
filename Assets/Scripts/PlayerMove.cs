using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Ű �� ��������
    private float keyX;
    private float keyY;

    // ������ �ӵ�
    private float moveSpeed;

    // �ִϸ�����
    private Animator playerAnimator;

    // ����
    private Vector3 direction;

    // ���콺 ��ġ (���Ϳ� �Ҵ��� ��)  
    private float mousePosY;

    // ������ �ٵ�
    private Rigidbody playerRb;

    private bool isJump;

    private void Start()
    {
        moveSpeed = 20f;

        playerAnimator = GetComponent<Animator>();

        playerRb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ȸ��
        if (Input.GetMouseButton(1))
        {
            Rotate();
        }
        // ����
        else if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
           StartCoroutine(Jump());
        }
    }

    private void FixedUpdate()
    {
        Move();
    }


    #region// ȸ��
    private void Rotate()
    {
        mousePosY = Input.GetAxis("Mouse X");

        transform.Rotate(0, mousePosY, 0);


    }
    #endregion

    #region// ������
    private void Move()
    {
        // ����
        keyX = Input.GetAxis("Horizontal");
        // ����
        keyY = Input.GetAxis("Vertical");

        direction = new Vector3(keyX, 0, keyY);

        // �̵�
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // �� �����̸� �ȱ� X
        if (keyX == 0 && keyY == 0)
        {
            playerAnimator.SetBool("IsWalk", false);
        }
        else
        {
            playerAnimator.SetBool("IsWalk", true);
        }
    }
    #endregion


    #region// ����

    private IEnumerator Jump()
    {
        playerAnimator.SetTrigger("Jump");

        yield return new WaitForSeconds(0.85f);

        playerRb.AddForce(0, 100, 0, ForceMode.Impulse);

        isJump = false;

        yield return new WaitForSeconds(10);

        isJump = true;
    }

    #endregion

}
