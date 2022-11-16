using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Anzahl an Punkten die bei normalem durchfliegen auf den Score gerechnet werden")] int points = 5;
    [SerializeField] [Tooltip("Anzahl an Punkten die bei durchfliegen auf die Resource gerechnet werden")] int resourcePoints = 20;

    WhingMovement01 myPlayer;

    private void Start()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();

        // DEBUG!!!!!!!! ACHTUNG!!!!!!!!!!!!!!!!!
        resourcePoints = 5;
        Debug.LogError("LEO HAT HIER WAS PROVISORISCH HARD GECODET!!!! Bitte dran denken das zu löschen, wenn entschieden ist wie Score Objekte funktionieren!");
        //DEBUG!!! ACHTUNG!!!!!!!
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.AddScore(points);
        }
        else
            Debug.LogWarning("Es fehlt ein ScoreSystem-Script in der Szene ihr Volldeppen!");

        myPlayer.AddResourcePoints(resourcePoints);
    }
}
