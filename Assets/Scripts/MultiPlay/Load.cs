using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Load : MonoBehaviour
{
    private static Load instance;
    public static Load Instance => instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public IEnumerator Create()
    {
        yield return new WaitForSeconds(0.5f);

        PhotonNetwork.Instantiate("MultiPlay/Player_MultiPlay", new Vector3(0, 0, -78.73f), Quaternion.Euler(0, 0, 0), 0);

    }


}
