using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private void Start()
    {     
           var responceOfBackend = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (responceOfBackend.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + responceOfBackend); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + responceOfBackend); // 실패일 경우 statusCode 400대 에러 발생
        }


    }


    private void Test()
    {
        //Registaration.Instance.SignUp("user1", "1234"); // [추가] 뒤끝 회원가입 함수
        Debug.Log("테스트를 종료합니다.");
    }
}
