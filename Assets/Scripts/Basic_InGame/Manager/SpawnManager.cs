using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieType
{
    basic,
    speed,
    tanker,
    shild,
}

public class SpawnManager : MonoBehaviour
{
    // 생성 위치
    [SerializeField] Transform[] spawnPos;

    [SerializeField] ObjectPooling objectPooling_Zombie_Basic;

    [SerializeField] ObjectPooling objectPooling_Zombie_Tanker;

    [SerializeField] ObjectPooling objectPooling_Zombie_Speed;

    private ZombieType zombieType;

    private WaitForSeconds spawnDelay;

    private int stage;

    private int randomZombe;

    // 좀비 소환 가능?
    private bool basic;
    private bool speed;
    private bool tanker;
    private bool shild;

    private void Start()
    {
        spawnDelay = new WaitForSeconds(0.5f);
    }

    private void Update()
    {

    }

    public IEnumerator zombieSpawn(int stage)
    {
        this.stage = 0;

        #region// 난이도
        switch (stage)
        {
            case 1:
                basic = true;               
                break;

            case 3:
                tanker = true;
                break;

            case 5:
                speed = true;
                break;

            case 7:
                shild = true;
                break;

            case 10:
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

                    zombieType = ZombieType.basic;

                    break;

                case 1:

                    if (!speed) continue;

                    zombieType = ZombieType.speed;

                    break;

                case 2:

                    if (!tanker) continue;

                    zombieType = ZombieType.tanker;

                    break;

                case 3:

                    if (!shild) continue;

                    zombieType = ZombieType.shild;

                    break;
            }


            Spawn(zombieType);

            this.stage++;

            yield return spawnDelay;
        }
    }

    GameObject spawn_Zombie;

    private void Spawn(ZombieType type)
    {
        switch (type)
        {
            case ZombieType.basic:

                spawn_Zombie = objectPooling_Zombie_Basic.OutPut();

                spawn_Zombie.GetComponent<Zombie_BasicAndTankerAndSpeed>().Setting(spawnPos[Random.Range(0, 6)], 0.5f, 100, 2.13f, 5);

                Gamemanager.Instance.CurrNumber.Add(spawn_Zombie);

                break;

            case ZombieType.speed:

                spawn_Zombie = objectPooling_Zombie_Speed.OutPut();

                spawn_Zombie.GetComponent<Zombie_BasicAndTankerAndSpeed>().Setting(spawnPos[Random.Range(0, 6)], 6f, 50, 1.4f, 7);

                Gamemanager.Instance.CurrNumber.Add(spawn_Zombie);

                break;

            case ZombieType.tanker:

                spawn_Zombie = objectPooling_Zombie_Tanker.OutPut();
                spawn_Zombie.GetComponent<Zombie_BasicAndTankerAndSpeed>(). Setting(spawnPos[Random.Range(0, 6)], 0.2f, 400, 3f, 0);

                Gamemanager.Instance.CurrNumber.Add(spawn_Zombie);

                break;

            case ZombieType.shild:

                spawn_Zombie = objectPooling_Zombie_Basic.OutPut();

                spawn_Zombie.GetComponent<Zombie_BasicAndTankerAndSpeed>().Setting(spawnPos[Random.Range(0, 6)], 0.5f, 100, 2.13f, 5);

                Gamemanager.Instance.CurrNumber.Add(spawn_Zombie);

                break;
        }
    }


}
