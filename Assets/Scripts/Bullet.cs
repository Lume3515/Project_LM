using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �߻�ӵ�
    private float fireSpeed;

    // ������ �ٵ�
    private Rigidbody bulletRb;

    // ����
    private Vector3 direction;

    private ObjectPooling objectPooling;

    private void Start()
    {
        bulletRb = GetComponent<Rigidbody>();

        fireSpeed = 0;

       

        objectPooling = GetComponentInParent<ObjectPooling>();
    }

    public void Setting(Vector3 pos, Transform rot)
    {
        transform.position = pos;
        transform.rotation = rot.rotation;
    }

    private void OnEnable()
    {
        direction = Vector3.left;
    }

    private void Update()
    {
        bulletRb.velocity = (direction) * fireSpeed;
    }



    // ���� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Input Area"))
        {
            Debug.Log("����");
            objectPooling.Input(gameObject);
        }
    }
}
