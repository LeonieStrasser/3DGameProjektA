using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterUp : MonoBehaviour
{
    [SerializeField] float force;
    Rigidbody effectRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            effectRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            effectRb = null;
        }
    }

    private void FixedUpdate()
    {
        if (effectRb != null)
        {
            UpForceBoost();
        }
    }
    void UpForceBoost()
    {
        effectRb.AddForce(Vector3.up * force * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
