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

    // ����ü��
    private int currHP;
    // �׾�����?
    private bool die;

    // ������ ��ġ
    private Vector3 movePos;

    // �÷��̾�� �������?
    private bool nearPlayer;

    // �Ѿ� ������Ʈ Ǯ�� ��ü
    [SerializeField] ObjectPooling bulletPool;

    // ��ֹ���
    private NavMeshObstacle[] obscurationsNMObstacle;
    // ��ֹ����� ��ġ
    private Transform[] obscurationsTr;

    // �� �� ���� ������ �� �޴� �ٸ� �ٽ� �÷��̾� ���� �̵�
    private float damageTimer;


    private void Start()
    {
        obscurationsNMObstacle = new NavMeshObstacle[FindObjectsOfType<NavMeshObstacle>().Length];
        obscurationsTr = new Transform[FindObjectsOfType<NavMeshObstacle>().Length];

        // NavMeshAgent������Ʈ�� �ִ� ��� ��ü ��������
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

    // ü�°��� ����
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

    // ���� ����
    private IEnumerator Die()
    {
        yield return null;
    }

    //  �߻籸��
    private IEnumerator Fire()
    {
        
        yield return null;
    }

    private bool firstColl;

    // ������ ����
    private IEnumerator Move()
    {
        while (!Gamemanager.Instance.GameOver)
        {
            // �÷��̾� ��ó�� �ƴ� ��
            if (!nearPlayer)
            {
                //Debug.Log("�̵���");
                agent.destination = playerTr.position;
            }
            else
            {
                if (firstColl) damageTimer = 0;
                firstColl = false;

                //Debug.Log("��ֹ��� �̵���");

                FindPos();
                agent.destination = movePos;

                damageTimer += Time.deltaTime;

                if (damageTimer >= 20)
                {
                    nearPlayer = false;
                }
            }

            // ����ٸ�
            if (agent.velocity.magnitude == 0)
            {
                Debug.Log("����");
                transform.LookAt(playerTr.position);

                StartCoroutine(Fire());
            }

            yield return null;
        }
    }

    // �̵���ġ ã��
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
