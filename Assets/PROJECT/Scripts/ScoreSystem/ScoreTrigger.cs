using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Anzahl an Punkten die bei normalem durchfliegen auf den Score gerechnet werden")] int scorePoints = 5;
    [SerializeField] [Tooltip("Anzahl an Punkten die bei durchfliegen auf die Resource gerechnet werden")] int resourcePoints = 20;
    [SerializeField] float cooldownTime;

    int activeScorepoints;
    WhingMovement01 myPlayer;
    BonusManager myBonusManager;

    bool cooldownOn = false;


    private void Start()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();
        myBonusManager = FindObjectOfType<BonusManager>();

        activeScorepoints = scorePoints;
        myBonusManager.OnTimeEffectStart += MultiplyScorePoints;
        myBonusManager.OnTimeEffectEnd += MultiplyScorePoints;

    }

    private void OnTriggerEnter(Collider other)
    {
        FuelTrigger(other);
    }

    public virtual void FuelTrigger(Collider other)
    {
        if (!cooldownOn)
        {
            if (other.tag == "Player" && ScoreSystem.Instance != null)
            {
                cooldownOn = true;
                ScoreSystem.Instance.AddScoreImediatly(activeScorepoints);
                StartCoroutine(CooldownTimer());
            }
            else
                Debug.LogWarning("Es fehlt ein ScoreSystem-Script in der Szene ihr Volldeppen!", this.gameObject);

            myPlayer.AddResourcePoints(resourcePoints);

        }

    }

    private void MultiplyScorePoints(float multiplyer) // WIrd angesprochen wenn im BonusManager der EffectTimer ausgelöst wird
    {
        activeScorepoints = Mathf.RoundToInt(scorePoints * multiplyer);
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownOn = false;
        OnCooldownEnd();
    }

    public virtual void OnCooldownEnd()
    {

    }
}
