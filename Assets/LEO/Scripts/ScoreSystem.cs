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
            scoreText.text = currentScore.ToString();
        }

        get
        {
            return currentScore;
        }
    }



    // UI
    [Header("HUD")]

    [SerializeField] TextMeshProUGUI scoreText;


    private void Awake()
    {
        // Levelmanager setzen, Wenn schon ein LevelManager existiert mach ihn kaputt
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        CurrentScore = 0;
    }

    public void AddScore(int newScorePoints)
    {
        CurrentScore += newScorePoints;
    }

}
