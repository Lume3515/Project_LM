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

// �߻� Ÿ��
public enum ShootingType
{
    Shoulder, // ����  
    Run, // �ٱ�
    Walk, // �ȱ�
    Sit, // �ɱ�
    Stand, // ���ֱ� 
    SitWalk,    // �ɾƼ� �ȱ�
    Null
}

public class Gamemanager : MonoBehaviour
{
    // �̱��� ����
    private static Gamemanager instance;
    public static Gamemanager Instance => instance;

    private SpawnManager spawnManager;

    // �� �� ��� �ִ���?
    private List<GameObject> currNumber = new List<GameObject>();
    public List<GameObject> CurrNumber { get { return currNumber; } set { currNumber = value; } } // ������Ƽ

    // Ÿ��
    private ShootingType shootingType;
    public ShootingType ShootingType { get { return shootingType; } set { shootingType = value; } }

    // ���ð�
    private float waitTimer;

    // ���� �ߴ���?
    private bool isSpawn;

    // �ܼ� 
    [SerializeField] TextMeshProUGUI console;

    // �� ���� ���� �ϰ� �ϴ� ����
    private bool firstColl;

    // ���� ��������
    private int currstage;

    // ���ӿ��� ����?
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
        // ���� �� �׾��ٸ�
        if (currNumber.Count <= 0 && !firstColl)
        {
            currstage++;
            firstColl = true;

            isSpawn = false;

            // Ȱ��ȭ > ���̰�
            console.enabled = true;

            StartCoroutine(WaitTime());
        }

    }

    // ���ð�
    public IEnumerator WaitTime()
    {
        waitTimer = 15;

        // while�� ������ true�� �� �۵���;; ��Ծ����̤�
        while (waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;

            // �κ� ���� ���� �Է��ְ� 1�� �ڸ��� �������� ���� 
            console.text = $"<color=#ffffff>���� �������� ���� {waitTimer:00} ��</color>\n<color=#87CEEB>'K'Ű�� ���� ��ŵ �� �� �ֽ��ϴ�.</color>\n<color=#ffffff>���� �������� : {currstage}</color>";

            // ��� �ð� ��ŵ
            if (Input.GetKeyDown(KeyCode.K) && !isSpawn)
            {
                isSpawn = true;
                StartCoroutine(spawnManager.zombieSpawn(currstage));

                firstColl = false;

                // �� Ȱ��ȭ > �� ���̰�
                console.enabled = false;

                // ��ŵ���� ���� �ι� ���� ����                
                yield break;

            }

            yield return null;
        }

        isSpawn = true;
        StartCoroutine(spawnManager.zombieSpawn(currstage));

        // �� Ȱ��ȭ > �� ���̰�
        console.enabled = false;

        firstColl = false;
    }

    public void GamoOver()
    {
        gameOver = true;
        StopAllCoroutines();
    }

}
