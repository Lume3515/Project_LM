using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.SceneManagement;

public enum Type
{
    logIn,
    signUp,
    newName
}

public class Registaration : MonoBehaviour
{
    private static Registaration instance = null;

    private bool newName;

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

    public void SignUp(string id, string pw, TextMeshProUGUI console)
    {
        // Step 2. ȸ������ �����ϱ� ����        

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"ȸ�����Կ� �����߽��ϴ�. \n�г��� : {id}��";
        }
        else
        {
            console.text = $"ȸ�����Կ� �����߽��ϴ�. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, Type type, string text)
    {
        // Step 3. �α��� �����ϱ� ����
        Debug.Log("�α����� ��û�մϴ�.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);
        //Debug.Log(responceOfBackEnd);

        if (responceOfBackEnd.IsSuccess())
        {
            if (type == Type.logIn)
            {
                SceneManager.LoadScene(1);
                console.text = $"�α����� �����߽��ϴ�. ���ӿ� �����մϴ�.";
                
            }
            else if (type == Type.newName)
            {
                newName = true;
                // �ƴ϶�� ����
                Registaration.Instance.Nickname(text, console);
                Debug.Log(responceOfBackEnd+ "d");
            }
        }
        else
        {
            console.text = $"�α����� �����߽��ϴ�. : {responceOfBackEnd}";
            newName = false;

        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console)
    {
        // Step 4. �г��� ���� �����ϱ� ����

        Debug.Log("�г��� ������ ��û�մϴ�.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess() && newName)
        {
            console.text = "�г��� ���� �Ϸ�!";
        }
        else if (!newName)
        {
            console.text = ("���̵� ����� Ʋ�Ƚ��ϴ�.");
        }
        else
        {
            console.text = ("�г��� ���濡 �����߽��ϴ� : " + bro);
        }


    }
}
