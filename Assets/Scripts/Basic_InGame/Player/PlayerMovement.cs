using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

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

        right = cameraTr.transform.right;
        forward = cameraTr.transform.forward;
    }

    // 입력
    private void Update()
    {
        if (Gamemanager.Instance.GameOver)
        {
            playerRb.velocity = Vector3.zero;
            return;
        }
        else
        {

            Movement();
        }

        if (!roll && Input.GetKeyDown(KeyCode.Space) && rollCoolTIme && !PlayerFire.Instance.IsReload && PlayerFire.Instance.Shooting_Bool)
        {
           if(firstClick_Bool) firstClick_Bool = true;
            StartCoroutine(AddTime());
        }
        if (!firstClick_Bool)
        {
            if(firstClick >= 1)
            {
                firstClick = 0;
                firstClick_Bool = true;
            }
        }

    }

    // 연산
    private void FixedUpdate()
    {
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

    // 움직임
    private void Movement()
    {
        // 즉각적인 반응 필요

        if ((sitDown && PlayerFire.Instance.IsReload) || roll)
        {
            moveX = 0;
            moveZ = 0;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
        }



        // 안 움직일 떄
        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

            //Debug.Log($"1번째 :{playerFire.ShootingType != ShootingType.Aim} / 2번쨰 : {playerFire.ShootingType != ShootingType.Shoulder} 타입 : {playerFire.ShootingType}");

            //Debug.Log($"호출됨 {playerFire.ShootingType}");
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        // 달리기 구현
        else if (Input.GetKey(KeyCode.LeftShift) && !sitDown && !PlayerFire.Instance.IsReload && Gamemanager.Instance.ShootingType != ShootingType.Shoulder && !roll)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", true);
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Run;

            moveSpeed = 5f;
        }
        else if (!sitDown)
        {
            playerAnimator.SetBool("isWalk", true);
            playerAnimator.SetBool("isRun", false);

            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Walk;

            moveSpeed = 2.8f;
        }


        // 앉기 구현
        if (Input.GetKeyDown(KeyCode.C) && !sitDown && !PlayerFire.Instance.IsReload && !roll)
        {
            sitDown = true;

            Gamemanager.Instance.ShootingType = ShootingType.Sit;

            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
            playerAnimator.SetBool("isSitDown", true);

            moveSpeed = 1.5f;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && sitDown && !PlayerFire.Instance.IsReload))
        {
            sitDown = false;
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

            playerAnimator.SetBool("isSitDown", false);
        }
        if (sitDown)
        {
            // 앉았는데 안 움직일 떄
            if (playerRb.velocity.magnitude == 0)
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 0f);

                if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Sit;

            }
            else
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 1f);
                {
                    Gamemanager.Instance.ShootingType = ShootingType.SitWalk;
                }
            }
        }

        // playerRb.velocity.y를 따로 뺀 이유는 playerRb.velocity.y를 0에 할당하면 moveSpeed가 곱해져서 총 벡터의 값이 0이 돼서 > 안되는거 같다!!(안 움직임)
        moveDirection = cameraTr.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0); // 로컬 기준

        if(!roll) playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        if (Gamemanager.Instance.ShootingType == ShootingType.Run)
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

    #region// 구르기

    // 구르는 중?
    private bool roll;
    private bool rollCoolTIme = true; // 쿨타임

    // 입력 시간 재기
    private float firstClick;
    private bool firstClick_Bool = true;       

    // 구르기
    private IEnumerator Roll()
    {
        roll = true;
        rollCoolTIme = false;

        playerAnimator.SetTrigger("roll");
        transform.rotation = cameraTr.rotation;
        playerRb.velocity = transform.forward * 4.5f;

        yield return new WaitForSeconds(2.2f);

        roll = false;

        firstClick = 0;
        firstClick_Bool = true;
        StopCoroutine(AddTime());

        yield return new WaitForSeconds(5f);

        rollCoolTIme = true;
    }

    // 시간 더하기
    private IEnumerator AddTime()
    {
        if (firstClick_Bool)
        {
            firstClick_Bool = false;
            while (firstClick < 1)
            {
                firstClick += Time.deltaTime;
                
                yield return null;
            }

            yield break;
        }
        else
        {
            if(firstClick < 1)
            {
                StartCoroutine(Roll());
            }
        }
    }

    #endregion

}