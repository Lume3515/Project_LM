using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // ���� ��ġ
    [SerializeField] Transform followPos;

    // ī�޶� Ʈ������
    private Transform cameraTr;

    private void Start()
    {
        cameraTr = Camera.main.transform;
    }


    private void FixedUpdate()
    {        
        transform.position = Vector3.Lerp(transform.position, followPos.position, Time.deltaTime * 40);

        transform.LookAt(cameraTr);

        // ������ ����
        transform.rotation *= Quaternion.Euler(0, 200, 0);

    }



}
