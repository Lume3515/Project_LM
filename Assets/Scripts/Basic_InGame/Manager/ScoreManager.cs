using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Text;
using LitJson;

public static class PlayerScore
{
    // ���� ����
    public static int headShot; // �Ӹ� ��
    public static int bodyShot; // �� ��
    public static int armShot; // �� ��
    public static int legShot; // �ٸ� ��
    public static int bestScore; // �ְ�����  

    // ���� ����
    public static int currHeadShot; // �Ӹ� ��
    public static int currBodyShot; // �� ��
    public static int currArmShot; // �� ��
    public static int currLegShot; // �ٸ� ��
    public static int currBestScore; // �ְ�����
    public static int basicZombie;// �Ϲ�����
    public static int tankerZombie;// ��Ŀ����
    public static int speedZombie; // �ӵ� ����   
    public static int enemyGun; // �ѽ�� ��
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

    //// ������ ����
    public void GameDataInsert_kill()
    {
        //Debug.Log(PlayerScore.headShot);

        //Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");

        PlayerScore.currBestScore = (PlayerScore.currHeadShot * 10) + (PlayerScore.currBodyShot * 6) +
            (PlayerScore.currArmShot * 4) + (PlayerScore.currLegShot * 2) + (PlayerScore.basicZombie * 5) + (PlayerScore.speedZombie * 20) + (PlayerScore.tankerZombie * 10);

        Param param = new Param();

        // Ŭ ���� �� ����
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

        //Debug.Log("���� ���� ������ ������ ��û�մϴ�.");
        var bro = Backend.GameData.Insert("UserData_Kill", param);           

        if (bro.IsSuccess())
        {

            Debug.Log("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���� ������ �������Դϴ�.  
            gameDataRowInDate = bro.GetInDate();

            BackendRankManager.Instance.RankInsert_BestScore(PlayerScore.currBestScore);
        }
        else
        {
            Debug.LogError("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }

    // ������ �ҷ�����
    public void GameDataGet_Kill()
    {
        Debug.Log("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");

        var bro = Backend.GameData.GetMyData("UserData_Kill", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.  

            //Debug.Log(gameDataJson);

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.  
            if (gameDataJson.Count <= 0)
            {              
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                GameDataInsert_kill();
            }
            else
            {                             

                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���� ������ �������Դϴ�.  

                PlayerScore.headShot = int.Parse(gameDataJson[0]["headShot"].ToString());
                PlayerScore.bodyShot = int.Parse(gameDataJson[0]["bodyShot"].ToString());
                PlayerScore.armShot = int.Parse(gameDataJson[0]["armShot"].ToString());
                PlayerScore.legShot = int.Parse(gameDataJson[0]["legShot"].ToString());
                PlayerScore.bestScore = int.Parse(gameDataJson[0]["bestScore"].ToString());

                Debug.Log("�������� ������ �ֽ��ϴ�");

                GameDataUpdate_Kill();
            }
        }
        else
        {
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }
    }

    // ���� ���� �����ϱ�
    public void GameDataUpdate_Kill()
    {
        //if (userData == null)
        //{
        //    Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Insert Ȥ�� Get�� ���� �����͸� �������ּ���.");
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
            Debug.Log("�� ���� �ֽ� ���� ���� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update("UserData_Kill", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2("UserData_Kill", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ������ ������ �����߽��ϴ�. : " + bro);

            BackendRankManager.Instance.RankInsert_BestScore(PlayerScore.bestScore);
        }
        else
        {
            Debug.LogError("���� ���� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }
}
