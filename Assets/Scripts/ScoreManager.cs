using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Text;

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

    // ������ ����
    public void GameDataInsert()
    {
        //Debug.Log(PlayerScore.headShot);

        //Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");

        Param param = new Param();

        // Ŭ ���� �� ����
        if (PlayerScore.currHeadShot > PlayerScore.headShot) param.Add("headShot", PlayerScore.headShot);
        if (PlayerScore.currBodyShot > PlayerScore.bodyShot) param.Add("bodyShot", PlayerScore.bodyShot);
        if (PlayerScore.currArmShot > PlayerScore.armShot) param.Add("armShot", PlayerScore.armShot);
        if (PlayerScore.currLegShot > PlayerScore.legShot) param.Add("legShot", PlayerScore.legShot);
        if (PlayerScore.currBestScore > PlayerScore.bestScore) param.Add("bestScore", PlayerScore.bestScore);

        //Debug.Log("���� ���� ������ ������ ��û�մϴ�.");
        var bro = Backend.GameData.Insert("UserData_Kill", param);

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���� ������ �������Դϴ�.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }
}
