using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public static class PlayerInformation
{
    public static string playerNickName;
}

public class LogInManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname; // 입력 닉네임
    [SerializeField] TMP_InputField inputPW; // 비번
    [SerializeField] TMP_InputField inputPWCheck; // 비번체크

    [SerializeField] TextMeshProUGUI playerState; // 플레이어 정보
    [SerializeField] TextMeshProUGUI console; // 정보창    
    [SerializeField] TextMeshProUGUI createAccountTMP;

    [SerializeField] Button gameStart; // 시작버튼
    [SerializeField] Button howToGamePlay; // 게임방법
    [SerializeField] Button createAnAccountOrLogin; // 계정생성 또는 로그인
    [SerializeField] Button making; // 계정 만들기
    [SerializeField] Button deleteAccount; // 계정 삭제
    [SerializeField] Button gameQuit; // 게임나가기

    [SerializeField] GameObject gameStartGamaObject;
    [SerializeField] GameObject textBackGroundGameObject;
    [SerializeField] GameObject[] howToPlayAndRank; // 게임방법 및 랭크 활성화
    [SerializeField] GameObject createAnAccount; // 계정 만들기  
    [SerializeField] GameObject inputPWCheck_GameObject;


    private List<string> playerNickNameList = new List<string>();
    private List<string> playerPWList = new List<string>();


    private bool exit = false;
    private bool createAccountBool = false;




    public void Start()
    {
        gameStart.onClick.AddListener(GameStartBT);
        howToGamePlay.onClick.AddListener(HowToGamePlayBT);
        making.onClick.AddListener(Making);
        createAnAccountOrLogin.onClick.AddListener(CreateAnAccountOrLoginBT);
        deleteAccount.onClick.AddListener(deleteAccountBT);

        gameQuit.onClick.AddListener(GameQuitBT);

    }

    private void Update()
    {


        if (inputNickname.text.Length > 8 || inputPW.text.Length > 8)
        {

            console.text = "닉네임 또는 비번이 8글자를 초과했습니다.";
            gameStartGamaObject.SetActive(false);
            createAnAccount.SetActive(false);
            playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :\n잡은 좀비 수 :";

        }
        else if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty)
        {

            console.text = "공백은 닉네임이나 비번이 될 수 없습니다.";
            gameStartGamaObject.SetActive(false);
            createAnAccount.SetActive(false);
            playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 :";
        }
        else
        {

            for (int i = 0; i < playerNickNameList.Count; i++)
            {
                if (playerNickNameList[i] == inputNickname.text)// 중복 확인
                {
                    playerState.text = $"플레이어 닉네임 : {playerNickNameList[i]}\n플레이어 레벨 :";
                }
            }

            if (!createAccountBool)
            {
                gameStartGamaObject.SetActive(true);

            }
            else
            {
                if (inputPWCheck.text == inputPW.text)
                {


                    createAnAccount.SetActive(true);
                }
                else
                {
                    createAnAccount.SetActive(false);
                }

            }

        }
    }

    private void GameQuitBT() // 나가기
    {
        Application.Quit();
    }

    private void GameStartBT() // 시작버튼
    {
        for (int i = 0; i < playerNickNameList.Count; i++)
        {
            if (playerNickNameList[i] == inputNickname.text && playerPWList[i] == inputNickname.text)// 중복 확인
            {
                console.text = "로그인 중 입니다.";
                PlayerInformation.playerNickName = playerNickNameList[i];
                SceneManager.LoadScene(1);


            }
            else
            {
                console.text = "같은 닉네임이나 비번이 없습니다.";
            }



        }



    }

    private void HowToGamePlayBT() // 게임 방법
    {

        if (exit)
        {
            textBackGroundGameObject.SetActive(true);

            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(false);
            }
            exit = false;
        }
        else
        {
            textBackGroundGameObject.SetActive(false);
            exit = true;

            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(true);
            }
        }

    }

    private void CreateAnAccountOrLoginBT() // 계정만들기와 로그인 버튼
    {
        inputNickname.text = string.Empty;
        inputPW.text = string.Empty;
        playerState.text = $"플레이어 닉네임 : \n플레이어 레벨 : ";

        createAccountBool = !createAccountBool;

        if (createAccountBool)
        {
            inputPWCheck_GameObject.SetActive(true);
            createAccountTMP.text = "계정 생성하기";

            createAnAccount.SetActive(false);
        }
        else if (!createAccountBool)
        {

            inputPWCheck_GameObject.SetActive(false);
            createAccountTMP.text = "로그인 하기";

        }
    }

    private void Making() // 계정 생성
    {
        int count = 0;

        for (int i = 0; i < playerNickNameList.Count; i++)
        {

            if (playerNickNameList[i] == inputNickname.text || inputPW.text == playerPWList[i])// 중복 확인
            {
                count++;

            }


        }
        if (count == 0)
        {

            playerNickNameList.Add(inputNickname.text);
            playerPWList.Add(inputPW.text);

            console.text = "계정 생성 완료";
        }
        else
        {
            console.text = "같은 이름이 있습니다.";

        }




    }

    private void deleteAccountBT()
    {
        for (int i = 0; i < playerNickNameList.Count; i++)
        {
            if (playerNickNameList[i] == inputNickname.text)// 중복 확인
            {
                playerNickNameList.Remove(inputNickname.text);
                console.text = $"{inputNickname.text}님 삭제 완료";

                break;
            }

        }

        console.text = $"{inputNickname.text}님이 없습니다.";
    }

}
