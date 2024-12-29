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
        // Step 2. 회원가입 구현하기 로직
        Debug.Log("회원가입을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다. : " + responceOfBackEnd);
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + responceOfBackEnd);
        }
    }

    public void Login(string id, string pw)
    {
        // Step 3. 로그인 구현하기 로직
    }

    public void Nickname(string nickname)
    {
        // Step 4. 닉네임 변경 구현하기 로직
    }
}
