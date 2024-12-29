using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    // 카메라 위치 // 0 : 왼쪽 1 : 오른쪽 2 : 보스
    [SerializeField] Transform[] cameraPos;
    [SerializeField] int cameraIndex;

    // 플레이어 트랜스폼
    private Transform playerTr;

    // 마우스 X축
    private float mouseX;

    private bool start = true;


    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cameraIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            cameraIndex = 1;
        }
    }

    private void FixedUpdate()
    {
        CameraMove();
        Rotation();
    }

    private void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPos[cameraIndex].position, 0.25f);
    }

    private void Rotation()
    {
        Debug.Log(transform.eulerAngles.x);

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
