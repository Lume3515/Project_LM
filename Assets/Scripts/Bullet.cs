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

            // ��ֹ� ������������, �÷��̾� ���� ��ġ�� ����, �Ѿ��� ������ ���� ������ �޶���
            Instantiate(impact_Obstacle, impact_Info.point, Quaternion.LookRotation(impact_Info.normal));

            objectPooling.Input(gameObject);
            Debug.Log("��ֹ� �浹");
        }             
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
