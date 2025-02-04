using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Info", menuName = "Scriptable Object/Gun Info", order = int.MaxValue)]
public class GunInfo : ScriptableObject
{

    [SerializeField] float fireSpeed;

    // ������ ������
    [SerializeField] float headDamage;
    [SerializeField] float bodyDamage;
    [SerializeField] float legAndArmDamage;

    [SerializeField] int maxAmmo;
    [SerializeField] GameObject gun;

    // ���� ������ ���� ��ġ(idle)
    [SerializeField] Vector3 leftArmPos;
    [SerializeField] Vector3 rightArmPos;

    [SerializeField] Quaternion leftArmRot;
    [SerializeField] Quaternion rightArmRot;



    public float FireSpeed => fireSpeed;
    public float HeadDamage => headDamage;
    public float BodyDamage => bodyDamage;
    public float LegAndArmDamage => legAndArmDamage;
    public int MaxAmmo => maxAmmo;
    public GameObject Gun => gun;
    public Vector3 LeftArmPos => leftArmPos;
    public Vector3 RightArmPos => rightArmPos;
    public Quaternion LeftArmRot => leftArmRot;
    public Quaternion RightArmRot => rightArmRot;


}

