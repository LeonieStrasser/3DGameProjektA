using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Anzahl an Punkten die bei normalem durchfliegen auf den Score gerechnet werden")] int ScorePoints = 5;
    [SerializeField] [Tooltip("Anzahl an Punkten die bei durchfliegen auf die Resource gerechnet werden")] int resourcePoints = 20;

    WhingMovement01 myPlayer;

    private void Start()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.AddScore(ScorePoints);
        }
        else
            Debug.LogWarning("Es fehlt ein ScoreSystem-Script in der Szene ihr Volldeppen!");

        myPlayer.AddResourcePoints(resourcePoints);
    }
}
