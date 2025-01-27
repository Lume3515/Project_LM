using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 카메라
    private Camera mainCamera;

    // true : 가능
    private bool shooting = true;
    public bool Shooting_Bool => shooting;
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

    private static PlayerFire instance;
    public static PlayerFire Instance => instance;

    // 최대 탄창
    private int maxAmmo;
    // 현재 탄창
    private int currAmmo;

    // 장전중 이거나 장전이 필요
    private bool isReload;

    public bool IsReload => isReload;

    private Vector3 screen_RayPos;

    [SerializeField] ObjectPooling bulletUI_ObjectPooling;

    // 총알수 보여주는 UI 최상위 부모
    [SerializeField] Transform ammoNumberParentTr;

    private int index;

    // 쏠수 업음
    private bool notShoot;

    // 오왼위아래
    [SerializeField] Transform[] aims;

    // 조준점 옆 탄창
    [SerializeField] Image ammoClip_Number;
    private void Start()
    {

        if (instance == null) instance = this;

        shootingDelay = new WaitForSeconds(0.1f);

        maxTime = 0.7f;

        mainCamera = Camera.main;

        fireSpeed = 500;

        mainCamera.fieldOfView = 60;

        maxAmmo = 30;

        // 애니메이션
        animator.SetTrigger("reload");

        StartCoroutine(Reload());
    }

    private void Update()
    {
        //Debug.Log(shootingType);
        //Debug.Log(shootingType);

        //firePos.rotation = mainCamera.transform.rotation * Quaternion.Euler(addPos);

        // 총알이 없거나 최대총알 수보다 낮을 때만
        if (currAmmo < maxAmmo && shooting)
        {
            if (currAmmo <= 0) notShoot = true;
            else notShoot = false;

            // 시간 동기화
            if (isReload)
            {
                AnimatorStateInfo stateInfo_Reload = animator.GetCurrentAnimatorStateInfo(0);
                float progress_Reload = stateInfo_Reload.normalizedTime % 1;

                animator.SetFloat("StateProgress_Reload", progress_Reload);
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReload)
            {


                StartCoroutine(Reload());
                animator.SetBool("reload", true);
            }

        }
        //Debug.Log(Gamemanager.Instance.ShootingType);

        // 좌클릭 시
        if (Input.GetMouseButton(0) && shooting && Gamemanager.Instance.ShootingType != ShootingType.Run && !isReload && !notShoot)
        {
            // 애니메이션
            animator.SetBool("isShoot", true);

            StartCoroutine(Fire());
        }

        // 우클릭 시 > 토글
        if (Input.GetMouseButton(1) && Gamemanager.Instance.ShootingType != ShootingType.Run && !isReload && !notShoot)
        {
            Gamemanager.Instance.ShootingType = ShootingType.Shoulder;
            //Debug.Log(shootingType);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 32, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(20, 0), Time.deltaTime * 10);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-20, 0), Time.deltaTime * 10);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 20), Time.deltaTime * 10);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -20), Time.deltaTime * 10);

            ShoulderAndAim = true;
            //Debug.Log("1");
        }
        // 달릴 때
        else if (Gamemanager.Instance.ShootingType == ShootingType.Walk)
        {
            Gamemanager.Instance.ShootingType = ShootingType.Walk;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(80, 0), Time.deltaTime * 5);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-80, 0), Time.deltaTime * 5);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 80), Time.deltaTime * 5);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -80), Time.deltaTime * 5);
        }
        // 뛸 때
        else if (Gamemanager.Instance.ShootingType == ShootingType.Run)
        {
            Gamemanager.Instance.ShootingType = ShootingType.Run;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(120, 0), Time.deltaTime * 5);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-120, 0), Time.deltaTime * 5);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 120), Time.deltaTime * 5);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -120), Time.deltaTime * 5);
        }
        // 서있을 때
        else
        {
            ShoulderAndAim = false;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(30, 0), Time.deltaTime * 5);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-30, 0), Time.deltaTime * 5);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 30), Time.deltaTime * 5);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -30), Time.deltaTime * 5);

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

        // 오류 났었는데 "currAmmo / "나누는 부분을 int로 해서 오류났었음 ㅠ
        ammoClip_Number.fillAmount = Mathf.Lerp(ammoClip_Number.fillAmount, currAmmo / (float)maxAmmo, Time.deltaTime * 10);
    }


    // 재장전
    private IEnumerator Reload()
    {
        isReload = true;

        SoundManager.Instance.Sound(SoundType.Reload);
        yield return new WaitForSeconds(2.5f);

        while (currAmmo < maxAmmo)
        {
            currAmmo++;
            bulletUI_ObjectPooling.OutPut();

            yield return null;
        }

        animator.SetBool("reload", false);
        isReload = false;
        notShoot = false;
        yield break;
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
        if (Physics.Raycast(screen_Aim, out hit, 150, 1 << 6 | 1 << 7))
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

        //// 레이와 총구의 거리가 가깝다면  위치를 카메라로 변경
        //if (Vector3.Distance(firePos.position, screen_RayPos) < 7.5f)
        //{
        //    firePos.position = mainCamera.transform.position;
        //}
        //else
        //{
        //    firePos.localPosition = originPos_firePos;
        //}

        // 바라보게 한다 Euler를 쓰면 위치부터가 다르기 때문에 글렀다. 그러므로 LookRotation을 쓴다.
        firePos.rotation = Quaternion.LookRotation(direction);



        // 총알 생성
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, Gamemanager.Instance.ShootingType, firePos, 1);

        // 총알 딜레이 용
        shooting = false;

        // 발사 총구 화염 생성
        m4MuzzleFlash.Play();

        if (index >= ammoNumberParentTr.childCount) index = 0;

        Transform child = ammoNumberParentTr.GetChild(index);
        bulletUI_ObjectPooling.Input(child.gameObject);
        index++;
        currAmmo--;

        SoundManager.Instance.Sound(SoundType.Shooting);
        StopCoroutine(Rebound());
        StartCoroutine(Rebound());

        // 총 딜레이
        yield return shootingDelay;

        shooting = true;
    }

    private float recoilAmount = 0f; // 반동 값 저장
    public float RecoliAmount => recoilAmount;
    private float recoilSpeed = 0.0002f; // 반동 진행 속도
    private float recoilDecaySpeed = 0.0001f; // 반동 복구 속도
    private float maxRecoil = -0.2f; // 최대 반동 각도  

    public IEnumerator Rebound()
    {
        float targetRecoil = recoilAmount + maxRecoil; // 목표 반동 각도
        float elapsedTime = 0f;

        // 반동 증가
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, targetRecoil, elapsedTime / recoilSpeed);
            yield return null;
        }

        // 반동 복구
        elapsedTime = 0f;
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, 0f, elapsedTime / recoilDecaySpeed);
            yield return null;
        }

        recoilAmount = 0f; // 반동 값 초기화

        // 추가 반동 올리기: 반동이 완전히 복구된 후 1/2만큼 더 올려줌
        recoilAmount = maxRecoil / 2f;
        yield return new WaitForSeconds(0.1f); // 약간의 딜레이를 두고

        // 복구된 반동을 다시 원위치로 돌아가게 설정
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, 0f, elapsedTime / recoilDecaySpeed);
            yield return null;
        }

        recoilAmount = 0f; // 최종적으로 반동 값 초기화
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawRay(firePos.position, firePos.forward * 150);

    //    //Gizmos.color = Color.red;

    //    //Gizmos.DrawRay(firePos.position, firePos.forward * 150);

    //    //Gizmos.DrawCube(screenPos, new Vector3(1, 1, 1));

    //    //    Gizmos.color = Color.red;

    //    //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    //}

}
