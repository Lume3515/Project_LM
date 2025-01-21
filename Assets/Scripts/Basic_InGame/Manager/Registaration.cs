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
    Null

}

public class Registaration
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
                // 아니라면 변경
                Registaration.Instance.Nickname(text, console);
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
            console.text = $"로그인이 실패했습니다. : {responceOfBackEnd}";
           

        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console, LogInType type)
    {
        // Step 4. �г��� ���� �����ϱ� ����

        Debug.Log("�г��� ������ ��û�մϴ�.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            console.text = "닉네임 변경 완료!";
        }        
        else
        {
            console.text = ("닉네임 변경에 실패했습니다 : " + bro);
        }


        }
    }
