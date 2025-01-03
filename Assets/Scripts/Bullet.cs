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

    public void Setting(float speed)
    {
        fireSpeed = speed;
        //Debug.Log(speed);

        gameObject.SetActive(true);

        Fire();
    }

    private void Fire()
    {

        transform.position = firePos.position;
        transform.rotation = firePos.rotation;

        fireDirection = firePos.forward;

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

    }

    private void OnCollisionEnter(Collision collision)
    {          

        if (collision.collider.CompareTag("Map"))
        {
            impact_Info = collision.GetContact(0);
            //Debug.Log(impact_Info.normal);

            // 장애물 프립팹을생성, 플레이어 정보 위치에 생성, 총알의 각도에 따라 방향이 달라짐
            Instantiate(impact_Obstacle, impact_Info.point, Quaternion.LookRotation(impact_Info.normal));

            objectPooling.Input(gameObject);
            Debug.Log("장애물 충돌");
        }             
    }

    private void OnTriggerExit(Collider other)
    {
        
    
        if (other.CompareTag("Destroy Zone"))
        {
            objectPooling.Input(gameObject);
            Debug.Log("게임에서 나감");
        }
    }
}
