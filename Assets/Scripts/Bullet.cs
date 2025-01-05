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

    // �߻� ��ġ
    private Vector3 fireDirection;

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

    // ź���� ����
    public void Setting(float speed, ShootingType type)
    {
        fireSpeed = speed;
        //Debug.Log(speed);

        gameObject.SetActive(true);

        shootingType = type;

        Fire();
    }

    private void Fire()
    {      
        switch (shootingType)
        {
            case ShootingType.Aim:
                carbonSpread = new Vector3(0, 0, 0);
                break;

            case ShootingType.Shoulder:
                carbonSpread = new Vector3(Random.Range(-0.006f, 0.006f), Random.Range(-0.006f, 0.006f), Random.Range(-0.006f, 0.006f));
                break;

            case ShootingType.Run:
                carbonSpread = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));

                break;

            case ShootingType.Walk:
                carbonSpread = new Vector3(Random.Range(-0.03f ,0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
                break;

            case ShootingType.Sit:
                carbonSpread = new Vector3(Random.Range(-0.008f, 0.008f), Random.Range(-0.008f, 0.008f), Random.Range(-0.008f, 0.008f));
                break;

            case ShootingType.SitWalk:
                carbonSpread = new Vector3(Random.Range(-0.03f, 0.3f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
                break;

            default: // ���ֱ� �� ����
                carbonSpread = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
                break;


        }


        transform.position = firePos.position;
        transform.rotation = firePos.rotation;

        fireDirection = firePos.forward + carbonSpread;

        bulletRb.velocity = Vector3.zero;

        bulletRb.AddForce(fireDirection * fireSpeed, ForceMode.Impulse);
    }


    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();

        objectPooling = GetComponentInParent<ObjectPooling>();

        firePos = Camera.main.transform;

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

            // ��ֹ� ������������, �÷��̾� ���� ��ġ�� ����, �Ѿ��� ������ ���� ������ �޶���
            Instantiate(impact_Obstacle, impact_Info.point, Quaternion.LookRotation(impact_Info.normal));
            objectPooling.Input(gameObject);

            Debug.Log("��ֹ� �浹");

            return;

        }

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

        // ��ֹ� ������������, �÷��̾� ���� ��ġ�� ����, �Ѿ��� ������ ���� ������ �޶���
        Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(impact_Info.normal));
        objectPooling.Input(gameObject);
    }


    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Destroy Zone"))
        {
            objectPooling.Input(gameObject);
            Debug.Log("���ӿ��� ����");
        }
    }
}
