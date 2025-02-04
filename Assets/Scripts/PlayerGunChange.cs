using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunChange : MonoBehaviour
{
    // 0 : Hk416, 1 : 
    [SerializeField] GunInfo[] infos;

    [SerializeField] Transform parent;

    private void Start()
    {
        SpawnGun(0, infos[0]);
    }

    // 몇 번 인벤토리에 넣을건지 또 정보를 넣어야함
    private void SpawnGun(int index, GunInfo info)
    {
        if (index == 0)
        {
            Instantiate(info.Gun, parent);
            FirstSpace(info);
        }
        if (index == 1)
        {
            Instantiate(info.Gun, parent);
            SecondSpace(info);
        }
        if (index == 2)
        {
            Instantiate(info.Gun, parent);
            PistolSpace(info);
        }
    }

    // 0 : left, 1 : right
    [SerializeField] Transform[] arms;

    #region// 인벤토리

    private GunInfo firstInfo;
    private GameObject firstSpace_Obj;
    public void FirstSpace(GunInfo info)
    {
        firstInfo = info;

        // 팔 위치 수정
        arms[0].localPosition = firstInfo.LeftArmPos;
        arms[1].localPosition = firstInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = firstInfo.LeftArmRot;
        arms[1].localRotation = firstInfo.RightArmRot;
    }

    private GunInfo secondInfo;
    private GameObject secondSpace_Obj;
    public void SecondSpace(GunInfo info)
    {
        secondInfo = info;

        // 팔 위치 수정
        arms[0].localPosition = secondInfo.LeftArmPos;
        arms[1].localPosition = secondInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = secondInfo.LeftArmRot;
        arms[1].localRotation = secondInfo.RightArmRot;
    }

    private GunInfo pistolInfo;
    private GameObject pistolSpace_Obj;
    public void PistolSpace(GunInfo info)
    {
        pistolInfo = info;

        // 팔 위치 수정
        arms[0].localPosition = pistolInfo.LeftArmPos;
        arms[1].localPosition = pistolInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = pistolInfo.LeftArmRot;
        arms[1].localRotation = pistolInfo.RightArmRot;
    }

    #endregion
}