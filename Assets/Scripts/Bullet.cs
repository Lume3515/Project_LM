using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;

    private float fireSpeed;

    [SerializeField] Transform firePos;

    private Vector3 fireDirection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = firePos.position;
            transform.rotation = firePos.rotation;            

            fireDirection = firePos.forward;

            bulletRb.velocity = Vector3.zero;

            bulletRb.AddForce(fireDirection * fireSpeed, ForceMode.Impulse);
        }
    }

    private void Start()
    {
        bulletRb = GetComponent<Rigidbody>();

        fireSpeed = 10;
       

    }



}
