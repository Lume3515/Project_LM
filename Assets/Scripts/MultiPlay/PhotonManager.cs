using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // ������ ��ü ���� => ��𿡼��� �Ŵ����� ������ �� �ֵ��� �̱��� ���� ���
    private static PhotonManager instance = null;
    public static PhotonManager Instance => instance;  

    private void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // ���� ������Ʈ�� Ȱ��ȭ�� �� ���� ȣ��
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ���� �̸�
    private string roomName;
    public string RoomName { set { roomName = value; } }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ���� Ȱ��ȭ�� ���� 0�� ° ���̶�� (Login) ��
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // �� �ڵ� ����ȭ
            PhotonNetwork.AutomaticallySyncScene = true;
            // ���� ����
            PhotonNetwork.GameVersion = roomVersion;

            // ���� �������� �ʴ� ������ ���۷�
            Debug.Log($"���� �������� �ʴ� ������ ���� : {PhotonNetwork.SendRate}");

        } // ���� ��Ƽ�÷��� ���̶��
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {

        }
    }

    // �� ����
    private string roomVersion = "1.0.0";

    public void ConnectTOPhoton()
    {
        // ���� ���濡 ����Ǿ� ���� �ʴٸ�
        if (!PhotonNetwork.IsConnected)
        {
            // ���濡 ����
            PhotonNetwork.ConnectUsingSettings();

        }
    }

    // 2. ������ Ŭ���̾�Ʈ�� ���濡 ������ �� ȣ��Ǵ� �ݹ�
    public override void OnConnectedToMaster()
    {
        Debug.Log($"���濡 ���� ���� ���� : {PhotonNetwork.IsConnected}");

        // 3. �κ�� �����Ѵ�.
        PhotonNetwork.JoinLobby();
    }

    // 4. �κ� �����Ͽ��ٸ� ȣ��Ǵ� �ݹ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"�κ� ���� ���� ���� : {PhotonNetwork.InLobby}");
        // �κ� > ��
        JoinRoom();
    }

    

    // 5. �濡 ����
    public void JoinRoom()
    {
        // ���� �Ӽ� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0; // �ִ� �÷��̾� ���� (0�� ���� ����)
        roomOptions.IsOpen = true; // ���� �������?
        roomOptions.IsVisible = true; // ���� ���̰� ����?

        // 6. �濡 �����ϰų� ����� �Լ�
        // ������ Ŭ���̾�Ʈ�� ��� ���� ����鼭 ���ÿ� �����ϰ� �ǰ�,
        // �� �̿��� ������ ������ �ϰ� �ȴ�.
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);     
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        MainMenuManager.Instance.Console_Room_TMP.text = "�� ���� �Ϸ�";
    }

    // �濡 ���� �� ȣ��
    public override void OnJoinedRoom()
    {
        MainMenuManager.Instance.Console_Room_TMP.text = ($"�濡 ���� ���� ���� : {PhotonNetwork.InRoom}, �ο� �� : {PhotonNetwork.CurrentRoom.PlayerCount} ");
        // 7. ���ξ� �ҷ�����
        LoadMainScene();
    }

    // ���ξ� �ε�
    // ������ Ŭ���̾�Ʈ�� ��� ���� ���� �ε�
    // �Ϲ� ������ ��� �����Ͱ� �ε��� ���� �ڵ� ����ȭ�˴ϴ�.
    private void LoadMainScene()
    {
        Debug.Log("���� �� �ε� �õ�...");

        // ������ Ŭ���̾�Ʈ�� ��� ���ξ� �ҷ�����
        if (PhotonNetwork.IsMasterClient)
        {
            LoadingManager.name_Scene = "MultiPlay";
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("������ Ŭ���̾�Ʈ�� �ƴ�");
        }
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �ۼ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MainMenuManager.Instance.Console_Room_TMP.text = "���� ���ų� ���� �̸��� Ʋ�Ƚ��ϴ�.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MainMenuManager.Instance.Console_Room_TMP.text = message;
    }

}
