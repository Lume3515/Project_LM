using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    basic, // 일반좀비
    speed, // 스피드 좀비
    tanker, // 탱커 좀비
    gun, // 총 쏘는 적
}

public class SpawnManager : MonoBehaviour
{
    // 생성 위치
    [SerializeField] Transform[] spawnPos;

    [SerializeField] ObjectPooling objectPooling_Zombie_Basic;

    [SerializeField] ObjectPooling objectPooling_Zombie_Tanker;

    [SerializeField] ObjectPooling objectPooling_Zombie_Speed;

    [SerializeField] ObjectPooling objectPooling_Enemy_Gun;

    [SerializeField] Transform[] etcSpawnPos;

    private SpawnType spawnType;

    private WaitForSeconds spawnDelay;

    private int stage;

    private int randomZombe;

    // 소환 가능?
    private bool basic;
    private bool speed;
    private bool tanker;
    private bool gun;

    private void Start()
    {
        spawnDelay = new WaitForSeconds(0.5f);
        PlayerHP.AllStop += Stop;  
    }   

    public IEnumerator zombieSpawn(int stage)
    {
        this.stage = 0;

        #region// 난이도
        switch (stage)
        {
            case 1:
                gun = true;
                break;

            case 2:
                speed = true;
                break;

            case 3:
                tanker = true;
                break;

            case 5:
                basic = true;
                break;

            case 7:
                basic = false;
                break;

        }
        #endregion

        stage += 3;

        while (this.stage < stage)
        {
            randomZombe = Random.Range(0, 4);

            switch (randomZombe)
            {
                case 0:

                    if (!basic) continue;

                    spawnType = SpawnType.basic;

                    break;

                case 1:

                    if (!speed) continue;

                    spawnType = SpawnType.speed;

                    break;

                case 2:

                    if (!tanker) continue;

                    spawnType = SpawnType.tanker;

                    break;

                case 3:

                    if (!gun) continue;

                    spawnType = SpawnType.gun;

                    break;
            }


            Spawn(spawnType);

            this.stage++;

            yield return spawnDelay;
        }
    }

    GameObject spawn;

    private void Spawn(SpawnType type)
    {
        switch (type)
        {
            case SpawnType.basic:

                spawn = objectPooling_Zombie_Basic.OutPut();

                spawn.GetComponent<Zombie>().Setting(spawnPos[Random.Range(0, 6)], 0.5f, 150, 2.13f, 2);

                Gamemanager.Instance.CurrNumber.Add(spawn);

                break;

            case SpawnType.speed:

                spawn = objectPooling_Zombie_Speed.OutPut();

                spawn.GetComponent<Zombie>().Setting(spawnPos[Random.Range(0, 6)], 6f, 100, 1.4f, 5);

                Gamemanager.Instance.CurrNumber.Add(spawn);

                break;

            case SpawnType.tanker:

                spawn = objectPooling_Zombie_Tanker.OutPut();
                spawn.GetComponent<Zombie>().Setting(spawnPos[Random.Range(0, 6)], 0.2f, 250, 3f, 0);

                Gamemanager.Instance.CurrNumber.Add(spawn);

                break;

            case SpawnType.gun:

                spawn = objectPooling_Enemy_Gun.OutPut();

                spawn.GetComponent<Enemy_Gun>().Setting(etcSpawnPos[Random.Range(0, 2)].position);

                Gamemanager.Instance.CurrNumber.Add(spawn);

                break;
        }
    }

    private void Stop()
    {
        StopAllCoroutines();
    }
}
