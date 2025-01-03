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
    [SerializeField] ParticleSystem m4MuzzleFlash;

    // �÷��̾� �ִϸ�����
    [SerializeField] Animator animator;

    // �Ѿ��� �ӵ�
    private float fireSpeed;

    // �� ������ �ð�
    private float time;
    private float maxTime;

    // ������Ʈ Ǯ�� ��ũ��Ʈ
    [SerializeField] ObjectPooling objectPooling;

    // �Ѿ� ��ü
    private GameObject bulletObj;     

    private void Start()
    {
        shootingDelay = new WaitForSeconds(0.1f);  

        maxTime = 0.7f;       

        fireSpeed = 50;
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


        // �Ѿ� ����
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed);

        // �Ѿ� ������ ��
        shooting = false;

        // �߻� ����
        m4MuzzleFlash.Play();

        // �ִϸ��̼�
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
