using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class CameraMovement_MultiPlay : MonoBehaviour
{
    [SerializeField] Transform parent;

    // ���콺 X��
    private float mouseX;

    // �÷��̾� Ʈ������
    private Transform playerTr;
  
    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;


        for (int i = 0; i < 4; i++)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            mouseX = 0;
        }


    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        CameraMove();
        Rotation();
    }

    private void CameraMove()
    {

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
        transform.Rotate(-mouseX * 5, 0, 0);

        float moveY = Input.GetAxisRaw("Mouse X");

        parent.Rotate(0, moveY * 5, 0);
    }

}
