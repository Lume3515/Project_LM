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

// �߻� Ÿ��
public enum ShootingType
{
    Aim, // ����
    Shoulder, // ����  
    Run, // �ٱ�
    Walk, // �ȱ�
    Sit, // �ɱ�
    Stand, // ���ֱ�
    Obscuration, // ����
    SitWalk    // �ɾƼ� �ȱ�
}

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

    // Ÿ��
    private ShootingType shootingType;
    public ShootingType ShootingType { get { return shootingType; } set { shootingType = value; } }

    // �������̳�, ������
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

        targetPosition = Vector3.zero;

        RaycastHit hit;

        // ��ũ�� �߾ӿ��� Ray�� �߻� (UI�� �߾ӿ��� �߻�)
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 10f));


        // Raycast�� �浹 ���� Ȯ��
        if (Physics.Raycast(ray, out hit, 300))
        {
            targetPosition = hit.point; 
        }
        else
        {
            // ���� �浹���� ������ ������ �� ������ ��ǥ�� ����
            targetPosition = ray.GetPoint(300);  
        }

        // ��ǥ ��ġ�� ���� ���� ���� ���
        Vector3 direction = (targetPosition - firePos.position).normalized;

        // firePos�� ��ǥ �������� ȸ��
        firePos.rotation = Quaternion.LookRotation(direction);

        // �Ѿ� ����
        bulletObj = objectPooling.OutPut();
        bulletObj.GetComponent<Bullet>().Setting(fireSpeed, shootingType, firePos);

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
        Gizmos.DrawRay(firePos.position, firePos.forward * 170);

        //Gizmos.color = Color.red;

        //Gizmos.DrawRay(firePos.position, firePos.forward * 150);

        //Gizmos.DrawCube(screenPos, new Vector3(1, 1, 1));

        //    Gizmos.color = Color.red;

        //    Gizmos.DrawRay(cameraTr.position, cameraTr.forward * 150);
    }

}
