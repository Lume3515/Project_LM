using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    // 카메라 위치 // 0 : 왼쪽 1 : 오른쪽
    [SerializeField] Transform[] cameraPos;
    [SerializeField] int cameraIndex;

    // 마우스 X축
    private float mouseX;

    private bool start = true;

    private Rigidbody playerRb;

    // 회전
    float cameraMoveDirection;

    private void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {      
        // 오른쪽으로 움직일 떄
        if (cameraMoveDirection > 1.5f)
        {
            cameraIndex = 1;
        }
        // 왼쪽으로 움직일 때
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

        #region// 상하제한.....
        // 하 제한
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
        // 상 제한
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
