using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    // ������ �ӵ�
    private float moveSpeed;

    // �׺���̼�
    private NavMeshAgent agent;

    // �÷��̾� Tr
    private Transform playerTr;

    // ���� ü��
    private int currHP;

    // �ִϸ�����
    private Animator animator;

    // �����ð�
    private float spawnTime;
    private float spawnMaxTime;

    // ���� ����?
    private bool isAttack;

    // ���� �ִϸ��̼� ������
    private WaitForSeconds attackDelay;

    // ������Ʈ Ǯ��
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

    // ü�� ���� > �Ѿ� ��ũ��Ʈ���� �Ҵ�
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

    // �״� �ð� ������
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

    // ������
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

    // ���� ����
    private IEnumerator Attack()
    {
        // �̵� ���߱�
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
        // ���� ���� ����
        if (other.CompareTag("Player") && !isAttack)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }
}
