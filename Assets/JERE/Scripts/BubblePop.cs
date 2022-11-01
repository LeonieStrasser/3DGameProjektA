using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{ 
    [SerializeField] GameObject VFXPop;
    private void OnTriggerEnter(Collider other)
    {

        Instantiate(VFXPop, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }

}
