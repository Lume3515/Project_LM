using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_BasicAndTankerAndSpeed : MonoBehaviour
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
    public bool IsAttack => isAttack;

    // 공격 애니메이션 딜레이
    private WaitForSeconds attackDelay;

    // 오브젝트 풀링
    private ObjectPooling objectPooling;

    // 데미지
    private int damage;

    //사망
    private bool die;

    private PlayerHP playerHp;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        spawnMaxTime = 4.05f;

        agent = GetComponent<NavMeshAgent>();

        playerTr = GameObject.FindWithTag("Player").transform;

        objectPooling = GetComponentInParent<ObjectPooling>();

        dieDelay = new WaitForSeconds(2.5f);

        minousHPDelay = new WaitForSeconds(1.3f);
    }

    private void OnEnable()
    {
        //transform.position = spawnPos[Random.Range(0, 2)].position;

        spawnTime = 0;
    }

    public void Setting(Transform pos, float speed, int hp, float attackDelayFloat, int attackDamage)
    {
        gameObject.SetActive(true);

        attackDelay = new WaitForSeconds(attackDelayFloat);

        damage = attackDamage;
        die = false;

        agent.Warp(pos.position);

        moveSpeed = speed;
        currHP = hp;
        //Debug.Log(agent.isOnNavMesh);

        isAttack = false;
    }

    // 체력 감소 > 총알 스크립트에서 할당
    public void MinusHP(int damage, DamageType type)
    {
        if (die) return;

        currHP -= damage;
        //Debug.Log(currHP);
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

            if (this.damage == 0) PlayerScore.tankerZombie++;
            else if (this.damage == 5) PlayerScore.basicZombie++;
            else if (this.damage == 10) PlayerScore.speedZombie++;

            // 이동 멈추기
            die = true;
            agent.isStopped = true;
            animator.SetTrigger("die");
            StopCoroutine(Attack());
            StartCoroutine(Die());
        }
    }

    // 죽는 시간 딜레이
    private WaitForSeconds dieDelay;

    private IEnumerator Die()
    {

        yield return dieDelay;

        Gamemanager.Instance.CurrNumber.Remove(gameObject);
        objectPooling.Input(gameObject);
    }


    private void Update()
    {
        if (Gamemanager.Instance.GameOver) return;

        if (spawnTime <= spawnMaxTime) spawnTime += Time.deltaTime;

        //Debug.Log(spawnTime);

        if (spawnTime >= spawnMaxTime && !isAttack && !die)
        {

            agent.isStopped = false;
            animator.SetBool("walk", true);
            MoveMent();
        }

    }

    // 움직임
    private void MoveMent()
    {
        agent.updateRotation = true;
        agent.destination = playerTr.position;
        agent.speed = moveSpeed;
    }

    // 공격하는 함수
    private IEnumerator Attack()
    {
        isAttack = true; // 공격 중 상태 설정
        stopAttack = false;
        animator.SetTrigger("attack");
        animator.SetBool("walk", false);

        RaycastHit hit;
        // 플레이어가 앞에 없을 때는 다시 찾아 쳐다보기 / 레이 위치 정해주고 앞으로 쏘고 플레이어 레이어만
        if (!Physics.Raycast((transform.position + new Vector3(0, 1, 0)), transform.forward, out hit, 30, 1 << 3))
        {
            transform.LookAt(playerTr.transform);
        }

        StartCoroutine(MinousHP_Player());

        yield return attackDelay; // 공격 딜레이 동안 대기

        animator.SetBool("walk", true);
        isAttack = false; // 공격 종료 상태 설정
    }

    private WaitForSeconds minousHPDelay;

    private PlayerState playerState;

    // 플레이어 체력 감소
    private IEnumerator MinousHP_Player()
    {
        if (Gamemanager.Instance.GameOver) yield break;

        yield return minousHPDelay;

        if (!stopAttack)
        {
            if (damage == 0 && !playerState.HorrorEffect_bool && !die)
            {
                //Debug.Log("d");
                StartCoroutine(playerState.HorrorEffect());

                yield break;
            }

            StartCoroutine(playerHp.MinousHP(damage));
        }

        yield break;
    }


    private bool stopAttack;

    private void OnTriggerStay(Collider other)
    {
        if (Gamemanager.Instance.GameOver) return;

        // 걸을 때만 공격 가능
        if (other.CompareTag("Player") && !isAttack)
        {
            if (playerHp == null) playerHp = other.GetComponent<PlayerHP>();
            if (playerState == null) playerState = other.GetComponent<PlayerState>();

            if (!isAttack) // 코루틴 중복 실행 방지
            {
                StartCoroutine(Attack()); // 코루틴 실행
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stopAttack = true;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay((transform.position + new Vector3(0, 1, 0)), transform.forward);
    //}
}
