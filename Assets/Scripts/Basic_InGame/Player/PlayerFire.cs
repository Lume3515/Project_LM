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

    private HorizontalLayoutGroup horizontalLayout_ammoNumberParent;

    private int index;

    // ��� ����
    private bool notShoot;
    private void Start()
    {

        if (instance == null) instance = this;

        shootingDelay = new WaitForSeconds(0.1f);

        maxTime = 0.7f;

        mainCamera = Camera.main;

        fireSpeed = 500;

        mainCamera.fieldOfView = 60;

        maxAmmo = 30;

        horizontalLayout_ammoNumberParent = ammoNumberParentTr.GetComponent<HorizontalLayoutGroup>();

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

            if (Input.GetKeyDown(KeyCode.R) && !isReload)
            {
                StartCoroutine(Reload());
                animator.SetTrigger("reload");
            }

        }
        

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
            ShoulderAndAim = true;
            //Debug.Log("1");
        }
        // �޸��� �� �̳� ����� ���� �� �� ���·�        
        else
        {
            ShoulderAndAim = false;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, Time.deltaTime * 13);
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

            yield return new WaitForSeconds(Time.deltaTime * 0.000001f);
        }

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
        if (Physics.Raycast(screen_Aim, out hit, 150))
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

        // �ٶ󺸰� �Ѵ� Euler�� ���� ��ġ���Ͱ� �ٸ��� ������ �۷���. �׷��Ƿ� LookRotation�� ����.
        firePos.rotation = Quaternion.LookRotation(direction);


        // �Ѿ� ����
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, Gamemanager.Instance.ShootingType, firePos, null);

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
