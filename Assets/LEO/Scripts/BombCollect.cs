using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollect : MonoBehaviour
{
    BombTimer playerBomb;
    CollectableBombTimer collectTimer;

    private void Awake()
    {
        playerBomb = FindObjectOfType<BombTimer>();
        collectTimer = FindObjectOfType<CollectableBombTimer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collectTimer.ResetTimer();
            playerBomb.PickUpBomb();
            Destroy(this.gameObject);
        }
    }
}
