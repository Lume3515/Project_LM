using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // ������ �ӵ�
    private float moveSpeed;

    // Ű ��(Input)
    private float moveX; // �¿�
    private float moveZ; // ����

    private Rigidbody playerRb;

    // ������ ����
    private Vector3 moveDirection;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        moveSpeed = 10f;
    }

    private void Update()
    {
        Movement();
    }

    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = transform.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0);

        playerRb.velocity = moveDirection;
    }
}
