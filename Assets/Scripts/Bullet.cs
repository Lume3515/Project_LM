using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 발사속도
    private float fireSpeed;

    // 리지드 바디
    private Rigidbody bulletRb;

    // 방향
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



    // 빠져 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Input Area"))
        {
            Debug.Log("나감");
            objectPooling.Input(gameObject);
        }
    }
}
