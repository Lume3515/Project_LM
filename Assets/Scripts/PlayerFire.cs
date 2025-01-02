using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{  
    // true : ����
    private bool shooting = true;

    // �߻� ������
    private WaitForSeconds shootingDelay;

    // �ѱ�ȭ��
    private ParticleSystem m4MuzzleFlash;

    // �߻� ��ġ
    [SerializeField] GameObject m4FirePos;

    // �÷��̾� �ִϸ�����
    [SerializeField] Animator animator;

    // ī�޶� Ʈ������
    private Transform cameraTr;

    // �� ������ �ð�
    private float time;
    private float maxTime;

    private void Start()
    {
        shootingDelay = new WaitForSeconds(0.07f);

        cameraTr = Camera.main.transform;

        maxTime = 0.7f;

        m4MuzzleFlash = m4FirePos.GetComponent<ParticleSystem>();      
        
    }

    private void Update()
    {
        // ��Ŭ�� ��
        if (Input.GetMouseButton(0) && shooting)
        {
            StartCoroutine(Fire());
        }

        // �� ������
        if (shooting)
        {
            time += Time.deltaTime;

            if (time > maxTime)
            {
                time = 0;

                animator.SetBool("isShoot", false);

                shooting = true;
            }
        }
    }

    private IEnumerator Fire()
    {
        // ���̷� ����



        //// �浹ü
        //RaycastHit hit;

        //// �߻���ġ, ����, ����(���� ��), �ִ� �Ÿ�,���̾� 6����
        //if (Physics.Raycast(cameraTr.position, cameraTr.forward, out hit, 150, 1 << 6))
        //{
        //    // ������ ������

            

        //    // �̸� ����
        //    Debug.Log(hit.collider.name);
        //}


        // �߻� ����
        shooting = false;

        m4MuzzleFlash.Play();

        animator.SetBool("isShoot", true);
        animator.SetBool("isShoot", true);

        // �� ������
        yield return shootingDelay;
        shooting = true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    //}

}
