using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

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

    // 조준중이나, 견착중
    private bool shoulderAndAim;

    public bool ShoulderAndAim { get { return shoulderAndAim; } set { shoulderAndAim = value; } }



    private Vector3 screen_RayPos;
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
            Gamemanager.Instance.ShootingType = ShootingType.Shoulder;
            //Debug.Log(shootingType);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 32, Time.deltaTime * 10);
            ShoulderAndAim = true;
            //Debug.Log("1");
        }
        else
        {
            ShoulderAndAim = false;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 10);
            Gamemanager.Instance.ShootingType = ShootingType.Stand;
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
        // Ray의 위치? 화면 조준점의 위치
        screen_RayPos = Vector3.zero;

        // 충돌 지점
        RaycastHit hit;

        // 1920 X 1080을 각각 나누기 2 하면 960 X 540이 나오는데 이는 화면의 가운데를 나타낸다. 또한 ScreenPointToRay는 스크린을 월드의 Ray로 바꿔주는 함수이다.
        Ray screen_Aim = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 5));

        // 레이를 쏘고 맞으면 hit에 값을 넣어줌 사정거리는 150이다.
        if (Physics.Raycast(screen_Aim, out hit, 150))
        {
            // 맞을 시 hit의 정보를 가져옴
            screen_RayPos = hit.point;
        }
        else
        {
            // 안 맞을 시 Ray의 150사정거리 정보를 가져옴
            screen_RayPos = screen_Aim.GetPoint(150);
        }

        // 방향구하는 식 이다. screen_RayPos - firePos.position(normalized은 정규화를 위해서 이다 1로 만들기 위해)
        Vector3 direction = (screen_RayPos - firePos.position).normalized;

        // 바라보게 한다 Euler를 쓰면 위치부터가 다르기 때문에 글렀다. 그러므로 LookRotation을 쓴다.
        firePos.rotation = Quaternion.LookRotation(direction);


        // 총알 생성
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, Gamemanager.Instance.ShootingType, firePos, null);

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
        Gizmos.DrawRay(firePos.position, firePos.forward * 150);

        //Gizmos.color = Color.red;

        //Gizmos.DrawRay(firePos.position, firePos.forward * 150);

        //Gizmos.DrawCube(screenPos, new Vector3(1, 1, 1));

        //    Gizmos.color = Color.red;

        //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    }

}
