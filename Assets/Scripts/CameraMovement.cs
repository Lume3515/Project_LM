using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    // ī�޶� ��ġ // 0 : ���� 1 : ������
    [SerializeField] Transform[] cameraPos;
    [SerializeField] int cameraIndex;

    // ���콺 X��
    private float mouseX;

    private bool start = true;

    private Rigidbody playerRb;

    // ȸ��
    float cameraMoveDirection;

    private void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {      
        // ���������� ������ ��
        if (cameraMoveDirection > 1.5f)
        {
            cameraIndex = 1;
        }
        // �������� ������ ��
        else if (cameraMoveDirection < -1.5f)
        {
            cameraIndex = 0;
        }
    }   

    private void FixedUpdate()
    {
        cameraMoveDirection = transform.TransformDirection(playerRb.velocity).x;

        CameraMove();
        Rotation();
    }

    private void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPos[cameraIndex].position, 0.18f);
    }

    private void Rotation()
    {
        //Debug.Log(transform.eulerAngles.x);

        float angle = transform.eulerAngles.x;

        #region// ��������.....
        // �� ����
        if (angle > 30 && angle < 100)
        {
            if (Input.GetAxis("Mouse Y") > 0)
            {
                mouseX = Input.GetAxisRaw("Mouse Y");
            }
            else if (Input.GetAxis("Mouse Y") < 0)
            {
                mouseX = 0;
            }
        }
        // �� ����
        else if (angle > 290 && angle < 330)
        {
            if (Input.GetAxis("Mouse Y") < 0)
            {
                mouseX = Input.GetAxisRaw("Mouse Y");
            }
            else if (Input.GetAxis("Mouse Y") > 0)
            {
                mouseX = 0;
            }
        }
        else
        {
            mouseX = Input.GetAxisRaw("Mouse Y");
        }
        #endregion
        if (start)
        {
            for (int i = 0; i < 4; i++)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                mouseX = 0;
            }

            start = false;
        }
        transform.Rotate(-mouseX * 5, 0, 0);
    }

}
