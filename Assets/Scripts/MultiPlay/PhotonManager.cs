using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 유일한 객체 선언 => 어디에서나 매니저에 접근할 수 있도록 싱글톤 패턴 사용
    private static PhotonManager instance = null;
    public static PhotonManager Instance => instance;  

    private void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // 게임 오브젝트가 활성화될 때 마다 호출
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 방의 이름
    private string roomName;
    public string RoomName { set { roomName = value; } }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 만약 현재 활성화된 씬이 0번 째 씬이라면 (Login) 씬
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // 씬 자동 동기화
            PhotonNetwork.AutomaticallySyncScene = true;
            // 버전 설정
            PhotonNetwork.GameVersion = roomVersion;

            // 포톤 서버와의 초당 데이터 전송량
            Debug.Log($"포톤 서버와의 초당 데이터 전송 : {PhotonNetwork.SendRate}");

        } // 메인 멀티플레이 씬이라면
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {

        }
    }

    // 방 버전
    private string roomVersion = "1.0.0";

    public void ConnectTOPhoton()
    {
        // 만약 포톤에 연결되어 있지 않다면
        if (!PhotonNetwork.IsConnected)
        {
            // 포톤에 연결
            PhotonNetwork.ConnectUsingSettings();

        }
    }

    // 2. 마스터 클라이언트가 포톤에 접속할 시 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log($"포톤에 접속 성공 유무 : {PhotonNetwork.IsConnected}");

        // 3. 로비로 접속한다.
        PhotonNetwork.JoinLobby();
    }

    // 4. 로비에 접속하였다면 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log($"로비에 접속 성공 유무 : {PhotonNetwork.InLobby}");
        // 로비 > 룸
        JoinRoom();
    }

    

    // 5. 방에 접속
    public void JoinRoom()
    {
        // 방의 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0; // 최대 플레이어 제한 (0은 제한 없음)
        roomOptions.IsOpen = true; // 방을 열어둘지?
        roomOptions.IsVisible = true; // 방을 보이게 할지?

        // 6. 방에 참가하거나 만드는 함수
        // 마스터 클라이언트의 경우 방을 만들면서 동시에 참가하게 되고,
        // 그 이외의 유저는 조인을 하게 된다.
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);     
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        MainMenuManager.Instance.Console_Room_TMP.text = "방 생성 완료";
    }

    // 방에 입장 시 호출
    public override void OnJoinedRoom()
    {
        MainMenuManager.Instance.Console_Room_TMP.text = ($"방에 입장 성공 유무 : {PhotonNetwork.InRoom}, 인원 수 : {PhotonNetwork.CurrentRoom.PlayerCount} ");
        // 7. 메인씬 불러오기
        LoadMainScene();
    }

    // 메인씬 로드
    // 마스터 클라이언트의 경우 메인 씬을 로드
    // 일반 유저의 경우 마스터가 로드한 씬과 자동 동기화됩니다.
    private void LoadMainScene()
    {
        Debug.Log("메인 씬 로드 시도...");

        // 마스터 클라이언트인 경우 메인씬 불러오기
        if (PhotonNetwork.IsMasterClient)
        {
            LoadingManager.name_Scene = "MultiPlay";
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아님");
        }
    }

    // 방 접속 실패 시 호출되는 콜백 작성
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MainMenuManager.Instance.Console_Room_TMP.text = "방이 없거나 방의 이름이 틀렸습니다.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MainMenuManager.Instance.Console_Room_TMP.text = message;
    }

}
