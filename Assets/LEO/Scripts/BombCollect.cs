using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollect : MonoBehaviour
{
    BombTimer playerBomb;

    private void Awake()
    {
        playerBomb = FindObjectOfType<BombTimer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerBomb.PickUpBomb();
            Destroy(this.gameObject);
        }
    }
}
