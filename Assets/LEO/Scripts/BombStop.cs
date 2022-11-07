using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStop : MonoBehaviour
{
    BombTimer playerBomb;

    private void Awake()
    {
        playerBomb = FindObjectOfType<BombTimer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerBomb.StopBomb();
        }
    }
}
