using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform playerTr;

    [SerializeField] Vector3 addPos;

    private void Awake()
    {
        
    }

    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.Lerp(transform.position,playerTr.position + addPos, Time.deltaTime * 15);
        transform.position = direction;
    }
}
