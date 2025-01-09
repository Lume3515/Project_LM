using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // ���콺 �ΰ���
    private float mouseSpeed;

    private Animator playerAnimator;

    private PlayerFire playerFire;


    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerFire = GetComponent<PlayerFire>();
    }

    private void Start()
    {

        moveSpeed = 3.8f;

        mouseSpeed = 5f;

        right = cameraTr.transform.right;
        forward = cameraTr.transform.forward;
    }

    private void FixedUpdate()
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

    // true : �ɾҴ�
    private bool sitDown;

    // ī�޶� Ʈ���� ��
    [SerializeField] Transform cameraTr;

    // ȸ�� ����(?) ��� ��������
    private Vector3 right;
    private Vector3 forward;

    // ���� ����
    private Quaternion lastRot;

    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // �� ������ ��
        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Stand;

            //Debug.Log($"1��° :{playerFire.ShootingType != ShootingType.Aim} / 2���� : {playerFire.ShootingType != ShootingType.Shoulder} Ÿ�� : {playerFire.ShootingType}");

            //Debug.Log($"ȣ��� {playerFire.ShootingType}");
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        // �޸��� ����
        else if (Input.GetKey(KeyCode.LeftShift) && !sitDown)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", true);

            if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Run;

            moveSpeed = 5f;
        }
        else if (!sitDown)
        {
            playerAnimator.SetBool("isWalk", true);
            playerAnimator.SetBool("isRun", false);

            if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Walk;

            moveSpeed = 2.8f;
        }


        // �ɱ� ����
        if (Input.GetKeyDown(KeyCode.C) && !sitDown)
        {
            sitDown = true;

            playerFire.ShootingType = ShootingType.Sit;

            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
            playerAnimator.SetBool("isSitDown", true);

            moveSpeed = 1.5f;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && sitDown))
        {
            sitDown = false;
            if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Stand;

            playerAnimator.SetBool("isSitDown", false);
        }
        if (sitDown)
        {
            // �ɾҴµ� �� ������ ��
            if (playerRb.velocity.magnitude == 0)
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 0f);

                if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Sit;

            }
            else
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 1f);
                {
                    playerFire.ShootingType = ShootingType.SitWalk;
                }
            }
        }

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = cameraTr.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0); // ���� ����

        playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        if (playerFire.ShootingType == ShootingType.Run)
        {

            // ��
            if (Input.GetKey(KeyCode.W))
            {

                moveDirection += forward;
            }
            // ��
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= forward;
            }
            // ����
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= right;
            }
            // ������
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += right;
            }

            transform.rotation = Quaternion.LookRotation(moveDirection);

        }
        else
        {
            transform.rotation = cameraTr.rotation;
        }


    }
    #endregion


}