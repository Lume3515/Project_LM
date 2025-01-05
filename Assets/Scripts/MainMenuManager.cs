using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    private void Start()
    {
        #region// 로그인 & 비번 & 체크

        for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
        {
            logInAndSignUPAndCheck_InputField[i] = logInAndSignUPAndCheck[i].GetComponent<TMP_InputField>();
            //Debug.Log(logInAndSignUPAndCheck[i].GetComponent<TMP_InputField>() == null);
        }

        createOrLogIn_TMP = createOrLogIn_BT.GetComponentInChildren<TextMeshProUGUI>();

        console_GameObject = console.gameObject;

        createOrLogIn_GameObject = createOrLogIn_BT.gameObject;

        #endregion

        #region// 메인 UI

        isMainMenu = false;

        #endregion     
    }

    private void Update()
    {
        if (mainUI_LoginAndSignUpAndNewNickName)
        {
            //Debug.Log("1");

            // 회원 가입이나 로그인 중일 때 공백이라면 
            if (logInAndSignUPAndCheck_InputField[0].text == string.Empty || logInAndSignUPAndCheck_InputField[1].text == string.Empty || logInAndSignUPAndCheck_InputField[2].text == string.Empty &&  signUp)
            {
                console.text = "아이디나 비번을 입력해주세요.";
                isRegistaration = false;
                //Debug.Log("2");
            }
            // 8자 초과라면
            else if (logInAndSignUPAndCheck_InputField[0].text.Length > 8 || logInAndSignUPAndCheck_InputField[1].text.Length > 8 || logInAndSignUPAndCheck_InputField[2].text.Length > 8)
            {
                console.text = "8자 까지 작성 가능합니다.";
                isRegistaration = false;
            }
            // 비번 확인이 맞을 때
            else if (logInAndSignUPAndCheck_InputField[1].text == logInAndSignUPAndCheck_InputField[2].text && !clickCreate)
            {
                console.text = "비번이 맞습니다.";
                isRegistaration = true;
            }
            // 비번 확인이 맞지 않을 때
            else if ((logInAndSignUPAndCheck_InputField[1].text != logInAndSignUPAndCheck_InputField[2].text) && !newNickName && signUp)
            {
                console.text = "비번확인이 알 맞지 않습니다.";                
                isRegistaration = false;
            }
            else
            {
                isRegistaration = true;
            }
        }

    }

    #region// 공용
    // 메인 메뉴인지?
    private bool isMainMenu;

    #endregion

    #region// 타이틀 
    // 버튼 직접연결 > 버튼 컴포넌트 에서

    // 버튼들 0 : 게임시작, 1 : 게임 튜토리얼
    [SerializeField] Button[] titleButton_1;

    // 버튼들 0 : 게임설정, 1 : 회원가입, 2 : 게임 종료
    [SerializeField] Button[] titleButton_2;


    // 버튼들 0 : 게임시작, 1 : 게임 튜토리얼, 2 : 게임 랭크
    public void TitleButton_1BT(int index)
    {
        //Debug.Log("1");

        if (isMainMenu) return;

        //Debug.Log(index);

        switch (index)
        {
            case 0:

                logInAndSignUPAndCheck_InputField[2].contentType = TMP_InputField.ContentType.Password;

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(true);
                }

                newNickName = false;
                clickCreate = false;
                signUp = false;
                isMainMenu = true;
                createOrLogIn_TMP.text = "로그인";
                mainUI_LoginAndSignUpAndNewNickName = true;
                mainUIParent.SetActive(true);
                logInAndSignUPAndCheck[2].SetActive(false);
                createOrLogIn_GameObject.SetActive(true);

                console_GameObject.SetActive(true);
                break;

            case 1:
                mainUIParent.SetActive(true);

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(false);
                }

                console_GameObject.SetActive(false);

                createOrLogIn_GameObject.SetActive(false);
                break;

            case 2:
                mainUIParent.SetActive(true);

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(false);
                }

                console_GameObject.SetActive(false);

                createOrLogIn_GameObject.SetActive(false);
                break;
        }
    }

    // 버튼들 0 : 게임설정, 1 : 닉변, 2 : 회원가입, 3 : 게임종료
    public void TitleButton_2BT(int index)
    {
        if (isMainMenu) return;

        switch (index)
        {
            case 0:

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(false);
                }
                isMainMenu = true;
                mainUIParent.SetActive(true);
                console_GameObject.SetActive(false);

                createOrLogIn_GameObject.SetActive(false);

                break;

            case 1:

                clickCreate = false;
                newNickName = true;

                logInAndSignUPAndCheck_InputField[2].contentType = TMP_InputField.ContentType.Standard;

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(true);
                }

                signUp = false;

                logInAndSignUPAndCheck_InputField[2].placeholder.GetComponent<TextMeshProUGUI>().text = "변경할 닉네임";
                mainUI_LoginAndSignUpAndNewNickName = true;
                isMainMenu = true;
                createOrLogIn_TMP.text = "닉네임 변경";
                mainUIParent.SetActive(true);
                logInAndSignUPAndCheck[2].SetActive(true);
                createOrLogIn_GameObject.SetActive(true);
                console_GameObject.SetActive(true);
                break;

            case 2:

                logInAndSignUPAndCheck_InputField[2].contentType = TMP_InputField.ContentType.Password;
                newNickName = false;
                clickCreate = false;

                for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
                {
                    logInAndSignUPAndCheck[i].SetActive(true);
                }

                signUp = true;
                isMainMenu = true;
                createOrLogIn_TMP.text = "계정 생성";
                mainUI_LoginAndSignUpAndNewNickName = true;
                mainUIParent.SetActive(true);
                logInAndSignUPAndCheck[2].SetActive(true);
                logInAndSignUPAndCheck_InputField[2].placeholder.GetComponent<TextMeshProUGUI>().text = "비밀번호 확인";
                createOrLogIn_GameObject.SetActive(true);
                console_GameObject.SetActive(true);
                break;

            case 3:
                Application.Quit();
                break;


        }
    }


    #endregion

    #region// 메인 UI
    // 메인 메뉴 UI나가기 버튼 > 버튼 컴포넌트에서 추가
    [SerializeField] Button exitMainUI;

    public void ExitMainUIBT()
    {
        mainUIParent.SetActive(false);
        isMainMenu = false;
        mainUI_LoginAndSignUpAndNewNickName = false;

        // 글자 비움
        for (int i = 0; i < logInAndSignUPAndCheck_InputField.Length; i++)
        {
            logInAndSignUPAndCheck_InputField[i].text = string.Empty;
        }
    }

    #endregion

    #region// 로그인 & 비번

    // 정규식 > 영어만 작성 가능하고 뛰어쓰기 안됨
    string pattern = @"^[a-zA-Z]+$";

    // 콘솔 게임오브젝트
    private GameObject console_GameObject;

    // 메인UI 최상위 부모
    [SerializeField] GameObject mainUIParent;

    // 콘솔 
    [SerializeField] TextMeshProUGUI console;

    // 로그인 & 비번 & 비번 채크(0 : 이름, 1 : 비번, 2 : 비번 체크 & 새로운 닉네임)
    [SerializeField] GameObject[] logInAndSignUPAndCheck;

    // (0 : 이름, 1 : 비번, 2 : 비번 체크 & 새로운 닉네임)
    private TMP_InputField[] logInAndSignUPAndCheck_InputField = new TMP_InputField[3];

    [SerializeField] Button createOrLogIn_BT; // > 버튼 컴포넌트에서 추가

    private TextMeshProUGUI createOrLogIn_TMP;

    private GameObject createOrLogIn_GameObject;

    // 계정생성인지? // true : 계정 생성중, 아니면 : 로그인
    private bool signUp;

    // 메인메뉴 UI에서 로그인이나 회원가입이나 닉변중인지?
    private bool mainUI_LoginAndSignUpAndNewNickName;

    // 로그인이나 회원가입, 닉변 가능?
    private bool isRegistaration;

    // 로그인이나 닉변, 가입버튼을 눌렀는지? true : 로그인이나 닉변, 가입버튼누름
    private bool clickCreate;

    // 새 닉네임을 짓고 있는지?
    private bool newNickName;

    // 계정 생성 또는 로그인 버튼 또는 닉네임 변경
    public void CreateOrLogInBT()
    {
        if (!isRegistaration) return;

        clickCreate = true;

        if (createOrLogIn_TMP.text == "계정 생성")
        {
            // 정규식 검사 > true > 톨과
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "비밀번호에 한글, 공백을 포함하지 마세요.";

                return;
            }        
                Registaration.Instance.SignUp(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console);
            
        }
        else if (createOrLogIn_TMP.text == "로그인")
        {
            // 정규식 검사 > true > 톨과
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "비밀번호에 한글, 공백을 포함하지 마세요.";

                return;
            }

            Registaration.Instance.Login(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console, LogInType.logIn, "null");
        }
        else if (createOrLogIn_TMP.text == "닉네임 변경")
        {
            // 정규식 검사 > true > 톨과
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "비밀번호에 한글, 공백을 포함하지 마세요.";

                return;
            }

            Registaration.Instance.Login(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console, LogInType.newName, logInAndSignUPAndCheck_InputField[2].text);
        }

        clickCreate = false;
    }

    // 정규식
    private bool Pattern(string text)
    {
        Debug.Log(Regex.IsMatch(text, pattern));

        // 정규식 인지?
         return Regex.IsMatch(text, pattern);        
    }

    #endregion
}
