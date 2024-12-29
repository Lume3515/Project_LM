using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // 정규식

public class LogInManager : MonoBehaviour
{
    //[SerializeField] Button deleteAccount; // 계정 삭제

    [SerializeField] GameObject textBackGroundGameObject; // 시작화면 부모 객체
    [SerializeField] GameObject[] howToPlayAndRank; // 게임방법 및 랭크 활성화
    [SerializeField] GameObject[] setting_Object; // 세팅 오브젝트
    [SerializeField] GameObject makingAccount_GameObject; // 계정 만들기  
    [SerializeField] GameObject inputPWCheck_GameObject;// 비번체크 게임오브젝트   
    [SerializeField] GameObject gameStart_GameObject; // 시작버튼 객체
    [SerializeField] GameObject[] logInAndSignUp; // 로그인과 가입, 콘솔포함
    [SerializeField] GameObject newNickName; // 닉네임 설정 버튼
    [SerializeField] GameObject soundsetting; // 사운드 세팅 버튼
    [SerializeField] GameObject mouseSpeedSetting; // 마우스 세팅 버튼            

    public void Start()
    {
        #region// 할당
        newNickName_Button = newNickName.GetComponent<Button>();
        soundsetting_Button = soundsetting.GetComponent<Button>();
        mouseSpeedSetting_Button = mouseSpeedSetting.GetComponent<Button>();

        making = makingAccount_GameObject.GetComponent<Button>();
        gameStart = gameStart_GameObject.GetComponent<Button>();
        inputPWCheck = inputPWCheck_GameObject.GetComponent<TMP_InputField>();

        newNiickName_TMP_InputField = newNickName.GetComponent<TMP_InputField>();
        newNickNameBT = newNickName_BT_Objcet.GetComponent<Button>();

        settingBT[0] = newNickName; settingBT[1] = soundsetting; settingBT[2] = mouseSpeedSetting;
        #endregion

        #region// 버튼 추가
        mouseSpeedSetting_Button.onClick.AddListener(MouseSpeedSetting);
        soundsetting_Button.onClick.AddListener(SoundSetting);
        newNickName_Button.onClick.AddListener(NewNickNameSet);
        gameStart.onClick.AddListener(GameStartBT);
        howToGamePlay.onClick.AddListener(HowToGamePlayBT);
        making.onClick.AddListener(Making);
        createAnAccountOrLogin.onClick.AddListener(CreateAnAccountOrLoginBT);
        gameQuit.onClick.AddListener(GameQuitBT);
        settiingButton.onClick.AddListener(SettiingBT);
        newNickNameBT.onClick.AddListener(NewNickNameBT);
        #endregion
    }

    private void Update()
    {
        #region// 조건
        // 공백인지 체크
        if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty && createAccount)
        {
            console.text = "공백은 닉네임이나 비번이 될 수 없습니다.";
            makingAccount_GameObject.SetActive(false);
            playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";
            newNickName_bool = false;
        }
        // 계정생성 때 숫자 제한
        else if (inputNickname.text.Length > 8 || inputPW.text.Length > 8)
        {
            console.text = "닉네임 또는 비번이 8글자를 초과했습니다.";
            makingAccount_GameObject.SetActive(false);
            playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";
            newNickName_bool = false;
        }
        else
        {
            newNickName_bool = true;
        }

