using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{  
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

    // 카메라 트랜스폼
    private Transform cameraTr;

    // 총 내리는 시간
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
        // 레이로 감지



        //// 충돌체
        //RaycastHit hit;

        //// 발사위치, 방향, 추출(리턴 값), 최대 거리,레이어 6번만
        //if (Physics.Raycast(cameraTr.position, cameraTr.forward, out hit, 150, 1 << 6))
        //{
        //    // 데미지 입히기

            

        //    // 이름 찍어보기
        //    Debug.Log(hit.collider.name);
        //}


        // 발사 사운드
        shooting = false;

        m4MuzzleFlash.Play();

        animator.SetBool("isShoot", true);
        animator.SetBool("isShoot", true);

        // 총 딜레이
        yield return shootingDelay;
        shooting = true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    //}

}
