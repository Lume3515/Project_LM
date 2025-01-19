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
    PVP
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
        // Step 2. 회원가입 구현하기 로직        

        Debug.Log("회원가입을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"회원가입에 성공했습니다. \nID : {id}님";
        }
        else
        {
            console.text = $"회원가입에 실패했습니다. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, LogInType type, string text)
    {
        // Step 3. 로그인 구현하기 로직
        Debug.Log("로그인을 요청합니다.");

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
                newName = true;
                // 아니라면 변경
                Registaration.Instance.Nickname(text, console);
                Debug.Log(responceOfBackEnd);
            }
            else if (type == LogInType.PVP)
            {
                MainMenuManager.Instance.PVPSetting();
            }

            ScoreManager.Instance.GameDataGet_Kill();

        }
        else
        {
            console.text = $"로그인이 실패했습니다. : {responceOfBackEnd}";
            newName = false;

        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console)
    {
        // Step 4. 닉네임 변경 구현하기 로직

        Debug.Log("닉네임 변경을 요청합니다.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess() && newName)
        {
            console.text = "닉네임 변경 완료!";
        }
        else if (!newName)
        {
            console.text = ("아이디나 비번이 틀렸습니다.");
        }
        else
        {
            console.text = ("닉네임 변경에 실패했습니다 : " + bro);
        }


    }
}
