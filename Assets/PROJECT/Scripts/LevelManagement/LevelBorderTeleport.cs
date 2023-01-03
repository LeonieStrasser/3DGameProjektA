using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBorderTeleport : MonoBehaviour
{
    [SerializeField] float levelRadius;
    [SerializeField] float respawnInsideOffset;


    LevelManager myManager;

    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        float distanceToCenter = Vector3.Distance(this.transform.position, myManager.PlayerPosition);
        if (distanceToCenter > levelRadius)
        {
            Debug.Log("Du hast das Level verlassen!");


            // Packt Player und Camera in ein TransportObjekt
            GameObject transportBox = new GameObject("Transportbox");
            transportBox.transform.position = myManager.PlayerPosition;
            myManager.myPlayer.transform.SetParent(transportBox.transform);
            Camera.main.transform.SetParent(transportBox.transform);

            // Rechnet gegenüberliegenden Punkt aus
            Vector3 directionToCenter = (this.transform.position - myManager.PlayerPosition).normalized;
            Vector3 respawnPoint = this.transform.position + directionToCenter * (levelRadius - respawnInsideOffset);

            // transportiert den Transporter zum neuen Ort
            transportBox.transform.position = respawnPoint;

            // Transportbox auspacken
            myManager.myPlayer.transform.SetParent(null);
            Camera.main.transform.SetParent(null);

            Destroy(transportBox);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(this.transform.position, levelRadius);

    }
    private void OnDrawGizmos()
    {
    }
}
