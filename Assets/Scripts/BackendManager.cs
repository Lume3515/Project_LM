using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private void Start()
    {     
           var responceOfBackend = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (responceOfBackend.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + responceOfBackend); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + responceOfBackend); // ������ ��� statusCode 400�� ���� �߻�
        }


    }


    private void Test()
    {
        //Registaration.Instance.SignUp("user1", "1234"); // [�߰�] �ڳ� ȸ������ �Լ�
        Debug.Log("�׽�Ʈ�� �����մϴ�.");
    }
}
