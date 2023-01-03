using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPosition : MonoBehaviour
{
    [Header("Bewegung zwischen zwei Punkten")]
    [SerializeField] float speed;
    [SerializeField] float xdistanceToCover;
    [SerializeField] float ydistanceToCover;
    [SerializeField] float zdistanceToCover;


    private void FixedUpdate()
    {
        Vector3 xMovement = transform.position;
        xMovement.x += xdistanceToCover * Mathf.Sin(Time.time * speed);
        transform.position = xMovement;

        Vector3 yMovement = transform.position;
        yMovement.y += ydistanceToCover * Mathf.Sin(Time.time * speed);
        transform.position = yMovement;

        Vector3 zMovement = transform.position;
        zMovement.z += zdistanceToCover * Mathf.Sin(Time.time * speed);
        transform.position = zMovement;
    }
}
