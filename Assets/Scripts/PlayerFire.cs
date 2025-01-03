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
    [SerializeField] ParticleSystem m4MuzzleFlash;

    // 플레이어 애니메이터
    [SerializeField] Animator animator;

    // 총알의 속도
    private float fireSpeed;

    // 총 내리는 시간
    private float time;
    private float maxTime;

    // 오브젝트 풀링 스크립트
    [SerializeField] ObjectPooling objectPooling;

    // 총알 객체
    private GameObject bulletObj;     

    private void Start()
    {
        shootingDelay = new WaitForSeconds(0.1f);  

        maxTime = 0.7f;       

        fireSpeed = 50;
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


        // 총알 생성
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed);

        // 총알 딜레이 용
        shooting = false;

        // 발사 사운드
        m4MuzzleFlash.Play();

        // 애니메이션
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
