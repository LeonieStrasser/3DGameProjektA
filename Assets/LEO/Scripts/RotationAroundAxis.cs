using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAroundAxis : MonoBehaviour
{
    [SerializeField] float xRotationSpeed;
    [SerializeField] float yRotationSpeed;
    [SerializeField] float zRotationSpeed;



    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.right, xRotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.up, yRotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.forward, zRotationSpeed * Time.deltaTime);
    }
}
