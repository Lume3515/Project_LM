using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.SceneManagement;

public enum LogInType
{
    logIn,
    newName,
    PVP,
    Rank,


}

public class Registaration
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



    public void SignUp(string id, string pw, TextMeshProUGUI console)
    {
        // Step 2. ȸ������ �����ϱ� ����        

        Debug.Log("ȸ�������� ��û�մϴ�.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"ȸ�����Կ� �����߽��ϴ�. \nID : {id}��";

            Nickname(id, console, LogInType.Etc);
        }
        else
        {
            console.text = $"ȸ�����Կ� �����߽��ϴ�. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, LogInType type, string text)
    {
        // Step 3. �α��� �����ϱ� ����
        Debug.Log("�α����� ��û�մϴ�.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);
        //Debug.Log(responceOfBackEnd);

        if (responceOfBackEnd.IsSuccess())
        {
            if (type == LogInType.logIn)
            {
                LoadingManager.name_Scene = "InGame";
                LoadingManager.loading = Loading.InGame;

                SceneManager.LoadScene(2);

            }
            else if (type == LogInType.newName)
            {

                // �ƴ϶�� ����
                Registaration.Instance.Nickname(text, console, LogInType.newName);
                Debug.Log(responceOfBackEnd);
            }
            else if (type == LogInType.PVP)
            {
                MainMenuManager.Instance.PVPSetting();
            }
            else if (type == LogInType.Rank)
            {
                MainMenuManager.Instance.RankSetting();
            }



        }
        else
        {
            console.text = $"�α����� �����߽��ϴ�. : {responceOfBackEnd}";


        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console, LogInType type)
    {
        // Step 4. �г��� ���� �����ϱ� ����

        Debug.Log("�г��� ������ ��û�մϴ�.");

        var bro = Backend.BMember.UpdateNickname(nickname);


        if (type == LogInType.newName)
        {
            if (bro.IsSuccess())
            {
                console.text = "�г��� ���� �Ϸ�!";
            }
            else
            {
                console.text = ("�г��� ���濡 �����߽��ϴ� : " + bro);
            }

            if (bro.IsSuccess())
            {
                console.text = "�г��� ���� �Ϸ�!";
            }
            else
            {
                if (bro.IsSuccess())
                {
                    Debug.Log("�г��� ���� �Ϸ�!");
                }
                else
                {
                    Debug.Log("�г��� ���濡 �����߽��ϴ� : " + bro);
                }
            }


        }
    }
