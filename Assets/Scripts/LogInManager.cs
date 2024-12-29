using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // ���Խ�

public class LogInManager : MonoBehaviour
{
    //[SerializeField] Button deleteAccount; // ���� ����

    [SerializeField] GameObject textBackGroundGameObject; // ����ȭ�� �θ� ��ü
    [SerializeField] GameObject[] howToPlayAndRank; // ���ӹ�� �� ��ũ Ȱ��ȭ
    [SerializeField] GameObject[] setting_Object; // ���� ������Ʈ
    [SerializeField] GameObject makingAccount_GameObject; // ���� �����  
    [SerializeField] GameObject inputPWCheck_GameObject;// ���üũ ���ӿ�����Ʈ   
    [SerializeField] GameObject gameStart_GameObject; // ���۹�ư ��ü
    [SerializeField] GameObject[] logInAndSignUp; // �α��ΰ� ����, �ܼ�����
    [SerializeField] GameObject newNickName; // �г��� ���� ��ư
    [SerializeField] GameObject soundsetting; // ���� ���� ��ư
    [SerializeField] GameObject mouseSpeedSetting; // ���콺 ���� ��ư            

    public void Start()
    {
        #region// �Ҵ�
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

        #region// ��ư �߰�
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
        #region// ����
        // �������� üũ
        if (inputNickname.text == string.Empty || inputPW.text == string.Empty || inputPWCheck.text == string.Empty && createAccount)
        {
            console.text = "������ �г����̳� ����� �� �� �����ϴ�.";
            makingAccount_GameObject.SetActive(false);
            playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";
            newNickName_bool = false;
        }
        // �������� �� ���� ����
        else if (inputNickname.text.Length > 8 || inputPW.text.Length > 8)
        {
            console.text = "�г��� �Ǵ� ����� 8���ڸ� �ʰ��߽��ϴ�.";
            makingAccount_GameObject.SetActive(false);
            playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";
            newNickName_bool = false;
        }
        else
        {
            newNickName_bool = true;
        }

        if (createAccount)
        {
            //�������� �� ���� ����� üũ��� ��
            if (inputPWCheck.text != inputPW.text)
            {
                console.text = "����� ���� �ʽ��ϴ�.";
                makingAccount_GameObject.SetActive(false);
                playerState.text = $"�÷��̾� �г��� : \n�÷��̾� ���� :";

            }
            else
            {
                makingAccount_GameObject.SetActive(true);

            }
        }
        #endregion
    }

    #region// ����
    private Button newNickName_Button; // �г��� ���� ��ư
    private Button soundsetting_Button; // ���� ���� ��ư
    private Button mouseSpeedSetting_Button; // ���� ���� ��ư

    // true : ����ȭ��
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

    #region// �⺻ ��ư
    [SerializeField] Button createAnAccountOrLogin; // �������� �Ǵ� �α���
    [SerializeField] Button gameQuit; // ���ӳ�����
    [SerializeField] Button settiingButton; // ���� ��ư

    private void GameQuitBT() // ������
    {
        Application.Quit();
    }

    private void GameStartBT() // ���۹�ư
    {
        Registaration.Instance.Login(inputNickname.text, inputPW.text, console, Type.logIn, "LogIn");
    }
    #endregion

    #region// ���ӹ��

    [SerializeField] Button howToGamePlay; // ���ӹ��
    private void HowToGamePlayBT() // ���� ���
    {
        // true�� �� ���ӹ�� ������
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
            // ���ӹ�� �� ��ũ ���̰� �����
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

    #region// ���� ���� �� �α���

    private TMP_InputField inputPWCheck; // ��� Ȯ�� ��ǲ
    [SerializeField] TMP_InputField inputNickname; // �г��� ��ǲ
    [SerializeField] TMP_InputField inputPW; // ��� ��ǲ

    [SerializeField] TextMeshProUGUI playerState; // �÷��̾� ���� 
    [SerializeField] TextMeshProUGUI console; // ����â   

    private Button gameStart; // ���۹�ư
    private Button making; // ���� �����

    // true : ���ӹ�� ȭ��
    private bool menual = false;

    // ���������� ����?    
    private bool createAccount = false;

    private void CreateAnAccountOrLoginBT() // ���������� �α��� ��ư
    {
        // true : �������� ȭ��
        createAccount = !createAccount;

        inputNickname.text = string.Empty;
        inputPW.text = string.Empty;
        inputPWCheck.text = string.Empty;

        // �������� ȭ���̶�� PWȮ��â �����
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

    private void Making() // ���� ����
    {
        Registaration.Instance.SignUp(inputNickname.text, inputPW.text, console);
    }
    #endregion

    #region// ����   

    private GameObject[] settingBT = new GameObject[3]; // 0 : �г��� ���� ��ư, 1 : ���� ���� ��ư, 2 : ���콺 ���� ��ư

    [SerializeField] GameObject newNiickName;
    [SerializeField] GameObject newNickName_BT_Objcet;

    private TMP_InputField newNiickName_TMP_InputField;
    private Button newNickNameBT;

    // true : �г��� ����â
    private bool nickNameSet;

    // �г��� ���� �Ǵ���?
    private bool newNickName_bool;

    //���콺 �ΰ��� ����
    private void MouseSpeedSetting()
    {

    }

    // ���� ����
    private void SoundSetting()
    {

    }

    // �г��� �ٲٱ�
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
        // ���� �г����� �ٲ� �� ���� �����̶�� ����
        if (!newNickName_bool) return;

        Registaration.Instance.Login(inputNickname.text, inputPW.text, console, Type.logIn, newNiickName_TMP_InputField.text);
    }
    #endregion
}
