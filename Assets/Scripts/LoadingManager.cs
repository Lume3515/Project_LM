using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // ���൵ �˷��ִ� �ܼ�
    [SerializeField] TextMeshProUGUI console;

    // �������� ������ �޼���
    [SerializeField] TextMeshProUGUI message;

    // �̹���
    [SerializeField] RawImage image;

    private static LoadingManager instance;
    public static LoadingManager Instance => instance;

    public static string name;

    private bool nextScene;

    private AsyncOperation op;


    private void Start()
    {
        nextScene = false;
        StartCoroutine(LoadSceneProgress());
    }

    private void Update()
    {
        if (nextScene)
        {
            console.text = "�����Ϸ��� [Space]�� Ŭ���ϼ���";

            if (Input.GetKeyDown(KeyCode.Space))
            {
                op.allowSceneActivation = true;
            }
        }
    }

    public IEnumerator LoadSceneProgress()
    {
        // �񵿱�(LoadSceneAsync)
        op = SceneManager.LoadSceneAsync(name);

        // allowSceneActivation : ���� �񵿱�� �ҷ����� �� ���� �ε��� ������ �ڵ��� �ҷ��� ������ �̵��� ������? ����
        op.allowSceneActivation = false;

        float timer = 0;
        float progress;

        while (!op.isDone)
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

            yield return null;
        }

    }

}
