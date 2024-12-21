using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{   

    // 플레이어 트랜스 폼
    private Transform playerTr;
    
    // 움직일 방향
    private Vector3 moveDirection; 

    private void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").transform;            

    }

    private void Update()
    {
        moveDirection = new Vector3(playerTr.position.x, playerTr.position.y + 2, playerTr.position.z - 5);       
    }

    private void FixedUpdate()
    {
       transform.position = Vector3.Slerp(transform.position, moveDirection, 0.8f); 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }

   
}
