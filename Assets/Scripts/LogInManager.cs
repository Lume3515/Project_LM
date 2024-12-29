using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // ���Խ�

public class LogInManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname; // �Է� �г���
    [SerializeField] TMP_InputField inputPW; // ���
    [SerializeField] TMP_InputField inputPWCheck; // ���üũ

    [SerializeField] TextMeshProUGUI playerState; // �÷��̾� ����
    [SerializeField] TextMeshProUGUI console; // ����â    
    [SerializeField] TextMeshProUGUI createAccountTMP; // �������� ��ư TMP

    [SerializeField] Button gameStart; // ���۹�ư
    [SerializeField] Button howToGamePlay; // ���ӹ��
    [SerializeField] Button createAnAccountOrLogin; // �������� �Ǵ� �α���
    [SerializeField] Button making; // ���� �����
    [SerializeField] Button deleteAccount; // ���� ����
    [SerializeField] Button gameQuit; // ���ӳ�����

    [SerializeField] GameObject textBackGroundGameObject; // ����ȭ�� �θ� ��ü
    [SerializeField] GameObject[] howToPlayAndRank; // ���ӹ�� �� ��ũ Ȱ��ȭ
    [SerializeField] GameObject createAnAccount; // ���� �����  
    [SerializeField] GameObject inputPWCheck_GameObject;// ���üũ ���ӿ�����Ʈ   

    // ���������� ����?    
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
        //    ��� ��Ģ : ���� ��, �ҹ���, Ư������, ���ڸ� �ϳ��̻��� ������.. > ���Խ�

        if (createAccount)
        {
            // �������� �� ���� ����
            if (inputNickname.text.Length > 8 || inputPW.text.Length > 8)
            {
                console.text = "�г��� �Ǵ� ����� 8���ڸ� �ʰ��߽��ϴ�.";
                createAnAccount.SetActive(false);
                playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";

            }
            //�������� �� ���� ����� üũ��� ��
            else if (inputPWCheck.text != inputPW.text)
            {
                console.text = "����� ���� �ʽ��ϴ�.";
                createAnAccount.SetActive(false);
                playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";
            }
            //�������� �� �������� üũ
            else if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty)
            {
                console.text = "������ �г����̳� ����� �� �� �����ϴ�.";
                createAnAccount.SetActive(false);
                playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";
            }
            else
            {
                createAnAccount.SetActive(true);
            }
        }
    }


    private void GameQuitBT() // ������
    {
        Application.Quit();
    }

    private void GameStartBT() // ���۹�ư
    {
        Registaration.Instance.Login(inputNickname.text, inputPW.text);
    }

    private void HowToGamePlayBT() // ���� ���
    {
        // true�� �� ���ӹ�� ������
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
            // ���ӹ�� �� ��ũ ���̰� �����
            for (int i = 0; i < howToPlayAndRank.Length; i++)
            {
                howToPlayAndRank[i].SetActive(true);
            }
        }
    }

    private void CreateAnAccountOrLoginBT() // ���������� �α��� ��ư
    {
        // true : �������� ȭ��
        createAccount = !createAccount;

        // �������� ȭ���̶�� PWȮ��â �����
        if (createAccount)
        {
            inputPWCheck_GameObject.SetActive(true);
        }
        else
        {
            inputPWCheck_GameObject.SetActive(false);
        }

    }

    private void Making() // ���� ����
    {
        Registaration.Instance.SignUp(inputNickname.text, inputPW.text);
    }

}
