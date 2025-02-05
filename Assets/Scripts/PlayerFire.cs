using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private static PlayerFire instance;
    public static PlayerFire Instance => instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Fire());
        }
    }

    [SerializeField] GameObject bullet;



    // น฿ป็
    private IEnumerator Fire()
    {
        yield return null;
    }
}
