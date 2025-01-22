using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Text;
using LitJson;

public static class PlayerScore
{
    // 누적 점수
    public static int headShot; // 머리 샷
    public static int bodyShot; // 몸 샷
    public static int armShot; // 팔 샷
    public static int legShot; // 다리 샷
    public static int bestScore; // 최고점수  

    // 현재 점수
    public static int currHeadShot; // 머리 샷
    public static int currBodyShot; // 몸 샷
    public static int currArmShot; // 팔 샷
    public static int currLegShot; // 다리 샷
    public static int currBestScore; // 최고점수
    public static int basicZombie;// 일반좀비
    public static int tankerZombie;// 탱커좀비
    public static int speedZombie; // 속도 좀비   
    public static int enemyGun; // 총쏘는 적
}

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance = null;
    public static ScoreManager Instance => instance;

    private string gameDataRowInDate = string.Empty;   

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    //// 데이터 저장
    public void GameDataInsert_kill()
    {
        //Debug.Log(PlayerScore.headShot);

        //Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");

        PlayerScore.currBestScore = (PlayerScore.currHeadShot * 10) + (PlayerScore.currBodyShot * 6) +
            (PlayerScore.currArmShot * 4) + (PlayerScore.currLegShot * 2) + (PlayerScore.basicZombie * 5) + (PlayerScore.speedZombie * 20) + (PlayerScore.tankerZombie * 10);

        Param param = new Param();

        // 클 때만 값 변경
        /*  if (PlayerScore.currHeadShot > PlayerScore.headShot) */
        param.Add("headShot", PlayerScore.currHeadShot);
        /*if (PlayerScore.currBodyShot > PlayerScore.bodyShot)*/
        param.Add("bodyShot", PlayerScore.currBodyShot);
        /*if (PlayerScore.currArmShot > PlayerScore.armShot) */
        param.Add("armShot", PlayerScore.currArmShot);
        /*if (PlayerScore.currLegShot > PlayerScore.legShot)*/
        param.Add("legShot", PlayerScore.currLegShot);
        /*if (PlayerScore.currBestScore > PlayerScore.bestScore)*/
        param.Add("bestScore", PlayerScore.currBestScore);

        //Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("UserData_Kill", param);           

        if (bro.IsSuccess())
        {

            Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임 정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();

            BackendRankManager.Instance.RankInsert_BestScore(PlayerScore.currBestScore);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    // 데이터 불러오기
    public void GameDataGet_Kill()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");

        var bro = Backend.GameData.GetMyData("UserData_Kill", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            //Debug.Log(gameDataJson);

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {              
                Debug.LogWarning("데이터가 존재하지 않습니다.");
                GameDataInsert_kill();
            }
            else
            {                             

                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                PlayerScore.headShot = int.Parse(gameDataJson[0]["headShot"].ToString());
                PlayerScore.bodyShot = int.Parse(gameDataJson[0]["bodyShot"].ToString());
                PlayerScore.armShot = int.Parse(gameDataJson[0]["armShot"].ToString());
                PlayerScore.legShot = int.Parse(gameDataJson[0]["legShot"].ToString());
                PlayerScore.bestScore = int.Parse(gameDataJson[0]["bestScore"].ToString());

                Debug.Log("데이터의 갯수가 있습니다");

                GameDataUpdate_Kill();
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }

    // 게임 정보 수정하기
    public void GameDataUpdate_Kill()
    {
        //if (userData == null)
        //{
        //    Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
        //    return;
        //}

        PlayerScore.currBestScore = (PlayerScore.currHeadShot * 10) + (PlayerScore.currBodyShot * 6) +
             (PlayerScore.currArmShot * 4) + (PlayerScore.currLegShot * 2) + (PlayerScore.basicZombie * 5) + (PlayerScore.speedZombie * 20) + (PlayerScore.tankerZombie * 10);

        Param param = new Param();
        if (PlayerScore.currHeadShot > PlayerScore.headShot) param.Add("headShot", PlayerScore.currHeadShot);
        if (PlayerScore.currBodyShot > PlayerScore.bodyShot) param.Add("bodyShot", PlayerScore.currBodyShot);
        if (PlayerScore.currArmShot > PlayerScore.armShot) param.Add("armShot", PlayerScore.currArmShot);
        if (PlayerScore.currLegShot > PlayerScore.legShot) param.Add("legShot", PlayerScore.currLegShot);
        if (PlayerScore.currBestScore > PlayerScore.bestScore) param.Add("bestScore", PlayerScore.currBestScore);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("UserData_Kill", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("UserData_Kill", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + bro);

            BackendRankManager.Instance.RankInsert_BestScore(PlayerScore.bestScore);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}
