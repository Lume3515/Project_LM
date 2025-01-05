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
        #region// �α��� & ��� & üũ

        for (int i = 0; i < logInAndSignUPAndCheck.Length; i++)
        {
            logInAndSignUPAndCheck_InputField[i] = logInAndSignUPAndCheck[i].GetComponent<TMP_InputField>();
            //Debug.Log(logInAndSignUPAndCheck[i].GetComponent<TMP_InputField>() == null);
        }

        createOrLogIn_TMP = createOrLogIn_BT.GetComponentInChildren<TextMeshProUGUI>();

        console_GameObject = console.gameObject;

        createOrLogIn_GameObject = createOrLogIn_BT.gameObject;

        #endregion

        #region// ���� UI

        isMainMenu = false;

        #endregion     
    }

    private void Update()
    {
        if (mainUI_LoginAndSignUpAndNewNickName)
        {
            //Debug.Log("1");

            // ȸ�� �����̳� �α��� ���� �� �����̶�� 
            if (logInAndSignUPAndCheck_InputField[0].text == string.Empty || logInAndSignUPAndCheck_InputField[1].text == string.Empty || logInAndSignUPAndCheck_InputField[2].text == string.Empty &&  signUp)
            {
                console.text = "���̵� ����� �Է����ּ���.";
                isRegistaration = false;
                //Debug.Log("2");
            }
            // 8�� �ʰ����
            else if (logInAndSignUPAndCheck_InputField[0].text.Length > 8 || logInAndSignUPAndCheck_InputField[1].text.Length > 8 || logInAndSignUPAndCheck_InputField[2].text.Length > 8)
            {
                console.text = "8�� ���� �ۼ� �����մϴ�.";
                isRegistaration = false;
            }
            // ��� Ȯ���� ���� ��
            else if (logInAndSignUPAndCheck_InputField[1].text == logInAndSignUPAndCheck_InputField[2].text && !clickCreate)
            {
                console.text = "����� �½��ϴ�.";
                isRegistaration = true;
            }
            // ��� Ȯ���� ���� ���� ��
            else if ((logInAndSignUPAndCheck_InputField[1].text != logInAndSignUPAndCheck_InputField[2].text) && !newNickName && signUp)
            {
                console.text = "���Ȯ���� �� ���� �ʽ��ϴ�.";                
                isRegistaration = false;
            }
            else
            {
                isRegistaration = true;
            }
        }

    }

    #region// ����
    // ���� �޴�����?
    private bool isMainMenu;

    #endregion

    #region// Ÿ��Ʋ 
    // ��ư �������� > ��ư ������Ʈ ����

    // ��ư�� 0 : ���ӽ���, 1 : ���� Ʃ�丮��
    [SerializeField] Button[] titleButton_1;

    // ��ư�� 0 : ���Ӽ���, 1 : ȸ������, 2 : ���� ����
    [SerializeField] Button[] titleButton_2;


    // ��ư�� 0 : ���ӽ���, 1 : ���� Ʃ�丮��, 2 : ���� ��ũ
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
                createOrLogIn_TMP.text = "�α���";
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

    // ��ư�� 0 : ���Ӽ���, 1 : �к�, 2 : ȸ������, 3 : ��������
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

                logInAndSignUPAndCheck_InputField[2].placeholder.GetComponent<TextMeshProUGUI>().text = "������ �г���";
                mainUI_LoginAndSignUpAndNewNickName = true;
                isMainMenu = true;
                createOrLogIn_TMP.text = "�г��� ����";
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
                createOrLogIn_TMP.text = "���� ����";
                mainUI_LoginAndSignUpAndNewNickName = true;
                mainUIParent.SetActive(true);
                logInAndSignUPAndCheck[2].SetActive(true);
                logInAndSignUPAndCheck_InputField[2].placeholder.GetComponent<TextMeshProUGUI>().text = "��й�ȣ Ȯ��";
                createOrLogIn_GameObject.SetActive(true);
                console_GameObject.SetActive(true);
                break;

            case 3:
                Application.Quit();
                break;


        }
    }


    #endregion

    #region// ���� UI
    // ���� �޴� UI������ ��ư > ��ư ������Ʈ���� �߰�
    [SerializeField] Button exitMainUI;

    public void ExitMainUIBT()
    {
        mainUIParent.SetActive(false);
        isMainMenu = false;
        mainUI_LoginAndSignUpAndNewNickName = false;

        // ���� ���
        for (int i = 0; i < logInAndSignUPAndCheck_InputField.Length; i++)
        {
            logInAndSignUPAndCheck_InputField[i].text = string.Empty;
        }
    }

    #endregion

    #region// �α��� & ���

    // ���Խ� > ��� �ۼ� �����ϰ� �پ�� �ȵ�
    string pattern = @"^[a-zA-Z]+$";

    // �ܼ� ���ӿ�����Ʈ
    private GameObject console_GameObject;

    // ����UI �ֻ��� �θ�
    [SerializeField] GameObject mainUIParent;

    // �ܼ� 
    [SerializeField] TextMeshProUGUI console;

    // �α��� & ��� & ��� äũ(0 : �̸�, 1 : ���, 2 : ��� üũ & ���ο� �г���)
    [SerializeField] GameObject[] logInAndSignUPAndCheck;

    // (0 : �̸�, 1 : ���, 2 : ��� üũ & ���ο� �г���)
    private TMP_InputField[] logInAndSignUPAndCheck_InputField = new TMP_InputField[3];

    [SerializeField] Button createOrLogIn_BT; // > ��ư ������Ʈ���� �߰�

    private TextMeshProUGUI createOrLogIn_TMP;

    private GameObject createOrLogIn_GameObject;

    // ������������? // true : ���� ������, �ƴϸ� : �α���
    private bool signUp;

    // ���θ޴� UI���� �α����̳� ȸ�������̳� �к�������?
    private bool mainUI_LoginAndSignUpAndNewNickName;

    // �α����̳� ȸ������, �к� ����?
    private bool isRegistaration;

    // �α����̳� �к�, ���Թ�ư�� ��������? true : �α����̳� �к�, ���Թ�ư����
    private bool clickCreate;

    // �� �г����� ���� �ִ���?
    private bool newNickName;

    // ���� ���� �Ǵ� �α��� ��ư �Ǵ� �г��� ����
    public void CreateOrLogInBT()
    {
        if (!isRegistaration) return;

        clickCreate = true;

        if (createOrLogIn_TMP.text == "���� ����")
        {
            // ���Խ� �˻� > true > ���
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "��й�ȣ�� �ѱ�, ������ �������� ������.";

                return;
            }        
                Registaration.Instance.SignUp(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console);
            
        }
        else if (createOrLogIn_TMP.text == "�α���")
        {
            // ���Խ� �˻� > true > ���
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "��й�ȣ�� �ѱ�, ������ �������� ������.";

                return;
            }

            Registaration.Instance.Login(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console, LogInType.logIn, "null");
        }
        else if (createOrLogIn_TMP.text == "�г��� ����")
        {
            // ���Խ� �˻� > true > ���
            if (!Pattern(logInAndSignUPAndCheck_InputField[1].text))
            {
                console.text = "��й�ȣ�� �ѱ�, ������ �������� ������.";

                return;
            }

            Registaration.Instance.Login(logInAndSignUPAndCheck_InputField[0].text, logInAndSignUPAndCheck_InputField[1].text, console, LogInType.newName, logInAndSignUPAndCheck_InputField[2].text);
        }

        clickCreate = false;
    }

    // ���Խ�
    private bool Pattern(string text)
    {
        Debug.Log(Regex.IsMatch(text, pattern));

        // ���Խ� ����?
         return Regex.IsMatch(text, pattern);        
    }

    #endregion
}
