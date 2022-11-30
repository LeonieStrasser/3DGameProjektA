using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Anzahl an Punkten die bei normalem durchfliegen auf den Score gerechnet werden")] int ScorePoints = 5;
    [SerializeField] [Tooltip("Anzahl an Punkten die bei durchfliegen auf die Resource gerechnet werden")] int resourcePoints = 20;
    [SerializeField] float cooldownTime;

    WhingMovement01 myPlayer;

    bool cooldownOn = false;

    private void Start()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cooldownOn)
        {
            if (other.tag == "Player" && ScoreSystem.Instance != null)
            {
                cooldownOn = true;
                ScoreSystem.Instance.AddScore(ScorePoints);
                StartCoroutine(CooldownTimer());
            }
            else
                Debug.LogWarning("Es fehlt ein ScoreSystem-Script in der Szene ihr Volldeppen!", this.gameObject);

            myPlayer.AddResourcePoints(resourcePoints);

        }
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownOn = false;
    }
}
