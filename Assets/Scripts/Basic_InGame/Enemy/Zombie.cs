using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    // 움직임 속도
    private float moveSpeed;

    // 네비게이션
    private NavMeshAgent agent;

    // 플레이어 Tr
    private Transform playerTr;

    // 현재 체력
    private int currHP;

    // 애니메이터
    private Animator animator;

    // 생성시간
    private float spawnTime;
    private float spawnMaxTime;

    // 공격 가능?
    private bool isAttack;

    // 공격 애니메이션 딜레이
    private WaitForSeconds attackDelay;

    // 오브젝트 풀링
    private ObjectPooling objectPooling;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        attackDelay = new WaitForSeconds(2.13f);

        spawnMaxTime = 4.05f;

        agent = GetComponent<NavMeshAgent>();

        playerTr = GameObject.FindWithTag("Player").transform;

        objectPooling = GetComponentInParent<ObjectPooling>();

        dieDelay = new WaitForSeconds(2.5f);
    }

    private void OnEnable()
    {
        //transform.position = spawnPos[Random.Range(0, 2)].position;

        spawnTime = 0;
    }

    public void Setting(Transform pos, float speed, int hp)
    {
        gameObject.SetActive(true);

        agent.Warp(pos.position);
        moveSpeed = speed;
        currHP = hp;
        //Debug.Log(agent.isOnNavMesh);
    }

    // 체력 감소 > 총알 스크립트에서 할당
    public void MinusHP(int damage, DamageType type)
    {
        currHP -= damage;
        Debug.Log(currHP);
        if (currHP <= 0)
        {
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

            agent.isStopped = true;

            StopCoroutine(Attack());
            StartCoroutine(Die());
        }
    }

    // 죽는 시간 딜레이
    private WaitForSeconds dieDelay;

    private IEnumerator Die()
    {
        animator.SetTrigger("die");

        yield return dieDelay;

        Gamemanager.Instance.CurrNumber.Remove(gameObject);
        objectPooling.Input(gameObject);
    }


    private void Update()
    {
        if (spawnTime <= spawnMaxTime) spawnTime += Time.deltaTime;

        //Debug.Log(spawnTime);

        if (spawnTime >= spawnMaxTime && !isAttack)
        {

            agent.isStopped = false;
            animator.SetBool("walk", true);
            MoveMent();
        }

    }

    // 움직임
    private void MoveMent()
    {
        if (agent.isPathStale)
        {
            agent.ResetPath();
            agent.SetDestination(playerTr.position);
        }
        agent.destination = playerTr.position;
        agent.speed = moveSpeed;
    }

    // 현재 상태
    private IEnumerator Attack()
    {
        // 이동 멈추기
        agent.isStopped = true;

        animator.SetTrigger("attack");
        animator.SetBool("walk", false);

        yield return attackDelay;

        animator.SetBool("walk", true);
        isAttack = false;

        yield break;
    }

    private void OnTriggerStay(Collider other)
    {
        // 걸을 때만 가능
        if (other.CompareTag("Player") && !isAttack)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }
}
