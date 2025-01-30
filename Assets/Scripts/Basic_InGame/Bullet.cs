using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;

    private float fireSpeed;

    // 발사할 대상의 트랜스 폼
    private Transform firePos;

    // 적의 임팩트
    private ParticleSystem impact_Enemy;

    // 장애물의 임팩트
    private ParticleSystem impact_Obstacle;

    // 임펙트의 정보
    private ContactPoint impact_Info;

    // 오브젝트 풀링
    private ObjectPooling objectPooling;

    // 머리
    private int head_Damage;
    // 몸통
    private int body_Damage;
    // 팔
    private int arm_Damage;
    // 다리
    private int Leg_Damage;

    // ShootingType변수
    private ShootingType shootingType;

    // 탄퍼짐
    private Vector3 carbonSpread;

    // 고유번호 : 0 : 적, 1 : 플레이어
    private int actorNumber;

    // 탄퍼짐 정도
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
        // 반동
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

            default: // 서있기 및 엄폐
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

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Obstacle, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);

            //Debug.Log("장애물 충돌");         

        }
        #region// 적(Enemy)

        if (actorNumber == 1) // 플레이어가 발사했을 때만
        {
            // 부위에 따른 체력감소
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

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }
        #endregion       
    }

    private void OnTriggerStay(Collider other)
    {
        #region// 플레이어
        if (actorNumber == 0 && other.CompareTag("EnemyAim")) // 적이 발사했을 때만
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
            //Debug.Log("게임에서 나감");
        }
    }
}
