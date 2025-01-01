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

    // 최대 체력
    private int maxHP;

    // 스폰 위치
    private Transform[] spawnPos;

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
    [SerializeField] ObjectPooling objectPooling;      

    private void Start()
    {     
        animator = GetComponent<Animator>();

        attackDelay = new WaitForSeconds(2.13f);

        maxHP = 100;

        spawnMaxTime = 4.05f;

        agent = GetComponent<NavMeshAgent>();

        playerTr = GameObject.FindWithTag("Player").transform;

        moveSpeed = 2f;

    }

    private void OnEnable()
    {
        //transform.position = spawnPos[Random.Range(0, 2)].position;

        spawnTime = 0;
        currHP = maxHP;

        animator.SetBool("walk", true);
    }

    public void Setting(float speed)
    {
        moveSpeed = speed;
    }

    // 체력 감소
    public void MinusHP(int damage)
    {
        currHP -= damage;

        if (currHP <= 0)
        {
            objectPooling.Input(gameObject);
            animator.SetTrigger("die");
            StopCoroutine(Attack());
        }
    }


    private void Update()
    {
        if (spawnTime <= spawnMaxTime) spawnTime += Time.deltaTime;

        //Debug.Log(spawnTime);

        if (spawnTime >= spawnMaxTime && !isAttack)
        {
            agent.isStopped = false;
            MoveMent();
        }
    }

    // 움직임
    private void MoveMent()
    {
        agent.destination = playerTr.position;
        agent.speed = moveSpeed;
    }

    // 현재 상태
    private IEnumerator Attack()
    {
        // 이동 멈추기
        agent.isStopped = true;

        animator.SetTrigger("hit");
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
