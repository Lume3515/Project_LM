using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_BasicAndTankerAndSpeed : MonoBehaviour
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
    public bool IsAttack => isAttack;

    // ���� �ִϸ��̼� ������
    private WaitForSeconds attackDelay;

    // ������Ʈ Ǯ��
    private ObjectPooling objectPooling;

    // ������
    private int damage;

    //���
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

    // ü�� ���� > �Ѿ� ��ũ��Ʈ���� �Ҵ�
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

            // �̵� ���߱�
            die = true;
            agent.isStopped = true;
            animator.SetTrigger("die");
            StopCoroutine(Attack());
            StartCoroutine(Die());
        }
    }

    // �״� �ð� ������
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

    // ������
    private void MoveMent()
    {
        agent.updateRotation = true;
        agent.destination = playerTr.position;
        agent.speed = moveSpeed;
    }

    // �����ϴ� �Լ�
    private IEnumerator Attack()
    {
        isAttack = true; // ���� �� ���� ����
        stopAttack = false;
        animator.SetTrigger("attack");
        animator.SetBool("walk", false);

        RaycastHit hit;
        // �÷��̾ �տ� ���� ���� �ٽ� ã�� �Ĵٺ��� / ���� ��ġ �����ְ� ������ ��� �÷��̾� ���̾
        if (!Physics.Raycast((transform.position + new Vector3(0, 1, 0)), transform.forward, out hit, 30, 1 << 3))
        {
            transform.LookAt(playerTr.transform);
        }

        StartCoroutine(MinousHP_Player());

        yield return attackDelay; // ���� ������ ���� ���

        animator.SetBool("walk", true);
        isAttack = false; // ���� ���� ���� ����
    }

    private WaitForSeconds minousHPDelay;

    private PlayerState playerState;

    // �÷��̾� ü�� ����
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

        // ���� ���� ���� ����
        if (other.CompareTag("Player") && !isAttack)
        {
            if (playerHp == null) playerHp = other.GetComponent<PlayerHP>();
            if (playerState == null) playerState = other.GetComponent<PlayerState>();

            if (!isAttack) // �ڷ�ƾ �ߺ� ���� ����
            {
                StartCoroutine(Attack()); // �ڷ�ƾ ����
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
