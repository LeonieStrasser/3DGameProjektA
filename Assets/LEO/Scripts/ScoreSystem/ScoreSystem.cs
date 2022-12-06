using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private static ScoreSystem instance;
    public static ScoreSystem Instance
    {
        get
        {
            return instance;
        }
    }


    [SerializeField] private float currentScore;
    public float CurrentScore           // Man kann ihn von auﬂen nur getten, Setten nur aus diesem Script
    {
        private set
        {
            currentScore = value;

            OnXpChange?.Invoke(Mathf.RoundToInt(value));
        }

        get
        {
            return currentScore;
        }
    }
    public event Action<int> OnXpChange;

    [SerializeField] int twirlMultiplikator = 2;


    WhingMovement01 myPlayer;


    private void Awake()
    {
        // Levelmanager setzen, Wenn schon ein LevelManager existiert mach ihn kaputt
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    

    private void Start()
    {
        CurrentScore = 0;
    }

    public void AddScore(float newScorePoints)
    {
        if(myPlayer.Twirl)
        {
            newScorePoints *= twirlMultiplikator;
        }
        CurrentScore += newScorePoints;
    }

}
