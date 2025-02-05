using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform parent;

    // ���콺 X��
    private float mouseX;

    // �÷��̾� Ʈ������
    private Transform playerTr;

    private float mouseY;

    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;


        for (int i = 0; i < 4; i++)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            mouseX = 0;
        }

    }

    private void FixedUpdate()
    {
        Rotation();

        // �ݵ��� ������ ī�޶� ȸ�� ���
        float finalX = transform.eulerAngles.x + PlayerFire.Instance.RecoliAmount - mouseX;
        

        // ī�޶� ȸ�� ����
        transform.rotation = Quaternion.Euler(finalX, transform.eulerAngles.y, 0);       
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
        //transform.Rotate(-mouseX * 5, 0, 0);

        mouseY = Input.GetAxisRaw("Mouse X");

        parent.Rotate(0, mouseY * 5, 0);
    }

}
