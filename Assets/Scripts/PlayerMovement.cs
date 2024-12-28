using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // ���콺 �ΰ���
    private float mouseSpeed;

    private Animator playerAnimator;

    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {

        moveSpeed = 3.8f;

        mouseSpeed = 5f;
    }

    private void Update()
    {
        Movement();
        Rotation();

        if (Input.GetKeyDown(KeyCode.C))
        {
            playerAnimator.SetBool("isSitDown", true);
            sitDown = !sitDown;

            if (!sitDown) playerAnimator.SetBool("isSitDown", false);
        }

    }

    #region// �÷��̾� �̵�
    // ������ �ӵ�
    private float moveSpeed;

    // Ű ��(Input)
    private float moveX; // �¿�
    private float moveZ; // ����

    private Rigidbody playerRb;

    // ������ ����
    private Vector3 moveDirection;

    // ���콺 Ű ��(�� ����)
    private float mouseY;



    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        playerAnimator.SetBool("isWalk", true);

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = transform.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0);

        playerRb.velocity = moveDirection;

        if (playerRb.velocity.magnitude == 0)
        {
            playerAnimator.SetBool("isWalk", false);
        }
        else
        {

            playerAnimator.SetBool("isSitDown", false);
        }
    }

    private void Rotation()
    {
        mouseY = Input.GetAxisRaw("Mouse X");

        transform.Rotate(0, mouseY * mouseSpeed, 0);
    }
    #endregion

    #region// �ɱ� 

    // true�� ���� ��
    private bool sitDown;

    private IEnumerator SitDown()
    {
        yield return null;
    }

    #endregion
}
