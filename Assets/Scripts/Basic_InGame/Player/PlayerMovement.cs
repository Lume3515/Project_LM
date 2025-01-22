using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    private Animator playerAnimator;

    private PlayerFire playerFire;


    private void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerFire = GetComponent<PlayerFire>();
    }

    private void Start()
    {
        moveSpeed = 3.8f;

        right = cameraTr.transform.right;
        forward = cameraTr.transform.forward;
    }

    // �Է�
    private void Update()
    {
        if (Gamemanager.Instance.GameOver)
        {
            playerRb.velocity = Vector3.zero;
            return;
        }
        else
        {

            Movement();
        }

        if (!roll && Input.GetKeyDown(KeyCode.Space) && rollCoolTIme && !PlayerFire.Instance.IsReload && PlayerFire.Instance.Shooting_Bool)
        {
           if(firstClick_Bool) firstClick_Bool = true;
            StartCoroutine(AddTime());
        }
        if (!firstClick_Bool)
        {
            if(firstClick >= 1)
            {
                firstClick = 0;
                firstClick_Bool = true;
            }
        }

    }

    // ����
    private void FixedUpdate()
    {
        Rotation();
    }

    #region// �÷��̾� �̵�

    // ������ �ӵ�
    private float moveSpeed;

    // Ű ��(Input)
    private float moveX; // �¿�
    private float moveZ; // ����

    private Rigidbody playerRb;

    // ������ ����
    private Vector3 moveDirection;

    // true : �ɾҴ�
    private bool sitDown;

    // ī�޶� Ʈ���� ��
    [SerializeField] Transform cameraTr;

    // ȸ�� ����(?) ��� ��������
    private Vector3 right;
    private Vector3 forward;

    // ������
    private void Movement()
    {
        // �ﰢ���� ���� �ʿ�

        if ((sitDown && PlayerFire.Instance.IsReload) || roll)
        {
            moveX = 0;
            moveZ = 0;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
        }



        // �� ������ ��
        if (playerRb.velocity.magnitude == 0 && !sitDown)
        {
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

            //Debug.Log($"1��° :{playerFire.ShootingType != ShootingType.Aim} / 2���� : {playerFire.ShootingType != ShootingType.Shoulder} Ÿ�� : {playerFire.ShootingType}");

            //Debug.Log($"ȣ��� {playerFire.ShootingType}");
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        // �޸��� ����
        else if (Input.GetKey(KeyCode.LeftShift) && !sitDown && !PlayerFire.Instance.IsReload && Gamemanager.Instance.ShootingType != ShootingType.Shoulder && !roll)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", true);
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Run;

            moveSpeed = 5f;
        }
        else if (!sitDown)
        {
            playerAnimator.SetBool("isWalk", true);
            playerAnimator.SetBool("isRun", false);

            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Walk;

            moveSpeed = 2.8f;
        }


        // �ɱ� ����
        if (Input.GetKeyDown(KeyCode.C) && !sitDown && !PlayerFire.Instance.IsReload && !roll)
        {
            sitDown = true;

            Gamemanager.Instance.ShootingType = ShootingType.Sit;

            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
            playerAnimator.SetBool("isSitDown", true);

            moveSpeed = 1.5f;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && sitDown && !PlayerFire.Instance.IsReload))
        {
            sitDown = false;
            if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Stand;

            playerAnimator.SetBool("isSitDown", false);
        }
        if (sitDown)
        {
            // �ɾҴµ� �� ������ ��
            if (playerRb.velocity.magnitude == 0)
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 0f);

                if (!playerFire.ShoulderAndAim) Gamemanager.Instance.ShootingType = ShootingType.Sit;

            }
            else
            {
                playerAnimator.SetFloat("SitDown_Multiplier", 1f);
                {
                    Gamemanager.Instance.ShootingType = ShootingType.SitWalk;
                }
            }
        }

        // playerRb.velocity.y�� ���� �� ������ playerRb.velocity.y�� 0�� �Ҵ��ϸ� moveSpeed�� �������� �� ������ ���� 0�� �ż� > �ȵǴ°� ����!!(�� ������)
        moveDirection = cameraTr.TransformDirection(moveX, 0, moveZ) * moveSpeed + new Vector3(0, playerRb.velocity.y, 0); // ���� ����

        if(!roll) playerRb.velocity = moveDirection;
    }

    private void Rotation()
    {
        if (Gamemanager.Instance.ShootingType == ShootingType.Run)
        {

            // ��
            if (Input.GetKey(KeyCode.W))
            {

                moveDirection += forward;
            }
            // ��
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= forward;
            }
            // ����
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= right;
            }
            // ������
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += right;
            }

            transform.rotation = Quaternion.LookRotation(moveDirection);

        }
        else
        {
            transform.rotation = cameraTr.rotation;
        }


    }
    #endregion

    #region// ������

    // ������ ��?
    private bool roll;
    private bool rollCoolTIme = true; // ��Ÿ��

    // �Է� �ð� ���
    private float firstClick;
    private bool firstClick_Bool = true;       

    // ������
    private IEnumerator Roll()
    {
        roll = true;
        rollCoolTIme = false;

        playerAnimator.SetTrigger("roll");
        transform.rotation = cameraTr.rotation;
        playerRb.velocity = transform.forward * 4.5f;

        yield return new WaitForSeconds(2.2f);

        roll = false;

        firstClick = 0;
        firstClick_Bool = true;
        StopCoroutine(AddTime());

        yield return new WaitForSeconds(5f);

        rollCoolTIme = true;
    }

    // �ð� ���ϱ�
    private IEnumerator AddTime()
    {
        if (firstClick_Bool)
        {
            firstClick_Bool = false;
            while (firstClick < 1)
            {
                firstClick += Time.deltaTime;
                
                yield return null;
            }

            yield break;
        }
        else
        {
            if(firstClick < 1)
            {
                StartCoroutine(Roll());
            }
        }
    }

    #endregion

}