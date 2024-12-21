using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 키 값 가져오기
    private float keyX;
    private float keyY;

    // 움직일 속도
    private float moveSpeed;

    // 애니메이터
    private Animator playerAnimator;

    // 방향
    private Vector3 direction;

    // 마우스 위치 (벡터에 할당할 축)  
    private float mousePosY;

    // 리지드 바디
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
        // 회전
        if (Input.GetMouseButton(1))
        {
            Rotate();
        }
        // 점프
        else if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
           StartCoroutine(Jump());
        }
    }

    private void FixedUpdate()
    {
        Move();
    }


    #region// 회전
    private void Rotate()
    {
        mousePosY = Input.GetAxis("Mouse X");

        transform.Rotate(0, mousePosY, 0);


    }
    #endregion

    #region// 움직임
    private void Move()
    {
        // 수직
        keyX = Input.GetAxis("Horizontal");
        // 수평
        keyY = Input.GetAxis("Vertical");

        direction = new Vector3(keyX, 0, keyY);

        // 이동
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // 안 움직이면 걷기 X
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


    #region// 점프

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
