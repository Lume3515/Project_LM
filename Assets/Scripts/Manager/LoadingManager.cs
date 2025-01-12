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
                // 90%�� �� ���� ��
                if (op.progress < 0.9f)
                {
                    console.text = $"{op.progress.ToString()}%";
                }
                // 90%���� �Ϸ� ���� ��
                else
                {

                    timer += Time.unscaledDeltaTime;
                    progress = Mathf.Lerp(0.9f, 1, timer);

                    console.text = $"{progress:##}%";

                    if (progress >= 1)
                    {
                        nextScene = true;
                        yield break;
                    }
                }
              
            }
            else if(loading == Loading.MultiPlay)
            {
                // 90%�� �� ���� ��
                if (PhotonNetwork.LevelLoadingProgress < 0.9f)
                {
                    console.text = $"{PhotonNetwork.LevelLoadingProgress.ToString()}%";
                }
                // 90%���� �Ϸ� ���� ��
                else
                {

                    timer += Time.unscaledDeltaTime;
                    progress = Mathf.Lerp(0.9f, 1, timer);

                    console.text = $"{progress:##}%";

                    if (progress >= 1)
                    {
                        nextScene = true;
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }

}
