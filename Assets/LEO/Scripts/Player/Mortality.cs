using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortality : MonoBehaviour
{
 
    [SerializeField] GameObject dieVFX;
    LevelManager myManager;
    [SerializeField] float distanceToCore;
    [SerializeField] float detectionRadius;
    [SerializeField] LayerMask detectionLayers;

    private void Start() 
    {
        myManager = FindObjectOfType<LevelManager>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Collision detected?");
        Collider[] colliderHits = Physics.OverlapSphere(this.transform.position + transform.forward * distanceToCore, detectionRadius,detectionLayers);
        if(colliderHits.Length > 0)
        {
            Instantiate(dieVFX, transform.position, Quaternion.identity);
            myManager.GameLoose();
            this.gameObject.SetActive(false);
        }
    }   

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawSphere(this.transform.position + transform.forward * distanceToCore, detectionRadius);
    }
    
        
    
}
