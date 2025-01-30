using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;

    private float fireSpeed;

    // �߻��� ����� Ʈ���� ��
    private Transform firePos;

    // ���� ����Ʈ
    private ParticleSystem impact_Enemy;

    // ��ֹ��� ����Ʈ
    private ParticleSystem impact_Obstacle;

    // ����Ʈ�� ����
    private ContactPoint impact_Info;

    // ������Ʈ Ǯ��
    private ObjectPooling objectPooling;

    // �Ӹ�
    private int head_Damage;
    // ����
    private int body_Damage;
    // ��
    private int arm_Damage;
    // �ٸ�
    private int Leg_Damage;

    // ShootingType����
    private ShootingType shootingType;

    // ź����
    private Vector3 carbonSpread;

    // ������ȣ : 0 : ��, 1 : �÷��̾�
    private int actorNumber;

    // ź���� ����
    public void Setting(float speed, ShootingType type, Transform pos, int number)
    {

        gameObject.SetActive(true);
        actorNumber = number;

        fireSpeed = speed;
        //Debug.Log(speed);

        firePos = pos;


        shootingType = type;


        Rebound();
    }

    private void Rebound()
    {
        // �ݵ�
        switch (shootingType)
        {
            case ShootingType.Shoulder:
                carbonSpread = new Vector3(0, 0, 0);
                break;

            case ShootingType.Run:
                carbonSpread = new Vector3(Random.Range(-0.07f, 0.07f), Random.Range(-0.07f, 0.07f), Random.Range(-0.07f, 0.07f));

                break;

            case ShootingType.Walk:
                carbonSpread = new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f));
                break;

            case ShootingType.Sit:
                carbonSpread = new Vector3(Random.Range(-0.005f, 0.005f), Random.Range(-0.005f, 0.005f), Random.Range(-0.05f, 0.05f));
                break;

            case ShootingType.SitWalk:
                carbonSpread = new Vector3(Random.Range(-0.008f, 0.008f), Random.Range(-0.008f, 0.008f), Random.Range(-0.008f, 0.008f));
                break;

            case ShootingType.Null:
                carbonSpread = new Vector3(0, 0, 0);
                break;

            default: // ���ֱ� �� ����
                carbonSpread = new Vector3(Random.Range(-0.002f, 0.002f), Random.Range(-0.002f, 0.002f), Random.Range(-0.002f, 0.002f));
                break;
        }

        Fire();
    }

    private void Fire()
    {
        transform.position = firePos.position;
        transform.rotation = firePos.rotation;     

        bulletRb.velocity = Vector3.zero;

        bulletRb.AddForce((transform.forward + carbonSpread)  * fireSpeed , ForceMode.Impulse);
    }


    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();

        objectPooling = GetComponentInParent<ObjectPooling>();

        impact_Enemy = Resources.Load("Bullet_Impact_Enemy").GetComponentInChildren<ParticleSystem>();
        impact_Obstacle = Resources.Load("Bullet_Impact_Obstacl").GetComponentInChildren<ParticleSystem>();

        head_Damage = 20;
        body_Damage = 15;
        arm_Damage = 10;
        Leg_Damage = 5;

    }

    private void OnCollisionEnter(Collision collision)
    {
        impact_Info = collision.GetContact(0);

        if (collision.collider.CompareTag("Map"))
        {
            //Debug.Log(impact_Info.normal);

            // ����Ʈ ������������, �Ѿ��� �浹 ��ġ�� ����, �浹 �� �Ѿ��� ������ ���� ���� �ν��Ͻ�
            Instantiate(impact_Obstacle, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);

            //Debug.Log("��ֹ� �浹");         

        }
        #region// ��(Enemy)

        if (actorNumber == 1) // �÷��̾ �߻����� ����
        {
            // ������ ���� ü�°���
            if (collision.collider.CompareTag("Zombie_Head"))
            {
                collision.gameObject.GetComponentInParent<Zombie>().MinusHP(head_Damage, DamageType.HeadSHot);
            }
            else if (collision.collider.CompareTag("Zombie_Arm"))
            {
                collision.gameObject.GetComponentInParent<Zombie>().MinusHP(arm_Damage, DamageType.armShot);
            }
            else if (collision.collider.CompareTag("Zombie_Leg"))
            {
                collision.gameObject.GetComponentInParent<Zombie>().MinusHP(Leg_Damage, DamageType.legShot);
            }
            else if (collision.collider.CompareTag("Zombie_Body"))
            {
                collision.gameObject.GetComponentInParent<Zombie>().MinusHP(body_Damage, DamageType.BodyShot);
            }



            if (collision.collider.CompareTag("EnemyGun_Body"))
            {
                collision.collider.GetComponentInParent<Enemy_Gun>().MinousHP(body_Damage, DamageType.BodyShot);
            }
            else if (collision.collider.CompareTag("EnemyGun_Head"))
            {
                collision.collider.GetComponentInParent<Enemy_Gun>().MinousHP(head_Damage, DamageType.HeadSHot);
            }
            else if (collision.collider.CompareTag("EnemyGun_Arm"))
            {
                collision.collider.GetComponentInParent<Enemy_Gun>().MinousHP(arm_Damage, DamageType.armShot);
            }
            else if (collision.collider.CompareTag("EnemyGun_Leg"))
            {
                collision.collider.GetComponentInParent<Enemy_Gun>().MinousHP(Leg_Damage, DamageType.legShot);
            }

            // ����Ʈ ������������, �Ѿ��� �浹 ��ġ�� ����, �浹 �� �Ѿ��� ������ ���� ���� �ν��Ͻ�
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }
        #endregion       
    }

    private void OnTriggerStay(Collider other)
    {
        #region// �÷��̾�
        if (actorNumber == 0 && other.CompareTag("EnemyAim")) // ���� �߻����� ����
        {
            other.GetComponentInParent<PlayerHP>().MinousHP(13);
            objectPooling.Input(gameObject);

        }
        #endregion
    }



    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Destroy Zone"))
        {
            objectPooling.Input(gameObject);
            //Debug.Log("���ӿ��� ����");
        }
    }
}
