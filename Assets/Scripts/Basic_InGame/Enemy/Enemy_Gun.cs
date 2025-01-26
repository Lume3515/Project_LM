using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class Enemy_Gun : MonoBehaviour
{
    private Animator animator;

    private Transform playerTr;

    private NavMeshAgent agent;

    // ����
    [SerializeField] Transform weapon;
    [SerializeField] Transform firePos;

    // ����ü��
    private int currHP;
    // �׾�����?
    private bool die;

    // ������ ��ġ
    private Vector3 movePos;

    // �÷��̾�� �������?
    private bool nearPlayer;

    // �Ѿ� ������Ʈ Ǯ�� ��ü
    private ObjectPooling bulletPool;

    // �� ��� ���� ������Ʈ Ǯ��
    private ObjectPooling enemy_Gun_Pool;

    // ��ֹ���
    private NavMeshObstacle[] obscurationsNMObstacle;
    // ��ֹ����� ��ġ
    private Transform[] obscurationsTr;

    // �� �� ���� ������ �� �޴� �ٸ� �ٽ� �÷��̾� ���� �̵�
    private float damageTimer;

    // ���� ������
    private WaitForSeconds attackDelay;
    private bool attackDelay_Bool;

    private float fireSpeed;

    private bool firstSpawn = true; // ó�� �����ƴ���?

    private void Awake()
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

        attackDelay = new WaitForSeconds(0.14f);

        enemy_Gun_Pool = GetComponentInParent<ObjectPooling>();

        bulletPool = GameObject.FindWithTag("BulletPool").GetComponent<ObjectPooling>();
    }

    public void Setting(Vector3 pos)
    {
        gameObject.SetActive(true);
        firstSpawn = true;

        fireSpeed = 500;

        die = false;

        currHP = 80;

        agent.Warp(pos); // �����̵�

        StartCoroutine(Move());
    }

    // ü�°��� ����
    public void MinousHP(int damage, DamageType type)
    {
        if (die) return;
        damageTimer = 0; // �Ѿ˿� �¾Ҵٸ� 0���� �Ҵ��ؼ� �� �ڸ� �����ϱ�
        currHP -= damage;

        if (currHP <= 0)
        {
            die = true;
            animator.SetTrigger("die");

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

            StartCoroutine(Die());
            PlayerScore.enemyGun++;
        }
    }

    private GameObject bulletObj;

    // ���� ����
    private IEnumerator Die()
    {
        animator.SetTrigger("die");

        yield return new WaitForSeconds(1.23f);

        enemy_Gun_Pool.Input(gameObject);
    }

    //  �߻籸��
    private IEnumerator Fire()
    {
        attackDelay_Bool = true;// �ߺ� �߻� ����

        animator.SetBool("attack", true);        

        // �Ѿ� ����
        bulletObj = bulletPool.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, Gamemanager.Instance.ShootingType, firePos, 0);

        yield return attackDelay;

        attackDelay_Bool = false;
    }

    private bool firstColl; // �ѹ��� ȣ��
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
                if (firstColl) damageTimer = 0; // �÷��̾�� ���� �� true�� �ǰ� true�� �Ǹ� 0�� �Ҵ��
                firstColl = false;

                //Debug.Log("��ֹ��� �̵���");

                FindPos();
                agent.destination = movePos;

                damageTimer += Time.deltaTime;

                if (damageTimer >= 20) // 20�� ���� �ƹ� ������ �����ٸ� �÷��̾ ��ó�� ���ٰ� �Ǵ��ϰ� �ٽ� �÷��̾ ���� �̵���
                {
                    nearPlayer = false;
                }
            }

            // ����ٸ�
            if (agent.velocity.magnitude == 0 && !firstSpawn)
            {
                //Debug.Log("����");

                // ��ġ�� �ٸ� ����
                if (weapon.localPosition.x != 0.211f || weapon.localPosition.y != -0.03f || weapon.localPosition.z != -0.109f)
                {
                    //Debug.Log("��ġ �ٸ�");
                    weapon.localPosition = new Vector3(0.211f, -0.03f, -0.109f);
                }
                // ������ �ٸ� ����
                if (weapon.localRotation.x != -1.414f || weapon.localRotation.y != -48.906f || weapon.localRotation.z != -56.61f)
                {
                    weapon.localRotation = Quaternion.Euler(-1.414f, -48.906f, -56.61f);
                }

                transform.LookAt(playerTr.position);

                if (!attackDelay_Bool) StartCoroutine(Fire());
            }
            else
            {
                firstSpawn = false; // �̰� �ϴ� ������ ó�� ���� �Ǹ� �ӵ��� 0�̶� �����ִ� if���� �۵��ؼ� �̱� �����̴�.

                // ��ġ�� �ٸ� ����
                if (weapon.localPosition.x != 0.236f || weapon.localPosition.y != -0.042f || weapon.localPosition.z != -0.033f)
                {
                    //Debug.Log("��ġ �ٸ�");
                    weapon.localPosition = new Vector3(0.236f, -0.042f, -0.033f);
                }
                // ������ �ٸ� ����
                if (weapon.localRotation.x != -3.971f || weapon.localRotation.y != -60.232f || weapon.localRotation.z != -100.141f)
                {
                    weapon.localRotation = Quaternion.Euler(-3.971f, -60.232f, -100.141f);
                }

                animator.SetBool("attack", false);
            }



            yield return null;
        }
    }

    // �̵���ġ ã��
    private void FindPos()
    {
        GameObject[] enemyGunObjects = GameObject.FindGameObjectsWithTag("Enemy_Gun");
        NavMeshAgent[] otherAgents = new NavMeshAgent[enemyGunObjects.Length];

        for (int j = 0; j < enemyGunObjects.Length; j++)
        {
            otherAgents[j] = enemyGunObjects[j].GetComponent<NavMeshAgent>();
        }

        for (int i = 0; i < obscurationsTr.Length; i++)
        {
            Vector3 potentialPos = obscurationsTr[i].position + new Vector3(0.1f, 0, 0.94f); // �̵��� ��ֹ�(����)�� ��ġ

            bool isOverlapping = false; // �ߺ�����?
            foreach (NavMeshAgent nav in otherAgents)
            {
                if (nav != agent && (nav.destination - potentialPos).sqrMagnitude < 8) // agent�� ������ �ƴϰ�, ���������ϰ� ũ�Ⱑ 8���� �۴ٸ�(�ٸ� agent �� ai�� �̹� �ִٸ�) �ߺ�ó��
                {
                    isOverlapping = true;
                    break;
                }
            }

            if (!isOverlapping && (transform.position - obscurationsTr[i].position).sqrMagnitude <
                (transform.position - movePos).sqrMagnitude) // �� ��ġ���� ��ֹ��� ��ġ�� ���� �� �Ÿ��� ������? movePos���� �۴ٸ�(�����ٸ�) ���ο� �� �Ҵ�, �׸��� �� ��ġ�� �ߺ��� �ƴ� ��
            {
                movePos = potentialPos;
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
