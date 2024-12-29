using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class Registaration : MonoBehaviour
{
    private static Registaration instance = null;

    public static Registaration Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Registaration();
            }

            return instance;
        }
    }

    public void SignUp(string id, string pw)
    {
        // Step 2. ȸ������ �����ϱ� ����
        Debug.Log("ȸ�������� ��û�մϴ�.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + responceOfBackEnd);
        }
        else
        {
            Debug.LogError("ȸ�����Կ� �����߽��ϴ�. : " + responceOfBackEnd);
        }
    }

    public void Login(string id, string pw)
    {
        // Step 3. �α��� �����ϱ� ����
    }

    public void Nickname(string nickname)
    {
        // Step 4. �г��� ���� �����ϱ� ����
    }
}
