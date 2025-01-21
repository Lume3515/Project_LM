using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Text;
using TMPro;

public class BackendRankManager : MonoBehaviour
{
    private static BackendRankManager _instance = null;

    public static BackendRankManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendRankManager();
            }

            return _instance;
        }
    }

    public void RankInsert_BestScore(int score)
    {      
        string rankUUID = "01948280-2e63-734b-be70-7d2a827362cb";

        string tableName = "UserData_Kill";
        string rowInDate = string.Empty;

       
        Debug.Log("������ ��ȸ�� �õ��մϴ�.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("������ ��ȸ �� ������ �߻��߽��ϴ� : " + bro);
            return;
        }

        Debug.Log("������ ��ȸ�� �����߽��ϴ� : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�. ������ ������ �õ��մϴ�.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("������ ���� �� ������ �߻��߽��ϴ� : " + bro2);
                return;
            }

            Debug.Log("������ ���Կ� �����߽��ϴ� : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("�� ���� ������ rowInDate : " + rowInDate); // ����� rowIndate�� ���� ������ �����ϴ�.  

        Param param = new Param();
        param.Add("bestScore", score);

        // ����� rowIndate�� ���� �����Ϳ� param������ ������ �����ϰ� ��ŷ�� �����͸� ������Ʈ�մϴ�.  
        Debug.Log("��ŷ ������ �õ��մϴ�.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��� �� ������ �߻��߽��ϴ�. : " + rankBro);
            return;
        }

        Debug.Log("��ŷ ���Կ� �����߽��ϴ�. : " + rankBro);
    }

    public void RankGet_BestScore(GameObject rankTMP, Transform content)
    {
        string rankUUID = "01948280-2e63-734b-be70-7d2a827362cb"; 
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��ȸ�� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��ŷ ��ȸ�� �����߽��ϴ�. : " + bro);

        Debug.Log("�� ��ŷ ��� ���� �� : " + bro.GetFlattenJSON()["totalCount"].ToString());

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();        

           GameObject spawn =  GameObject.Instantiate(rankTMP, content);
            spawn.GetComponentInChildren<TextMeshProUGUI>().text = "   ���� : " + jsonData["rank"].ToString() + "�� /   �г��� : " + jsonData["nickname"].ToString() + "�� /   �ְ����� : " + jsonData["score"].ToString() + "��";


            //info.AppendLine("���� : " + jsonData["rank"].ToString());
            //info.AppendLine("�г��� : " + jsonData["nickname"].ToString());
            //info.AppendLine("���� : " + jsonData["score"].ToString());
            //info.AppendLine("gamerInDate : " + jsonData["gamerInDate"].ToString());
            //info.AppendLine("���Ĺ�ȣ : " + jsonData["index"].ToString());
            //info.AppendLine();
            //Debug.Log(info);
        }
    }
}
