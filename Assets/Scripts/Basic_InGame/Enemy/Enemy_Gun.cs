using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class Enemy_Gun : MonoBehaviour
{
    private Animator animator;

    private Transform playerTr;

    private NavMeshAgent agent;

    // 무기
    [SerializeField] Transform weapon;
    [SerializeField] Transform firePos;

    // 현재체력
    private int currHP;
    // 죽었는지?
    private bool die;

    // 움직일 위치
    private Vector3 movePos;

    // 플레이어와 가까운지?
    private bool nearPlayer;

    // 총 쏘는 적의 오브젝트 풀링
    private ObjectPooling enemy_Gun_Pool;

    // 장애물들
    private NavMeshObstacle[] obscurationsNMObstacle;
    // 장애물들의 위치
    private Transform[] obscurationsTr;

    // 몇 초 동안 공격을 안 받는 다면 다시 플레이어 향해 이동
    private float damageTimer;

    // 공격 딜레이
    private WaitForSeconds attackDelay;
    private bool attackDelay_Bool;
    
    private Transform fireAim;

    // 지역에 있는지?
    private bool isArea;
    private void Awake()
    {
        obscurationsNMObstacle = new NavMeshObstacle[FindObjectsOfType<NavMeshObstacle>().Length];
        obscurationsTr = new Transform[FindObjectsOfType<NavMeshObstacle>().Length];

        // NavMeshAgent컴포넌트가 있는 모든 객체 가져오기
        obscurationsNMObstacle = FindObjectsOfType<NavMeshObstacle>();

        for (int i = 0; i < obscurationsNMObstacle.Length; i++)
        {
            obscurationsTr[i] = obscurationsNMObstacle[i].transform;
        }

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        playerTr = GameObject.FindWithTag("Player").transform;

        fireAim = GameObject.FindWithTag("EnemyAim").transform;

        attackDelay = new WaitForSeconds(0.3f);

        enemy_Gun_Pool = GetComponentInParent<ObjectPooling>();

        

        //Debug.Log(enemy_Gun_Pool == null);
        //Debug.Log(enemy_Gun_Pool.gameObject.name);
    }

    private void Start()
    {
        PlayerHP.AllStop += Stop;
    }
    public void Setting(Vector3 pos)
    {
        gameObject.SetActive(true);

        die = false;

        currHP = 50;

        agent.Warp(pos); // 순간이동

        StartCoroutine(Move());
    }

    // 체력감소 구현
    public void MinousHP(int damage, DamageType type)
    {
        if (die) return;

        damageTimer = 0; // 총알에 맞았다면 0을 할당해서 그 자리 유지하기

        currHP -= damage;

        if (currHP <= 0)
        {
            die = true;

            switch (type)
            {
                case DamageType.HeadSHot:
                    PlayerScore.currHeadShot++;
                    break;

                case DamageType.BodyShot:
                    PlayerScore.currBodyShot++;
                    break;

                case DamageType.armShot:
                    PlayerScore.currArmShot++;
                    break;

                case DamageType.legShot:
                    PlayerScore.currLegShot++;
                    break;
            }

            StartCoroutine(Die());
            PlayerScore.enemyGun++;
        }
    }

    private GameObject bulletObj;

    // 죽음 구현
    private IEnumerator Die()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1.23f);

        //Debug.Log("2");
        Gamemanager.Instance.CurrNumber.Remove(gameObject);
        enemy_Gun_Pool.Input(gameObject);

    }

    public void Die_Reset()
    {
        Gamemanager.Instance.CurrNumber.Remove(gameObject);
        enemy_Gun_Pool.Input(gameObject);
    }

    //  발사구현
    private IEnumerator Fire()
    {
        if (die) yield break;

        attackDelay_Bool = true;// 중복 발사 방지

        animator.SetBool("attack", true);

        firePos.LookAt(fireAim.position);


        // 공격
        yield return attackDelay;

        attackDelay_Bool = false;
    }

    private bool firstColl; // 한번만 호출
    // 움직임 구현
    private IEnumerator Move()
    {
        if (die) yield break;

        while (true)
        {

            // 플레이어 근처가 아닐 때
            if (!nearPlayer)
            {
                //Debug.Log("이동중");
                agent.destination = playerTr.position;
            }
            else
            {
                if (firstColl) damageTimer = 0; // 플레이어와 닿을 시 true가 되고 true가 되면 0이 할당됨
                firstColl = false;

                //Debug.Log("장애물로 이동중");

                FindPos();
                agent.destination = movePos;

                damageTimer += Time.deltaTime;

                if (damageTimer >= 20) // 20초 동안 아무 공격이 없었다면 플레이어가 근처에 없다고 판단하고 다시 플레이어를 향해 이동함
                {
                    nearPlayer = false;
                }
            }

            isArea = Vector3.Distance(transform.position, agent.destination) < 1.5f; // 보정한 목적지 범위에 위치한지?

            // 멈췄다면
            if (isArea)
            {
                //Debug.Log("멈춤");

                // 위치가 다를 때만
                if (weapon.localPosition.x != 0.211f || weapon.localPosition.y != -0.03f || weapon.localPosition.z != -0.109f)
                {
                    //Debug.Log("위치 다름");
                    weapon.localPosition = new Vector3(0.211f, -0.03f, -0.109f);
                }
                // 방향이 다를 때만
                if (weapon.localRotation.x != -1.414f || weapon.localRotation.y != -48.906f || weapon.localRotation.z != -56.61f)
                {
                    weapon.localRotation = Quaternion.Euler(-1.414f, -48.906f, -56.61f);
                }

                transform.LookAt(playerTr.position);

                if (!attackDelay_Bool) StartCoroutine(Fire());
            }
            else
            {


                // 위치가 다를 때만
                if (weapon.localPosition.x != 0.236f || weapon.localPosition.y != -0.042f || weapon.localPosition.z != -0.033f)
                {
                    //Debug.Log("위치 다름");
                    weapon.localPosition = new Vector3(0.236f, -0.042f, -0.033f);
                }
                // 방향이 다를 때만
                if (weapon.localRotation.x != -3.971f || weapon.localRotation.y != -60.232f || weapon.localRotation.z != -100.141f)
                {
                    weapon.localRotation = Quaternion.Euler(-3.971f, -60.232f, -100.141f);
                }

                animator.SetBool("attack", false);
            }
            yield return null;
        }

    }

    // 이동위치 찾기
    private void FindPos()
    {
        GameObject[] enemyGunObjects = GameObject.FindGameObjectsWithTag("Enemy_Gun");
        NavMeshAgent[] otherAgents = new NavMeshAgent[enemyGunObjects.Length];

        for (int j = 0; j < enemyGunObjects.Length; j++)
        {
            otherAgents[j] = enemyGunObjects[j].GetComponent<NavMeshAgent>();
        }

        for (int i = 0; i < obscurationsTr.Length; i++)
        {
            Vector3 potentialPos = obscurationsTr[i].position + new Vector3(0.1f, 0, 0.94f); // 이동할 장애물(엄폐물)의 위치

            bool isOverlapping = false; // 중복인지?
            foreach (NavMeshAgent nav in otherAgents)
            {
                if (nav != agent && (nav.destination - potentialPos).sqrMagnitude < 8) // agent가 내꺼가 아니고, 방향을구하고 크기가 8보다 작다면(다른 agent 즉 ai가 이미 있다면) 중복처리
                {
                    isOverlapping = true;
                    break;
                }
            }

            if (!isOverlapping && (transform.position - obscurationsTr[i].position).sqrMagnitude <
                (transform.position - movePos).sqrMagnitude) // 내 위치에서 장애물의 위치를 빼고 그 거리가 갱신한? movePos보다 작다면(가깝다면) 새로운 값 할당, 그리고 그 위치가 중복이 아닐 때
            {
                movePos = potentialPos;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (die) return;
            firstColl = true;
            nearPlayer = true;
        }
    }

    private void Stop()
    {
        StopAllCoroutines();
    }
}
