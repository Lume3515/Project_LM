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

    // ���� ��ġ
    private Transform[] spawnPos;

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

        currHP = 100;

        objectPooling = GetComponentInParent<ObjectPooling>();
    }

    private void OnEnable()
    {
        //transform.position = spawnPos[Random.Range(0, 2)].position;

        spawnTime = 0;

        Setting(1.4f, 100);
    }

    public void Setting(float speed, int hp)
    {
        moveSpeed = speed;
        currHP = hp;
    }

    // ü�� ���� > �Ѿ� ��ũ��Ʈ���� �Ҵ�
    public void MinusHP(int damage, DamageType type)
    {
        currHP -= damage;

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
            animator.SetBool("walk", true);
            MoveMent();
        }

    }

    // ������
    private void MoveMent()
    {
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
