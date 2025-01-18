using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // 따라갈 위치
    [SerializeField] Transform followPos;

    // 카메라 트랜스폼
    private Transform cameraTr;

    private void Start()
    {
        cameraTr = Camera.main.transform;
    }


    private void FixedUpdate()
    {        
        transform.position = Vector3.Lerp(transform.position, followPos.position, Time.deltaTime * 40);

        transform.LookAt(cameraTr);

        // 더해줄 각도
        transform.rotation *= Quaternion.Euler(0, 200, 0);

    }



}
