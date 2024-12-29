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
    [SerializeField] TMP_InputField inputNickname; // �Է� �г���
    [SerializeField] TMP_InputField inputPW; // ���
    [SerializeField] TMP_InputField inputPWCheck; // ���üũ

    [SerializeField] TextMeshProUGUI playerState; // �÷��̾� ����
    [SerializeField] TextMeshProUGUI console; // ����â    
    [SerializeField] TextMeshProUGUI createAccountTMP;

    [SerializeField] Button gameStart; // ���۹�ư
    [SerializeField] Button howToGamePlay; // ���ӹ��
    [SerializeField] Button createAnAccountOrLogin; // �������� �Ǵ� �α���
    [SerializeField] Button making; // ���� �����
    [SerializeField] Button deleteAccount; // ���� ����
    [SerializeField] Button gameQuit; // ���ӳ�����

    [SerializeField] GameObject gameStartGamaObject;
    [SerializeField] GameObject textBackGroundGameObject;
    [SerializeField] GameObject[] howToPlayAndRank; // ���ӹ�� �� ��ũ Ȱ��ȭ
    [SerializeField] GameObject createAnAccount; // ���� �����  
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

            console.text = "�г��� �Ǵ� ����� 8���ڸ� �ʰ��߽��ϴ�.";
            gameStartGamaObject.SetActive(false);
            createAnAccount.SetActive(false);
            playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :\n���� ���� �� :";

        }
        else if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty)
        {

            console.text = "������ �г����̳� ����� �� �� �����ϴ�.";
            gameStartGamaObject.SetActive(false);
            createAnAccount.SetActive(false);
            playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";
        }
        else
        {

            for (int i = 0; i < playerNickNameList.Count; i++)
            {
                if (playerNickNameList[i] == inputNickname.text)// �ߺ� Ȯ��
                {
                    playerState.text = $"�÷��̾� �г��� : {playerNickNameList[i]}\n�÷��̾� ���� :";
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

    private void GameQuitBT() // ������
    {
        Application.Quit();
    }

    private void GameStartBT() // ���۹�ư
    {
        for (int i = 0; i < playerNickNameList.Count; i++)
        {
            if (playerNickNameList[i] == inputNickname.text && playerPWList[i] == inputNickname.text)// �ߺ� Ȯ��
            {
                console.text = "�α��� �� �Դϴ�.";
                PlayerInformation.playerNickName = playerNickNameList[i];
                SceneManager.LoadScene(1);


            }
            else
            {
                console.text = "���� �г����̳� ����� �����ϴ�.";
            }



        }



    }

    private void HowToGamePlayBT() // ���� ���
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

    private void CreateAnAccountOrLoginBT() // ���������� �α��� ��ư
    {
        inputNickname.text = string.Empty;
        inputPW.text = string.Empty;
        playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� : ";

        createAccountBool = !createAccountBool;

        if (createAccountBool)
        {
            inputPWCheck_GameObject.SetActive(true);
            createAccountTMP.text = "���� �����ϱ�";

            createAnAccount.SetActive(false);
        }
        else if (!createAccountBool)
        {

            inputPWCheck_GameObject.SetActive(false);
            createAccountTMP.text = "�α��� �ϱ�";

        }
    }

    private void Making() // ���� ����
    {
        int count = 0;

        for (int i = 0; i < playerNickNameList.Count; i++)
        {

            if (playerNickNameList[i] == inputNickname.text || inputPW.text == playerPWList[i])// �ߺ� Ȯ��
            {
                count++;

            }


        }
        if (count == 0)
        {

            playerNickNameList.Add(inputNickname.text);
            playerPWList.Add(inputPW.text);

            console.text = "���� ���� �Ϸ�";
        }
        else
        {
            console.text = "���� �̸��� �ֽ��ϴ�.";

        }




    }

    private void deleteAccountBT()
    {
        for (int i = 0; i < playerNickNameList.Count; i++)
        {
            if (playerNickNameList[i] == inputNickname.text)// �ߺ� Ȯ��
            {
                playerNickNameList.Remove(inputNickname.text);
                console.text = $"{inputNickname.text}�� ���� �Ϸ�";

                break;
            }

        }

        console.text = $"{inputNickname.text}���� �����ϴ�.";
    }

}
