using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // 진행도 알려주는 콘솔
    [SerializeField] TextMeshProUGUI console;

    // 유저에게 보내는 메세지
    [SerializeField] TextMeshProUGUI message;

    // 이미지
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
            console.text = "시작하려면 [Space]를 클릭하세요";

            if (Input.GetKeyDown(KeyCode.Space))
            {
                op.allowSceneActivation = true;
            }
        }
    }

    public IEnumerator LoadSceneProgress()
    {
        // 비동기(LoadSceneAsync)
        op = SceneManager.LoadSceneAsync(name);

        // allowSceneActivation : 씬을 비동기로 불러들일 떄 씬의 로딩이 끝나면 자동을 불러온 씬으로 이동할 것인지? ㄴㄴ
        op.allowSceneActivation = false;

        float timer = 0;
        float progress;

        while (!op.isDone)
        {

            // 90%도 안 됐을 떄
            if (op.progress < 0.9f)
            {
                console.text = $"{op.progress.ToString()}%";
            }
            // 90%까지 완료 됐을 때
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
