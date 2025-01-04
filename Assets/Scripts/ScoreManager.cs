using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Text;

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
}

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance = null;
    public static ScoreManager Instance => instance;

    private string gameDataRowInDate = string.Empty;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // 데이터 저장
    public void GameDataInsert()
    {
        //Debug.Log(PlayerScore.headShot);

        //Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");

        Param param = new Param();

        // 클 때만 값 변경
        if (PlayerScore.currHeadShot > PlayerScore.headShot) param.Add("headShot", PlayerScore.headShot);
        if (PlayerScore.currBodyShot > PlayerScore.bodyShot) param.Add("bodyShot", PlayerScore.bodyShot);
        if (PlayerScore.currArmShot > PlayerScore.armShot) param.Add("armShot", PlayerScore.armShot);
        if (PlayerScore.currLegShot > PlayerScore.legShot) param.Add("legShot", PlayerScore.legShot);
        if (PlayerScore.currBestScore > PlayerScore.bestScore) param.Add("bestScore", PlayerScore.bestScore);

        //Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("UserData_Kill", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임 정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }
}
