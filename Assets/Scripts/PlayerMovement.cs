using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �÷��̾� ������ �ٵ�
    private Rigidbody playerRb;

    // ������ �ӵ�
    private float moveSpeed;

    // �����Ŀ�
    private float jumpPower;

    // �Է°� ��������
    private float inputX;
    private float inputZ;

    // ����
    private Vector3 direction;

    // ��������?
    private bool isJump;

    private Animator playerAnimator;    

    // ������?
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
        // �Է°� ��������

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isFly = true;
            isJump = false;
        }

        direction = new Vector3(inputX, 0, inputZ) * moveSpeed;

        // ���ⱸ�ϱ�

        // ������ ������ �̶��
        if(playerRb.velocity.x > 0 && playerRb.velocity.z > 0 || playerRb.velocity.x < 0 && playerRb.velocity.z >0 )
        {
            // �ٶ󺸴� ����
            transform.rotation = Quaternion.Euler(0, playerRb.velocity.x , 0);
        }       
        else if (playerRb.velocity.x < 0 && playerRb.velocity.z < 0 || playerRb.velocity.x > 0 && playerRb.velocity.z < 0)
        {
            // �ٶ󺸴� ����
            transform.rotation = Quaternion.Euler(0, playerRb.velocity.x * playerRb.velocity.z * -1, 0);
        }

        
    }

    private void FixedUpdate()
    {
        // ��� �ӵ��� 0�̶�� Idle
        if (playerRb.velocity.x == 0 && playerRb.velocity.z == 0 && playerRb.velocity.y == 0)
        {
            playerAnimator.SetBool("IsWalk", false);
        }
        // �ٴڿ� ���� ���� �ȱ�
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
