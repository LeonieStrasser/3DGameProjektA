using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEffekt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(other.GetComponent<PlayerEffect>().HeldEffect == -1 && other.GetComponent<PlayerEffect>().CanEffectUp)
            {
                //Starte das PlayerEffect script
                other.GetComponent<PlayerEffect>().StartEffectUp();
            }
        }
    }
}
