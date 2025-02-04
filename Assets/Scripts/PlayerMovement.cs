using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    private Rigidbody playerRb;

    private Transform mainCameraTr;

    [SerializeField] Transform aimPos;

    // 축 기준
    private float mouseX;
    private float mouseY;
    [SerializeField] Transform rotArm;

    private float horizontal_Move;
    private float vertical_Move;

    private void Awake()
    {
        // 잠그고 숨김
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCameraTr = Camera.main.transform;
    }

    private void Update()
    {
        // 부드러운 이동
        horizontal_Move = Input.GetAxis("Horizontal");
        vertical_Move = Input.GetAxis("Vertical");

        Rotation();

        #region// 조준
        if (Input.GetMouseButton(1))
        {
            Aim();
        }
        else
        {
            mainCameraTr.localPosition = Vector3.Lerp(mainCameraTr.localPosition, new Vector3(0, 0.5599999f, -2.384186e-07f), Time.deltaTime * 10);

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Aim()
    {
        mainCameraTr.position = Vector3.Lerp(mainCameraTr.position, aimPos.position, Time.deltaTime * 10);

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 25.8f, Time.deltaTime * 10);
    }


    private void Movement()
    {
        Vector3 moveDirection = new Vector3(horizontal_Move, 0, vertical_Move);

        playerRb.velocity = transform.TransformDirection(moveDirection * 5);
    }

    private void Rotation()
    {
        #region// 상하제한.....

        float angle = mainCameraTr.eulerAngles.x;

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


        mouseY = Input.GetAxisRaw("Mouse X");

        transform.Rotate(0, mouseY, 0);
        mainCameraTr.Rotate(-mouseX, 0, 0);

        rotArm.rotation = mainCameraTr.rotation;
    }
}


