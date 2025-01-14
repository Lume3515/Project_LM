using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;

    private float fireSpeed;

    // 발사할 대상의 트랜스 폼
    private Transform firePos;

    // 발사 위치
    private Vector3 fireDirection;

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

    private PhotonView photonView;

    // 탄퍼짐 정도
    public void Setting(float speed, ShootingType type, Transform pos, PhotonView photonview)
    {
        this.photonView = photonview;

        gameObject.SetActive(true);

        fireSpeed = speed;
        //Debug.Log(speed);

        firePos = pos;


        shootingType = type;


        Fire();
    }

    
    private void Fire()
    {

        // 반동
        switch (shootingType)
        {
            case ShootingType.Aim:
                carbonSpread = new Vector3(0, 0, 0);
                break;

            case ShootingType.Shoulder:
                carbonSpread = new Vector3(Random.Range(-0.006f, 0.006f), Random.Range(-0.006f, 0.006f), Random.Range(-0.006f, 0.006f));
                break;

            case ShootingType.Run:
                carbonSpread = new Vector3(Random.Range(-0.07f, 0.07f), Random.Range(-0.07f, 0.07f), Random.Range(-0.07f, 0.07f));

                break;

            case ShootingType.Walk:
                carbonSpread = new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f));
                break;

            case ShootingType.Sit:
                carbonSpread = new Vector3(Random.Range(-0.04f, 0.04f), Random.Range(-0.04f, 0.04f), Random.Range(-0.04f, 0.04f));
                break;

            case ShootingType.SitWalk:
                carbonSpread = new Vector3(Random.Range(-0.05f, 0.5f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
                break;

            default: // 서있기 및 엄폐
                carbonSpread = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
                break;


        }


        transform.position = firePos.position;
        transform.rotation = firePos.rotation;

        fireDirection = (firePos.forward + firePos.right * carbonSpread.x + firePos.up * carbonSpread.y).normalized;

        bulletRb.velocity = Vector3.zero;

        bulletRb.AddForce(fireDirection * fireSpeed, ForceMode.Impulse);
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
        // 부위에 따른 체력감소
        else if (collision.collider.CompareTag("Zombie_Head"))
        {
            collision.gameObject.GetComponentInParent<Zombie>().MinusHP(head_Damage, DamageType.HeadSHot);

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }
        else if (collision.collider.CompareTag("Zombie_Arm"))
        {
            collision.gameObject.GetComponentInParent<Zombie>().MinusHP(arm_Damage, DamageType.armShot);

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }
        else if (collision.collider.CompareTag("Zombie_Leg"))
        {
            collision.gameObject.GetComponentInParent<Zombie>().MinusHP(Leg_Damage, DamageType.legShot);

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }
        else if (collision.collider.CompareTag("Zombie_Body"))
        {
            collision.gameObject.GetComponentInParent<Zombie>().MinusHP(body_Damage, DamageType.BodyShot);

            // 임팩트 프립팹을생성, 총알의 충돌 위치에 생성, 충돌 시 총알의 각도를 반전 시켜 인스턴싱
            Instantiate(impact_Enemy, impact_Info.point, Quaternion.LookRotation(transform.forward * -1));
            objectPooling.Input(gameObject);
        }


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
