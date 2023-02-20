using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMovement : MonoBehaviour
{
    Rigidbody myRigidbody;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        myRigidbody.AddForce(transform.forward * speed * 10, ForceMode.Force);
    }
}
