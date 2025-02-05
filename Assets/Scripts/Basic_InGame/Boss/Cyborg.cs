using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Cyborg : MonoBehaviour
{
    // �׺���̼�
    private NavMeshAgent agent;

    // ����ü�� ������
    private Slider bossHP_Bar;

    // ����ü�� ���� ǥ��
    private Slider bossHP_Number;

    // ���� UI
    private GameObject bossUI;

    // ���� �ִϸ�����
    private Animator animator;

    private bool isDie;

    private int hp;

    private void Start()
    {
        bossUI.SetActive(true);

        agent.speed = 3;

        hp = 5000;
    }

    // ü�� ����
    private void MinousHP(int damage)
    {
        hp -= damage;

        // ����
        if (hp <= 0)
        {
            agent.isStopped = true;
            animator.SetTrigger("isDie");

            Destroy(gameObject, 2);
        }
    }



    // ü�� ������
    private void HPGauge()
    {

    }

    // ������
    private void Move()
    {
        animator.SetBool("isWalk", true);


    }

    // �߻�
    private IEnumerator Shoot()
    {
        yield return null;
    }

    // ��������
    private IEnumerator SwordHit()
    {
        yield return null;
    }
}