        if (createAccount)
        {
            //계정생성 때 현재 비번과 체크비번 비교
            if (inputPWCheck.text != inputPW.text)
            {
                console.text = "비번이 맞지 않습니다.";
                makingAccount_GameObject.SetActive(false);
                playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";

            }
            else
            {
                makingAccount_GameObject.SetActive(true);

            }
        }
        #endregion
    }

    #region// 세팅
    private Button newNickName_Button; // 닉네임 설정 버튼
    private Button soundsetting_Button; // 사운드 세팅 버튼
    private Button mouseSpeedSetting_Button; // 사운드 세팅 버튼

    // true : 세팅화면
    private bool setting = false;

    private void SettiingBT()
    {
        setting = !setting;

        inputNickname.text = string.Empty;
        inputPW.text = string.Empty;

        createAccount = false;

        if (setting)
        {
            textBackGroundGameObject.SetActive(false);

            for (int i = 0; i < setting_Object.Length; i++)
            {
                setting_Object[i].SetActive(true);
            }

            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(false);
            }
        }
        else
        {
            textBackGroundGameObject.SetActive(true);

            for (int i = 0; i < setting_Object.Length; i++)
            {
                setting_Object[i].SetActive(false);
            }

            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(true);
            }
            newNiickName.SetActive(false);
            newNickName_BT_Objcet.SetActive(false);
        }
    }
    #endregion

    #region// 기본 버튼
    [SerializeField] Button createAnAccountOrLogin; // 계정생성 또는 로그인
    [SerializeField] Button gameQuit; // 게임나가기
    [SerializeField] Button settiingButton; // 세팅 버튼

    private void GameQuitBT() // 나가기
    {
        Application.Quit();
    }

    private void GameStartBT() // 시작버튼
    {
        Registaration.Instance.Login(inputNickname.text, inputPW.text, console, Type.logIn, "LogIn");
    }
    #endregion

    #region// 게임방법

    [SerializeField] Button howToGamePlay; // 게임방법
    private void HowToGamePlayBT() // 게임 방법
    {
        // true일 때 게임방법 보는중
        menual = !menual;

        if (menual)
        {
            textBackGroundGameObject.SetActive(false);
            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(true);
            }
            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(false);
            }
        }
        else
        {
            textBackGroundGameObject.SetActive(true);
            // 게임방법 및 랭크 보이게 만들기
            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(false);

            }
            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(true);
            }
        }
    }
    #endregion

    #region// 계정 생성 및 로그인

    private TMP_InputField inputPWCheck; // 비번 확인 인풋
    [SerializeField] TMP_InputField inputNickname; // 닉네임 인풋
    [SerializeField] TMP_InputField inputPW; // 비번 인풋

    [SerializeField] TextMeshProUGUI playerState; // 플레이어 정보 
    [SerializeField] TextMeshProUGUI console; // 설명창   

    private Button gameStart; // 시작버튼
    private Button making; // 계정 만들기

    // true : 게임방법 화면
    private bool menual = false;

    // 계정생성중 인지?    
    private bool createAccount = false;

    private void CreateAnAccountOrLoginBT() // 계정만들기와 로그인 버튼
    {
        // true : 계정생성 화면
        createAccount = !createAccount;

        inputNickname.text = string.Empty;
        inputPW.text = string.Empty;
        inputPWCheck.text = string.Empty;

        // 계정생성 화면이라면 PW확인창 숨기기
        if (createAccount)
        {
            inputPWCheck_GameObject.SetActive(true);
            gameStart_GameObject.SetActive(false);
        }
        else
        {
            gameStart_GameObject.SetActive(true);
            inputPWCheck_GameObject.SetActive(false);
        }

    }

    private void Making() // 계정 생성
    {
        Registaration.Instance.SignUp(inputNickname.text, inputPW.text, console);
    }
    #endregion

    #region// 설정   

    private GameObject[] settingBT = new GameObject[3]; // 0 : 닉네임 설정 버튼, 1 : 사운드 세팅 버튼, 2 : 마우스 세팅 버튼

    [SerializeField] GameObject newNiickName;
    [SerializeField] GameObject newNickName_BT_Objcet;

    private TMP_InputField newNiickName_TMP_InputField;
    private Button newNickNameBT;

    // true : 닉네임 설정창
    private bool nickNameSet;

    // 닉네임 만들어도 되는지?
    private bool newNickName_bool;

    //마우스 민감도 조절
    private void MouseSpeedSetting()
    {

    }

    // 사운드 조절
    private void SoundSetting()
    {

    }

    // 닉네임 바꾸기
    private void NewNickNameSet()
    {       
        nickNameSet = !nickNameSet;

        if (nickNameSet)
        {
            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(true);
            }

            for (int j = 0; j < settingBT.Length; j++)
            {
                settingBT[j].SetActive(false);
            }
            newNiickName.SetActive(true);
            newNickName_BT_Objcet.SetActive(true);
        }
        else
        {
            for (int j = 0; j < logInAndSignUp.Length; j++)
            {
                logInAndSignUp[j].SetActive(false);                
            }

            for (int j = 0; j < settingBT.Length; j++)
            {
                settingBT[j].SetActive(true);
                
            }
            newNickName_BT_Objcet.SetActive(false);
            newNickName_BT_Objcet.SetActive(false);
        }
    }

    private void NewNickNameBT()
    {
        // 만약 닉네임을 바꿀 수 없는 조건이라면 리턴
        if (!newNickName_bool) return;

        Registaration.Instance.Login(inputNickname.text, inputPW.text, console, Type.logIn, newNiickName_TMP_InputField.text);
    }
    #endregion
}
