using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // ī�޶�
    private Camera mainCamera;

    // true : ����
    private bool shooting = true;
    public bool Shooting_Bool => shooting;
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

    [SerializeField] Transform firePos;

    // �������̳�, ������
    private bool shoulderAndAim;

    public bool ShoulderAndAim { get { return shoulderAndAim; } set { shoulderAndAim = value; } }

    private static PlayerFire instance;
    public static PlayerFire Instance => instance;

    // �ִ� źâ
    private int maxAmmo;
    // ���� źâ
    private int currAmmo;

    // ������ �̰ų� ������ �ʿ�
    private bool isReload;

    public bool IsReload => isReload;

    private Vector3 screen_RayPos;

    [SerializeField] ObjectPooling bulletUI_ObjectPooling;

    // �Ѿ˼� �����ִ� UI �ֻ��� �θ�
    [SerializeField] Transform ammoNumberParentTr;

    private int index;

    // ��� ����
    private bool notShoot;

    // �������Ʒ�
    [SerializeField] Transform[] aims;

    // ������ �� źâ
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

        // �ִϸ��̼�
        animator.SetTrigger("reload");

        StartCoroutine(Reload());
    }

    private void Update()
    {
        //Debug.Log(shootingType);
        //Debug.Log(shootingType);

        //firePos.rotation = mainCamera.transform.rotation * Quaternion.Euler(addPos);

        // �Ѿ��� ���ų� �ִ��Ѿ� ������ ���� ����
        if (currAmmo < maxAmmo && shooting)
        {
            if (currAmmo <= 0) notShoot = true;
            else notShoot = false;

            // �ð� ����ȭ
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

        // ��Ŭ�� ��
        if (Input.GetMouseButton(0) && shooting && Gamemanager.Instance.ShootingType != ShootingType.Run && !isReload && !notShoot)
        {
            // �ִϸ��̼�
            animator.SetBool("isShoot", true);

            StartCoroutine(Fire());
        }

        // ��Ŭ�� �� > ���
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
        // �޸� ��
        else if (Gamemanager.Instance.ShootingType == ShootingType.Walk)
        {
            Gamemanager.Instance.ShootingType = ShootingType.Walk;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(80, 0), Time.deltaTime * 5);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-80, 0), Time.deltaTime * 5);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 80), Time.deltaTime * 5);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -80), Time.deltaTime * 5);
        }
        // �� ��
        else if (Gamemanager.Instance.ShootingType == ShootingType.Run)
        {
            Gamemanager.Instance.ShootingType = ShootingType.Run;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);

            aims[0].localPosition = Vector3.Lerp(aims[0].localPosition, new Vector3(120, 0), Time.deltaTime * 5);
            aims[1].localPosition = Vector3.Lerp(aims[1].localPosition, new Vector3(-120, 0), Time.deltaTime * 5);
            aims[2].localPosition = Vector3.Lerp(aims[2].localPosition, new Vector3(0, 120), Time.deltaTime * 5);
            aims[3].localPosition = Vector3.Lerp(aims[3].localPosition, new Vector3(0, -120), Time.deltaTime * 5);
        }
        // ������ ��
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

        // ���� �����µ� "currAmmo / "������ �κ��� int�� �ؼ� ���������� ��
        ammoClip_Number.fillAmount = Mathf.Lerp(ammoClip_Number.fillAmount, currAmmo / (float)maxAmmo, Time.deltaTime * 10);
    }


    // ������
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
        // Ray�� ��ġ? ȭ�� �������� ��ġ
        screen_RayPos = Vector3.zero;

        // �浹 ����
        RaycastHit hit;

        // 1920 X 1080�� ���� ������ 2 �ϸ� 960 X 540�� �����µ� �̴� ȭ���� ����� ��Ÿ����. ���� ScreenPointToRay�� ��ũ���� ������ Ray�� �ٲ��ִ� �Լ��̴�.
        Ray screen_Aim = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 5));


        // ���̸� ��� ������ hit�� ���� �־��� �����Ÿ��� 150�̴�.
        if (Physics.Raycast(screen_Aim, out hit, 150, 1 << 6 | 1 << 7))
        {
            // ���� �� hit�� ������ ������
            screen_RayPos = hit.point;
        }
        else
        {
            // �� ���� �� Ray�� 150�����Ÿ� ������ ������
            screen_RayPos = screen_Aim.GetPoint(150);
        }

        // ���ⱸ�ϴ� �� �̴�. screen_RayPos - firePos.position(normalized�� ����ȭ�� ���ؼ� �̴� 1�� ����� ����)
        Vector3 direction = (screen_RayPos - firePos.position).normalized;

        //// ���̿� �ѱ��� �Ÿ��� �����ٸ�  ��ġ�� ī�޶�� ����
        //if (Vector3.Distance(firePos.position, screen_RayPos) < 7.5f)
        //{
        //    firePos.position = mainCamera.transform.position;
        //}
        //else
        //{
        //    firePos.localPosition = originPos_firePos;
        //}

        // �ٶ󺸰� �Ѵ� Euler�� ���� ��ġ���Ͱ� �ٸ��� ������ �۷���. �׷��Ƿ� LookRotation�� ����.
        firePos.rotation = Quaternion.LookRotation(direction);



        // �Ѿ� ����
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, Gamemanager.Instance.ShootingType, firePos, 1);

        // �Ѿ� ������ ��
        shooting = false;

        // �߻� �ѱ� ȭ�� ����
        m4MuzzleFlash.Play();

        if (index >= ammoNumberParentTr.childCount) index = 0;

        Transform child = ammoNumberParentTr.GetChild(index);
        bulletUI_ObjectPooling.Input(child.gameObject);
        index++;
        currAmmo--;

        SoundManager.Instance.Sound(SoundType.Shooting);
        StopCoroutine(Rebound());
        StartCoroutine(Rebound());

        // �� ������
        yield return shootingDelay;

        shooting = true;
    }

    private float recoilAmount = 0f; // �ݵ� �� ����
    public float RecoliAmount => recoilAmount;
    private float recoilSpeed = 0.0002f; // �ݵ� ���� �ӵ�
    private float recoilDecaySpeed = 0.0001f; // �ݵ� ���� �ӵ�
    private float maxRecoil = -0.2f; // �ִ� �ݵ� ����  

    public IEnumerator Rebound()
    {
        float targetRecoil = recoilAmount + maxRecoil; // ��ǥ �ݵ� ����
        float elapsedTime = 0f;

        // �ݵ� ����
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, targetRecoil, elapsedTime / recoilSpeed);
            yield return null;
        }

        // �ݵ� ����
        elapsedTime = 0f;
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, 0f, elapsedTime / recoilDecaySpeed);
            yield return null;
        }

        recoilAmount = 0f; // �ݵ� �� �ʱ�ȭ

        // �߰� �ݵ� �ø���: �ݵ��� ������ ������ �� 1/2��ŭ �� �÷���
        recoilAmount = maxRecoil / 2f;
        yield return new WaitForSeconds(0.1f); // �ణ�� �����̸� �ΰ�

        // ������ �ݵ��� �ٽ� ����ġ�� ���ư��� ����
        while (elapsedTime < recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            recoilAmount = Mathf.Lerp(recoilAmount, 0f, elapsedTime / recoilDecaySpeed);
            yield return null;
        }

        recoilAmount = 0f; // ���������� �ݵ� �� �ʱ�ȭ
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
