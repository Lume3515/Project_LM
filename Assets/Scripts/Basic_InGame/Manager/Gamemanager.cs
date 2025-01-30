using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageType
{
    HeadSHot,
    BodyShot,
    armShot,
    legShot,
}

// 발사 타입
public enum ShootingType
{
    Shoulder, // 견착  
    Run, // 뛰기
    Walk, // 걷기
    Sit, // 앉기
    Stand, // 서있기 
    SitWalk,    // 앉아서 걷기
    Null
}

public class Gamemanager : MonoBehaviour
{
    // 싱글톤 패턴
    private static Gamemanager instance;
    public static Gamemanager Instance => instance;

    private SpawnManager spawnManager;

    // 몇 명 살아 있는지?
    private List<GameObject> currNumber = new List<GameObject>();
    public List<GameObject> CurrNumber { get { return currNumber; } set { currNumber = value; } } // 프로퍼티

    // 타입
    private ShootingType shootingType;
    public ShootingType ShootingType { get { return shootingType; } set { shootingType = value; } }

    // 대기시간
    private float waitTimer;

    // 스폰 했는지?
    private bool isSpawn;

    // 콘솔 
    [SerializeField] TextMeshProUGUI console;

    // 한 번만 실행 하게 하는 변수
    private bool firstColl;

    // 현재 스테이지
    private int currstage;

    // 게임오버 인지?
    private bool gameOver;
    public bool GameOver { get { return gameOver; } set { gameOver = value; } }



    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

    }

    private void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        spawnManager = GetComponent<SpawnManager>();
    }

    private void Update()
    {
        //Debug.Log(currNumber.Count);
        // 좀비가 다 죽었다면
        if (currNumber.Count <= 0 && !firstColl)
        {
            currstage++;
            firstColl = true;

            isSpawn = false;

            // 활성화 > 보이게
            console.enabled = true;

            StartCoroutine(WaitTime());
        }

    }

    // 대기시간
    public IEnumerator WaitTime()
    {
        waitTimer = 15;

        // while은 조건이 true일 떄 작동함;; 까먹었음ㅜㅜ
        while (waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;

            // 부분 마다 색깔 입려주고 1의 자리만 나오도록 설정 
            console.text = $"<color=#ffffff>다음 스테이지 까지 {waitTimer:00} 초</color>\n<color=#87CEEB>'K'키를 눌러 스킵 할 수 있습니다.</color>\n<color=#ffffff>다음 스테이지 : {currstage}</color>";

            // 대기 시간 스킵
            if (Input.GetKeyDown(KeyCode.K) && !isSpawn)
            {
                isSpawn = true;
                StartCoroutine(spawnManager.zombieSpawn(currstage));

                firstColl = false;

                // 비 활성화 > 안 보이게
                console.enabled = false;

                // 스킵으로 인한 두번 생성 방지                
                yield break;

            }

            yield return null;
        }

        isSpawn = true;
        StartCoroutine(spawnManager.zombieSpawn(currstage));

        // 비 활성화 > 안 보이게
        console.enabled = false;

        firstColl = false;
    }

    public void GamoOver()
    {
        gameOver = true;
        StopAllCoroutines();
    }

}
