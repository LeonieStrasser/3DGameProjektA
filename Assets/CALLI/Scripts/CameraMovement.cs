using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void FixedUpdate()
    {
        //Camera Movement
        Vector3 moveCamTo = transform.position - transform.forward * 2.0f + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 20.0f);
    }
}
