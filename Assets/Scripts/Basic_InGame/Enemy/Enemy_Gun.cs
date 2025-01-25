using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Gun : MonoBehaviour
{
    private Animator animator;

    private Transform playerTr;

    private NavMeshAgent agent;

    // 현재체력
    private int currHP;
    // 죽었는지?
    private bool die;

    // 움직일 위치
    private Vector3 movePos;

    // 플레이어와 가까운지?
    private bool nearPlayer;

    // 총알 오브젝트 풀링 객체
    [SerializeField] ObjectPooling bulletPool;

    // 장애물들
    private NavMeshObstacle[] obscurationsNMObstacle;
    // 장애물들의 위치
    private Transform[] obscurationsTr;

    // 몇 초 동안 공격을 안 받는 다면 다시 플레이어 향해 이동
    private float damageTimer;


    private void Start()
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

        StartCoroutine(Move());
    }

    private void OnEnable()
    {
        die = false;

        currHP = 80;
    }

    // 체력감소 구현
    private void MinousHP(int damage, DamageType type)
    {
        if (die) return;
        damageTimer = 0;
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
            PlayerScore.enemyGun++;
        }
    }

    // 죽음 구현
    private IEnumerator Die()
    {
        yield return null;
    }

    //  발사구현
    private IEnumerator Fire()
    {
        
        yield return null;
    }

    private bool firstColl;

    // 움직임 구현
    private IEnumerator Move()
    {
        while (!Gamemanager.Instance.GameOver)
        {
            // 플레이어 근처가 아닐 때
            if (!nearPlayer)
            {
                //Debug.Log("이동중");
                agent.destination = playerTr.position;
            }
            else
            {
                if (firstColl) damageTimer = 0;
                firstColl = false;

                //Debug.Log("장애물로 이동중");

                FindPos();
                agent.destination = movePos;

                damageTimer += Time.deltaTime;

                if (damageTimer >= 20)
                {
                    nearPlayer = false;
                }
            }

            // 멈췄다면
            if (agent.velocity.magnitude == 0)
            {
                Debug.Log("멈춤");
                transform.LookAt(playerTr.position);

                StartCoroutine(Fire());
            }

            yield return null;
        }
    }

    // 이동위치 찾기
    private void FindPos()
    {
        for (int i = 0; i < obscurationsTr.Length; i++)
        {
            if (Vector3.Distance(transform.position, obscurationsTr[i].position) < Vector3.Distance(transform.position, movePos))
            {
                movePos = obscurationsTr[i].position + new Vector3(0.1f, 0, 0.94f);
                //Debug.Log(movePos);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            firstColl = true;
            nearPlayer = true;
        }
    }
}
