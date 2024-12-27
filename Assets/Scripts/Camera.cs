using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // ���콺 x��
    private float mouseX;

    // ������ ����
    private Vector3 moveDirection;

    // ī�޶� ��ġ
    private Vector3 offSet;

    // �÷��̾� Ʈ������
    private Transform playerTr;  

    private void Update()
    {
        Rotation();
        Movement();
    }

    private void Movement()
    {
        moveDirection = playerTr.position + offSet;

        Vector3.Slerp();
    }

    private void Rotation()
    {
        mouseX = Input.GetAxisRaw("Mouse Y");        

        transform.Rotate(-mouseX, 0, 0);
    }


    private void OnTriggerStay(Collider other)
    {
        // Map�� ���� ��ü �����
        if (other.CompareTag("Map"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Map�� ���� ��ü ���̱�
        if (other.CompareTag("Map"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
