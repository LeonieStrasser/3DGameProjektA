using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField][Tooltip("Anzahl an Punkten die bei normalem durchfliegen auf den Score gerechnet werden")] int points;

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.AddScore(points);
        }
    }
}
