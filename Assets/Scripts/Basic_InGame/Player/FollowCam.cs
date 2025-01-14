using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Transform playerTr;

    private void Awake()
    {
        
    }

    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.Lerp(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerTr.position.x, 0, playerTr.position.z), Time.deltaTime * 15);
        transform.position = direction + new Vector3(0, 1.421f, 0);
    }
}
