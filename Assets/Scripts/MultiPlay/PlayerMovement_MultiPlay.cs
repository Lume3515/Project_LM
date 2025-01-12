using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayerMovement_MultiPlay : MonoBehaviour
{

    private Animator playerAnimator;

    private PlayerFire_MultiPlay playerFire_MultiPlay;

    // ����ȭ �ʼ�
    private PhotonView photonView;

    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;


        PhotonNetwork.Instantiate("MultiPlay/Camera_MultiPlay", new Vector3(0, 0, -78.73f), Quaternion.Euler(0, 0, 0), 0);
    }

    private void Start()
    {
        moveSpeed = 3.8f;

        cameraTr = Camera.main.transform;
        right = cameraTr.transform.right;
        forward = cameraTr.transform.forward;
        photonView = GetComponent<PhotonView>();

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerFire_MultiPlay = GetComponent<PlayerFire_MultiPlay>();
    }

    // �Է�
    private void Update()
    {
        // �������� üũ
        // ����䰡 ���� ��������, ���� ������ �ƴ϶�� ����
        if (!photonView.IsMine) return;

        Movement();
    }

    // ����
    private void FixedUpdate()
    {
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
    private Transform cameraTr;

    // ȸ�� ����(?) ��� ��������
    private Vector3 right;
    private Vector3 forward;
    

    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // �� ������ ��
        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            if (!playerFire_MultiPlay.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

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

            if (!playerFire_MultiPlay.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Run;

            moveSpeed = 5f;
        }
        else if (!sitDown)
        {
            playerAnimator.SetBool("isWalk", true);
            playerAnimator.SetBool("isRun", false);

            if (!playerFire_MultiPlay.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Walk;

            moveSpeed = 2.8f;
        }


        // �ɱ� ����
        if (Input.GetKeyDown(KeyCode.C) && !sitDown)
        {
            sitDown = true;

            Gamemanager.Instance.ShootingType = ShootingType.Sit;

            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
            playerAnimator.SetBool("isSitDown", true);

            moveSpeed = 1.5f;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && sitDown))
        {
            sitDown = false;
            if (!playerFire_MultiPlay.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

            playerAnimator.SetBool("isSitDown", false);
        }
        if (sitDown)
        {
            // �ɾҴµ� �� ������ ��
            if (playerRb.velocity.magnitude == 0)
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 0f);

                if (!playerFire_MultiPlay.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Sit;

            }
            else
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 1f);
                {
                    Gamemanager.Instance.ShootingType = ShootingType.SitWalk;
                }
            }
        }

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = cameraTr.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0); // ���� ����

        playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        if (Gamemanager.Instance.ShootingType == ShootingType.Run)
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