using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTr;

    // ���� ����
    private Vector3 offSet;

    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;

        offSet = new Vector3(0, 2.24f, 2.29f);
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        // ��ġ �̵�
        transform.position = offSet + playerTr.position;
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
