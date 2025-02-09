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

    // �̹���
    [SerializeField] Transform rotImage;


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
        rotImage.Rotate(0, 250 * Time.deltaTime, 0);

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

        console.text = "���� �����͸� �ʱ�ȭ ���Դϴ١�����";

        yield return new WaitForSeconds(2);

        console.text = "���ӿ� �ʿ��� ���ҽ��� ������ �����ϴ� ���Դϴ�...";

        yield return new WaitForSeconds(1);

        while (!op.isDone)
        {
            if (loading == Loading.InGame)
            {
                // 90%�� �� ���� ��
                if (op.progress < 0.9f)
                {
                    //console.text = $"{op.progress.ToString()}%";

                }
                // 90%���� �Ϸ� ���� ��
                else
                {
                    timer += Random.Range(0.001f, 0.002f);
                    progress = Mathf.Lerp(0f, 1f, timer);
                    console.text = $"���� �����͸� �ʱ�ȭ ���Դϴ١�����[Data initialization...{progress * 100:00.00}%]";

                    if (progress >= 1f)
                    {
                        nextScene = true;
                        yield break;
                    }

                }
                yield return new WaitForSeconds(0.035f * Time.deltaTime);

            }
            else if (loading == Loading.MultiPlay)
            {
                // 90%�� �� ���� ��
                if (PhotonNetwork.LevelLoadingProgress < 0.9f)
                {
                    //console.text = $"{PhotonNetwork.LevelLoadingProgress.ToString()}%";

                }
                // 90%���� �Ϸ� ���� ��
                else
                {
                    timer += Random.Range(0.001f, 0.002f);
                    progress = Mathf.Lerp(0f, 1f, timer);
                    console.text = $"���� �����͸� �ʱ�ȭ ���Դϴ١�����[Data initialization...{progress * 100:00.00}%]";

                    if (progress >= 1)
                    {
                        nextScene = true;
                        yield break;
                    }
                }

                yield return new WaitForSeconds(0.035f * Time.deltaTime);
            }
        }
    }

}
