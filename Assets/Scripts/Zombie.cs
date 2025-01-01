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

    // �ִ� ü��
    private int maxHP;

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

    // ü�� ����
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

        animator.SetTrigger("hit");
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
