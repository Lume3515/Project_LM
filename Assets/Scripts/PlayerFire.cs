using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] ObjectPooling objectPooling;

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

    // �� ������ �ð�
    private float time;
    private float maxTime;

    private void Start()
    {
        shootingDelay = new WaitForSeconds(0.07f);

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
        // �߻� ����
        shooting = false;

      GameObject spawn =  objectPooling.OutPut();
        //spawn.GetComponent<Bullet>().Setting(m4FirePos.transform.position, m4FirePos.transform);



        m4MuzzleFlash.Play();

        animator.SetBool("isShoot", true);
        animator.SetBool("isShoot", true);

        // �� ������
        yield return shootingDelay;
        shooting = true;
    }
}
