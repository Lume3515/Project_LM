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

    // true : �ɾҴ�
    private bool sitDown;

    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !sitDown)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", true);

            moveSpeed = 5f;
        }
        else if (!sitDown)
        {
            playerAnimator.SetBool("isWalk", true);
            playerAnimator.SetBool("isRun", false);
            

            moveSpeed = 2.8f;
        }



        if (Input.GetKeyDown(KeyCode.C) && !sitDown)
        {
            sitDown = true;

            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
            playerAnimator.SetBool("isSitDown", true);

            moveSpeed = 0.8f;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && sitDown))
        {
            sitDown = false;

            playerAnimator.SetBool("isSitDown", false);
        }
        else if (sitDown)
        {
            if(playerRb.velocity.magnitude == 0)
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 0f);
            }
            else
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 1f);
            }
        }

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = transform.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0);

        playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        mouseY = Input.GetAxisRaw("Mouse X");

        transform.Rotate(0, mouseY * mouseSpeed, 0);
    }
    #endregion

}
