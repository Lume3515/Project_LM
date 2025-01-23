using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public enum Loading
{
    InGame,
    MultiPlay
}


public class LoadingManager : MonoBehaviour
{
    // ���൵ �˷��ִ� �ܼ�
    [SerializeField] TextMeshProUGUI console;

    // �������� ������ �޼���
    [SerializeField] TextMeshProUGUI message;

    // �̹���
    [SerializeField] Transform image;

    private static LoadingManager instance;
    public static LoadingManager Instance => instance;

    public static string name_Scene;


    private bool nextScene;

    private AsyncOperation op;

    public static Loading loading;

    private void Awake()
    {
        if (instance == null) instance = this;

    }

    private void Start()
    {
        nextScene = false;
        StartCoroutine(LoadSceneProgress());
    }

    private void Update()
    {
        image.Rotate(0, 0.5f, 0);

        if (nextScene)
        {
            console.text = "�����Ϸ��� [Space]�� Ŭ���ϼ���";

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (loading == Loading.InGame)
                {
                    op.allowSceneActivation = true;
                }
                else if (loading == Loading.MultiPlay)
                {
                    op.allowSceneActivation = true;
                    //op.allowSceneActivation = true;
                }

            }
        }
    }
    //LevelLoadingProgress
    public IEnumerator LoadSceneProgress()
    {
        // �񵿱�(LoadSceneAsync)

        if (loading == Loading.InGame)
        {
            op = SceneManager.LoadSceneAsync(name_Scene);
        }
        else if (loading == Loading.MultiPlay)
        {
            PhotonNetwork.LoadLevel(name_Scene);
            //op.allowSceneActivation = true;


        }

        // allowSceneActivation : ���� �񵿱�� �ҷ����� �� ���� �ε��� ������ �ڵ��� �ҷ��� ������ �̵��� ������? ����
        op.allowSceneActivation = false;

        float timer = 0;
        float progress;

        while (!op.isDone)
        {
            if (loading == Loading.InGame)
            {
                // 70%�� �� ���� ��
                if (op.progress < 0.7f)
                {
                    console.text = $"{op.progress.ToString()}%";

                }
                // 70%���� �Ϸ� ���� ��
                else
                {

                    timer += Random.Range(0.001f, 0.006f);
                    progress = Mathf.Lerp(0.7f, 1f, timer);
                    console.text = $"[{progress * 100:00.00}]%";

                    if (progress >= 1f)
                    {
                        nextScene = true;
                        yield break;
                    }

                }
                yield return new WaitForSeconds(0.035f);

            }
            else if (loading == Loading.MultiPlay)
            {
                // 70%�� �� ���� ��
                if (PhotonNetwork.LevelLoadingProgress < 0.7f)
                {
                    console.text = $"{PhotonNetwork.LevelLoadingProgress.ToString()}%";

                }
                // 70%���� �Ϸ� ���� ��
                else
                {

                    timer += Random.Range(0.001f, 0.006f);
                    progress = Mathf.Lerp(0.7f, 1f, timer);
                    console.text = $"[{progress * 100:00.00}]%";

                    if (progress >= 1)
                    {
                        nextScene = true;
                        yield break;
                    }
                }

                yield return new WaitForSeconds(0.035f);
            }
        }
    }

}
