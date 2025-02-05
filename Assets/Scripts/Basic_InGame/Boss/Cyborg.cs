using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Cyborg : MonoBehaviour
{
    // 네비게이션
    private NavMeshAgent agent;

    // 보스체력 게이지
    private Slider bossHP_Bar;

    // 보스체력 숫자 표시
    private Slider bossHP_Number;

    // 보스 UI
    private GameObject bossUI;

    // 보스 애니메이터
    private Animator animator;

    private bool isDie;

    private int hp;

    private void Start()
    {
        bossUI.SetActive(true);

        agent.speed = 3;

        hp = 5000;
    }

    // 체력 감소
    private void MinousHP(int damage)
    {
        hp -= damage;

        // 죽음
        if (hp <= 0)
        {
            agent.isStopped = true;
            animator.SetTrigger("isDie");

            Destroy(gameObject, 2);
        }
    }



    // 체력 게이지
    private void HPGauge()
    {

    }

    // 움직임
    private void Move()
    {
        animator.SetBool("isWalk", true);


    }

    // 발사
    private IEnumerator Shoot()
    {
        yield return null;
    }

    // 근접공격
    private IEnumerator SwordHit()
    {
        yield return null;
    }
}
