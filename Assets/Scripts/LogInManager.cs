using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // 정규식

public class LogInManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname; // 입력 닉네임
    [SerializeField] TMP_InputField inputPW; // 비번
    [SerializeField] TMP_InputField inputPWCheck; // 비번체크

    [SerializeField] TextMeshProUGUI playerState; // 플레이어 정보
    [SerializeField] TextMeshProUGUI console; // 정보창    
    [SerializeField] TextMeshProUGUI createAccountTMP; // 계정생성 버튼 TMP

    [SerializeField] Button gameStart; // 시작버튼
    [SerializeField] Button howToGamePlay; // 게임방법
    [SerializeField] Button createAnAccountOrLogin; // 계정생성 또는 로그인
    [SerializeField] Button making; // 계정 만들기
    [SerializeField] Button deleteAccount; // 계정 삭제
    [SerializeField] Button gameQuit; // 게임나가기

    [SerializeField] GameObject textBackGroundGameObject; // 시작화면 부모 객체
    [SerializeField] GameObject[] howToPlayAndRank; // 게임방법 및 랭크 활성화
    [SerializeField] GameObject createAnAccount; // 계정 만들기  
    [SerializeField] GameObject inputPWCheck_GameObject;// 비번체크 게임오브젝트   

    // 계정생성중 인지?    
    private bool createAccount = false;

    private bool menual = false;

    public void Start()
    {

        gameStart.onClick.AddListener(GameStartBT);
        howToGamePlay.onClick.AddListener(HowToGamePlayBT);
        making.onClick.AddListener(Making);
        createAnAccountOrLogin.onClick.AddListener(CreateAnAccountOrLoginBT);
        gameQuit.onClick.AddListener(GameQuitBT);
    }

    private void Update()
    {
        //    비번 규칙 : 영문 대, 소문자, 특수문자, 숫자를 하나이상을 조합한.. > 정규식

        if (createAccount)
        {
            // 계정생성 때 숫자 제한
            if (inputNickname.text.Length > 8 || inputPW.text.Length > 8)
            {
                console.text = "닉네임 또는 비번이 8글자를 초과했습니다.";
                createAnAccount.SetActive(false);
                playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";

            }
            //계정생성 때 현재 비번과 체크비번 비교
            else if (inputPWCheck.text != inputPW.text)
            {
                console.text = "비번이 맞지 않습니다.";
                createAnAccount.SetActive(false);
                playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";
            }
            //계정생성 때 공백인지 체크
            else if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty)
            {
                console.text = "공백은 닉네임이나 비번이 될 수 없습니다.";
                createAnAccount.SetActive(false);
                playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";
            }
            else
            {
                createAnAccount.SetActive(true);
            }
        }
    }


    private void GameQuitBT() // 나가기
    {
        Application.Quit();
    }

    private void GameStartBT() // 시작버튼
    {
        Registaration.Instance.Login(inputNickname.text, inputPW.text);
    }

    private void HowToGamePlayBT() // 게임 방법
    {
        // true일 때 게임방법 보는중
        menual = !menual;

        if (menual)
        {
            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(false);
            }
        }
        else
        {
            // 게임방법 및 랭크 보이게 만들기
            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(true);
            }
        }
    }

    private void CreateAnAccountOrLoginBT() // 계정만들기와 로그인 버튼
    {
        // true : 계정생성 화면
        createAccount = !createAccount;

        // 계정생성 화면이라면 PW확인창 숨기기
        if (createAccount)
        {
            inputPWCheck_GameObject.SetActive(true);
        }
        else
        {
            inputPWCheck_GameObject.SetActive(false);
        }

    }

    private void Making() // 계정 생성
    {
        Registaration.Instance.SignUp(inputNickname.text, inputPW.text);
    }

}
