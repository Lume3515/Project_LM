using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunChange : MonoBehaviour
{
    // 0 : Hk416, 1 : 9mm Pistol, 2 : 
    [SerializeField] GunInfo[] infos;

    [SerializeField] Transform parent;

    private void Start()
    {
        SpawnGun(2, infos[1]);
        SpawnGun(0, infos[0]);
    }

    private void Update()
    {
        ChangeGun();
    }

    private void ChangeGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) FirstSpace(null, false);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SecondSpace(null, false);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) PistolSpace(null, false);
    }

    // 몇 번 인벤토리에 넣을건지 또 정보를 넣어야함
    private void SpawnGun(int index, GunInfo info)
    {
        if (index == 0)
        {
            firstSpace_Obj = Instantiate(info.Gun, parent);
            FirstSpace(info, true);
        }
        else if (index == 1)
        {
            secondSpace_Obj = Instantiate(info.Gun, parent);
            SecondSpace(info, true);
        }
        else if (index == 2)
        {
            pistolSpace_Obj = Instantiate(info.Gun, parent);
            PistolSpace(info, true);
        }
    }

    // 0 : left, 1 : right
    [SerializeField] Transform[] arms;

    #region// 인벤토리

    private GunInfo firstInfo;
    private GameObject firstSpace_Obj;
    public void FirstSpace(GunInfo info, bool reference)
    {
        if (reference) firstInfo = info;

        if (firstSpace_Obj == null) return;

        firstSpace_Obj.SetActive(true);
        if (secondSpace_Obj != null) secondSpace_Obj.SetActive(false);
        pistolSpace_Obj.SetActive(false);

        // 팔 위치 수정
        arms[0].localPosition = firstInfo.LeftArmPos;
        arms[1].localPosition = firstInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = firstInfo.LeftArmRot;
        arms[1].localRotation = firstInfo.RightArmRot;
    }

    private GunInfo secondInfo;
    private GameObject secondSpace_Obj;
    public void SecondSpace(GunInfo info, bool reference)
    {
        if (reference) secondInfo = info;

        if (secondSpace_Obj == null) return;
        secondSpace_Obj.SetActive(true);
        if (firstSpace_Obj != null) firstSpace_Obj.SetActive(false);
        pistolSpace_Obj.SetActive(false);

        // 팔 위치 수정
        arms[0].localPosition = secondInfo.LeftArmPos;
        arms[1].localPosition = secondInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = secondInfo.LeftArmRot;
        arms[1].localRotation = secondInfo.RightArmRot;
    }

    private GunInfo pistolInfo;
    private GameObject pistolSpace_Obj;
    public void PistolSpace(GunInfo info, bool reference)
    {
        if (reference) pistolInfo = info;

        pistolSpace_Obj.SetActive(true);
        if (firstSpace_Obj != null) firstSpace_Obj.SetActive(false);
        else if (secondSpace_Obj != null) secondSpace_Obj.SetActive(false);

        // 팔 위치 수정
        arms[0].localPosition = pistolInfo.LeftArmPos;
        arms[1].localPosition = pistolInfo.RightArmPos;

        // 팔 회전
        arms[0].localRotation = pistolInfo.LeftArmRot;
        arms[1].localRotation = pistolInfo.RightArmRot;
    }

    #endregion
}