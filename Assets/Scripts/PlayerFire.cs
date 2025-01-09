using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum DamageType
{
    HeadSHot,
    BodyShot,
    armShot,
    legShot,
}

// 발사 타입
public enum ShootingType
{
    Aim, // 조준
    Shoulder, // 견착  
    Run, // 뛰기
    Walk, // 걷기
    Sit, // 앉기
    Stand, // 서있기
    Obscuration, // 엄폐
    SitWalk    // 앉아서 걷기
}

public class PlayerFire : MonoBehaviour
{
    // 카메라
    private Camera mainCamera;

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

    [SerializeField] Transform firePos;

    // 타입
    private ShootingType shootingType;
    public ShootingType ShootingType { get { return shootingType; } set { shootingType = value; } }

    // 조준중이나, 견착중
    private bool shoulderAndAim;

    public bool ShoulderAndAim { get { return shoulderAndAim; } set { shoulderAndAim = value; } }


    private Vector3 targetPosition;
    private void Start()
    {
        shootingDelay = new WaitForSeconds(0.1f);

        maxTime = 0.7f;

        mainCamera = Camera.main;

        fireSpeed = 50;

        mainCamera.fieldOfView = 60;

      
    }

    private void Update()
    {
        //Debug.Log(shootingType);
        //Debug.Log(shootingType);

        //firePos.rotation = mainCamera.transform.rotation * Quaternion.Euler(addPos);

        // 좌클릭 시
        if (Input.GetMouseButton(0) && shooting)
        {

            // 애니메이션
            animator.SetBool("isShoot", true);

            StartCoroutine(Fire());
        }

        // 우클릭 시 > 토글
        if (Input.GetMouseButton(1))
        {
            shootingType = ShootingType.Shoulder;
            //Debug.Log(shootingType);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 32, Time.deltaTime * 10);
            ShoulderAndAim = true;
            //Debug.Log("1");
        }
        else
        {
            ShoulderAndAim = false;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 10);
            shootingType = ShootingType.Stand;
            //Debug.Log("2");
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

        targetPosition = Vector3.zero;

        RaycastHit hit;

        // 스크린 중앙에서 Ray를 발사 (UI의 중앙에서 발사)
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 10f));


        // Raycast로 충돌 지점 확인
        if (Physics.Raycast(ray, out hit, 300))
        {
            targetPosition = hit.point; 
        }
        else
        {
            // 만약 충돌하지 않으면 레이의 끝 지점을 목표로 설정
            targetPosition = ray.GetPoint(300);  
        }

        // 목표 위치를 향한 방향 벡터 계산
        Vector3 direction = (targetPosition - firePos.position).normalized;

        // firePos를 목표 방향으로 회전
        firePos.rotation = Quaternion.LookRotation(direction);

        // 총알 생성
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, shootingType, firePos);

        // 총알 딜레이 용
        shooting = false;

        // 발사 총구 화염 생성
        m4MuzzleFlash.Play();


        // 총 딜레이
        yield return shootingDelay;

        shooting = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(firePos.position, firePos.forward * 170);

        //Gizmos.color = Color.red;

        //Gizmos.DrawRay(firePos.position, firePos.forward * 150);

        //Gizmos.DrawCube(screenPos, new Vector3(1, 1, 1));

        //    Gizmos.color = Color.red;

        //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    }

}
