using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float mousePosX;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        mousePosX = Input.GetAxis("Mouse Y");

        transform.Rotate(-mousePosX, 0, 0);
    }
}
