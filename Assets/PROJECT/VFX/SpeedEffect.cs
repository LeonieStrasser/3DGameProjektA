using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField] VisualEffect speedlines;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float speedBorder1 = 100;
    // Update is called once per frame
    void Update()
    {
        Debug.Log("speed = " + playerRigidbody.velocity.magnitude);
        speedlines.SetFloat("Speed", playerRigidbody.velocity.magnitude);
        
        if(playerRigidbody.velocity.magnitude > speedBorder1)
        {

        }
    }
}
