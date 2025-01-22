using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Gun : MonoBehaviour
{
    private Animator animator;

    private Transform playerTr;

    private int currHP;

    private bool die;

    // �÷��̾ ���� ���ȴ��� Ȯ���ϴ� �ð�
    private float playerTime;

    // �Ѿ� ������Ʈ Ǯ�� ��ü
    [SerializeField] ObjectPooling bulletPool;

    // ��ֹ���
    private NavMeshAgent[] obscurationsNV;

    private Transform[] obscurations;

    private void Start()
    {
        // NavMeshAgent������Ʈ�� �ִ� ��� ��ü ��������
        obscurationsNV = FindObjectsOfType<NavMeshAgent>();

        for (int i = 0; i < obscurationsNV.Length; i++)
        {
            obscurations[i] = obscurationsNV[i].transform;
        }

        animator = GetComponent<Animator>();

        playerTr = GameObject.FindWithTag("Player").transform;
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

    // ������ ����
    private IEnumerator Move()
    {
        yield return null;
    }

    // �̵���ġ ã��
    private IEnumerator FindPos()
    {
        yield return null;

        for(int i = 0; i < obscurations.Length; i++)
        {
            if(Vector3.Distance(transform.position, obscurations[i].position) > )
            {

            }
        }

    }
}
