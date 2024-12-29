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
        // Step 2. 회원가입 구현하기 로직
        Debug.Log("회원가입을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"회원가입에 성공했습니다.";
        }
        else
        {
            console.text = $"회원가입에 실패했습니다. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw)
    {
        // Step 3. 로그인 구현하기 로직
        Debug.Log("로그인을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"로그인이 성공했습니다. 게임에 접속합니다.";
            SceneManager.LoadScene(1);
        }
        else
        {
            console.text = $"로그인이 실패했습니다. : {responceOfBackEnd}";
        }
    }

    public void Nickname(string nickname)
    {
        // Step 4. 닉네임 변경 구현하기 로직
    }
}
