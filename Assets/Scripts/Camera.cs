using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // 마우스 x축
    private float mouseX;

    // 움직일 방향
    private Vector3 moveDirection;

    // 카메라 위치
    private Vector3 offSet;

    // 플레이어 트랜스폼
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
        // Map일 때는 형체 숨기기
        if (other.CompareTag("Map"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Map일 때는 형체 보이기
        if (other.CompareTag("Map"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
