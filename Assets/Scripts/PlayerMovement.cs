using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // 마우스 민감도
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

    #region// 플레이어 이동

    // 움직임 속도
    private float moveSpeed;

    // 키 값(Input)
    private float moveX; // 좌우
    private float moveZ; // 깊이

    private Rigidbody playerRb;

    // 움직임 방향
    private Vector3 moveDirection;

    // true : 앉았다
    private bool sitDown;

    // 카메라 트랜스 폼
    [SerializeField] Transform cameraTr;

    // 회전 정의(?) 어느 방향인지
    private Vector3 right;
    private Vector3 forward;

    // 최종 방향
    private Quaternion lastRot;

    // 움직임
    private void Movement()
    {
        // 즉각적인 반응 필요
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // 안 움직일 떄
        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            if (!playerFire.ShoulderAndAim) playerFire.ShootingType = ShootingType.Stand;

            //Debug.Log($"1번째 :{playerFire.ShootingType != ShootingType.Aim} / 2번쨰 : {playerFire.ShootingType != ShootingType.Shoulder} 타입 : {playerFire.ShootingType}");

            //Debug.Log($"호출됨 {playerFire.ShootingType}");
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        // 달리기 구현
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


        // 앉기 구현
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
            // 앉았는데 안 움직일 떄
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

        // playerRb.velocity.y를 따로 뺀 이유는 playerRb.velocity.y를 0에 할당하면 moveSpeed가 곱해져서 총 벡터의 값이 0이 돼서 > 안되는거 같다!!(안 움직임)
        moveDirection = cameraTr.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0); // 로컬 기준

        playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        if (playerFire.ShootingType == ShootingType.Run)
        {

            // 앞
            if (Input.GetKey(KeyCode.W))
            {

                moveDirection += forward;
            }
            // 뒤
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= forward;
            }
            // 왼쪽
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= right;
            }
            // 오른쪽
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