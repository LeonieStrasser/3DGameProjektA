using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField] VisualEffect speedlines;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] WhingMovement01 myPlayer;
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("speed = " + playerRigidbody.velocity.magnitude);
        speedlines.SetFloat("Speed", playerRigidbody.velocity.magnitude);

        int playerState = 0;
        
        if(!myPlayer.BoostActive && !myPlayer.SlowMoActive)
        {
            playerState = 0;
        }
        else if(myPlayer.SlowMoActive && !myPlayer.BoostActive)
        {
            playerState = 1;
        }
        else if(myPlayer.BoostActive && !myPlayer.SlowMoActive)
        {
            playerState = 2;
        }
        else if(myPlayer.BoostActive && myPlayer.SlowMoActive)
        {
            playerState = 3;
        }

        speedlines.SetInt("PlayerState", playerState);
    }

}
