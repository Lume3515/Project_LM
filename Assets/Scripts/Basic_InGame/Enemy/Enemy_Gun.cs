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

    // 플레이어가 나를 때렸는지 확인하는 시간
    private float playerTime;

    // 총알 오브젝트 풀링 객체
    [SerializeField] ObjectPooling bulletPool;

    // 장애물들
    private NavMeshAgent[] obscurationsNV;

    private Transform[] obscurations;

    private void Start()
    {
        // NavMeshAgent컴포넌트가 있는 모든 객체 가져오기
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

    // 체력감소 구현
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

    // 움직임 구현
    private IEnumerator Move()
    {
        yield return null;
    }

    // 이동위치 찾기
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
