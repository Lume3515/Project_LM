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
    Etc
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


        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"ȸ�����Կ� �����Ͽ����ϴ�. ȯ���մϴ�. \nID : {id}��";

            Nickname(id, console, LogInType.Etc);
        }
        else
        {
            console.text = $"ȸ�����Կ� �����Ͽ����ϴ�.. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, LogInType type, string text)
    {

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
                Registaration.Instance.Nickname(text, console, LogInType.newName);
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
        }
    }
}