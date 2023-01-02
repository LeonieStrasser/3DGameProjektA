using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedLifetime : MonoBehaviour
{
    [SerializeField] float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    
}
