using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] string myTag = "Destructable";
    [SerializeField] GameObject vfxEffect;

    WhingMovement01 myPlayer;

    Collider myCollider;
    private void Awake()
    {
        transform.tag = myTag;
        if(transform.tag != myTag)
        {
            Debug.LogError("The Tag: " + myTag + " doesn´t exist!", gameObject);
        }

        myCollider = GetComponent<Collider>();
        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && myPlayer.Twirl)
        {
            DestructObject();
        }
    }

    public void DestructObject()
    {
        Instantiate(vfxEffect, this.transform);
        myCollider.enabled = false;
        Destroy(this.gameObject);
    }
}
