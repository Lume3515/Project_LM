using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.SceneManagement;

public class Registaration : MonoBehaviour
{
    private static Registaration instance = null;

    [SerializeField] TextMeshProUGUI console;
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
            console.text = $"ȸ�����Կ� �����߽��ϴ�.";
        }
        else
        {
            console.text = $"ȸ�����Կ� �����߽��ϴ�. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw)
    {
        // Step 3. �α��� �����ϱ� ����
        Debug.Log("�α����� ��û�մϴ�.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"�α����� �����߽��ϴ�. ���ӿ� �����մϴ�.";
            SceneManager.LoadScene(1);
        }
        else
        {
            console.text = $"�α����� �����߽��ϴ�. : {responceOfBackEnd}";
        }
    }

    public void Nickname(string nickname)
    {
        // Step 4. �г��� ���� �����ϱ� ����
    }
}
