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
        Collider[] colliderHits = Physics.OverlapSphere(this.transform.position + transform.forward * distanceToCore, detectionRadius, detectionLayers);
        if (colliderHits.Length > 0)
        {
            Die();
        }

    }

    private void Die()
    {
        if (myManager.CurrentGameState == LevelManager.gameState.running) // sorgt dafür dass Loose nur einmal aufgerufen wird
        {
            Instantiate(dieVFX, transform.position, Quaternion.identity);
            myManager.GameLoose();
            AudioManager.instance.PlayerCrash(); // <- Player Crash SFX
            AudioManager.instance.PauseRaceInProgress(); // <-- if Player cashes in Race, Race Music Fades Out
            // KILL ALL OTHER SFXs
            AudioManager.instance.PointsUpStop();
            AudioManager.instance.EdgeSparkStop();
        }
        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(this.transform.position + transform.forward * distanceToCore, detectionRadius);
    }



}
