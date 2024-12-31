using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] ObjectPooling objectPooling;

    // true : 가능
    private bool shooting = true;

    // 발사 딜레이
    private WaitForSeconds shootingDelay;

    // 총구화염
    private ParticleSystem m4MuzzleFlash;

    // 발사 위치
    [SerializeField] GameObject m4FirePos;

    // 플레이어 애니메이터
    [SerializeField] Animator animator;

    // 총 내리는 시간
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
        // 좌클릭 시
        if (Input.GetMouseButton(0) && shooting)
        {
            StartCoroutine(Fire());
        }

        // 총 내리기
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
        // 발사 사운드
        shooting = false;

      GameObject spawn =  objectPooling.OutPut();
        //spawn.GetComponent<Bullet>().Setting(m4FirePos.transform.position, m4FirePos.transform);



        m4MuzzleFlash.Play();

        animator.SetBool("isShoot", true);
        animator.SetBool("isShoot", true);

        // 총 딜레이
        yield return shootingDelay;
        shooting = true;
    }
}
