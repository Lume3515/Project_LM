using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

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

        // ��Ŭ�� ��
        if (Input.GetMouseButton(0) && shooting)
        {

            // �ִϸ��̼�
            animator.SetBool("isShoot", true);

            StartCoroutine(Fire());
        }

        // ��Ŭ�� �� > ���
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


        // �� ������
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
