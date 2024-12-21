using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 플레이어 리지드 바디
    private Rigidbody playerRb;

    // 움직임 속도
    private float moveSpeed;

    // 점프파워
    private float jumpPower;

    // 입력값 가져오기
    private float inputX;
    private float inputZ;

    // 방향
    private Vector3 direction;

    // 점프가능?
    private bool isJump;

    private Animator playerAnimator;    

    // 나는지?
    private bool isFly;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        moveSpeed = 18;

        jumpPower = 500;

        playerAnimator = GetComponent<Animator>();

        isJump = true;

        isFly = false;
    }

    private void Update()
    {
        // 입력값 가져오기

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isFly = true;
            isJump = false;
        }

        direction = new Vector3(inputX, 0, inputZ) * moveSpeed;

        // 방향구하기

        // 앞으로 가는중 이라면
        if(playerRb.velocity.x > 0 && playerRb.velocity.z > 0 || playerRb.velocity.x < 0 && playerRb.velocity.z >0 )
        {
            // 바라보는 방향
            transform.rotation = Quaternion.Euler(0, playerRb.velocity.x , 0);
        }       
        else if (playerRb.velocity.x < 0 && playerRb.velocity.z < 0 || playerRb.velocity.x > 0 && playerRb.velocity.z < 0)
        {
            // 바라보는 방향
            transform.rotation = Quaternion.Euler(0, playerRb.velocity.x * playerRb.velocity.z * -1, 0);
        }

        
    }

    private void FixedUpdate()
    {
        // 모든 속도가 0이라면 Idle
        if (playerRb.velocity.x == 0 && playerRb.velocity.z == 0 && playerRb.velocity.y == 0)
        {
            playerAnimator.SetBool("IsWalk", false);
        }
        // 바닥에 있을 때는 걷기
        else if (playerRb.velocity.x != 0 && playerRb.velocity.z != 0 && playerRb.velocity.y == 0 && !isFly)
        {
            playerAnimator.SetBool("IsWalk", true);
        }

        playerRb.velocity = direction;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("floor"))
        {
            isFly = false;
            isJump = true;
        }
    }   
}
