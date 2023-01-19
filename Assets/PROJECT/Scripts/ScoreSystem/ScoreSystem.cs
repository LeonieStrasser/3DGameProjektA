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
    public float CurrentScore           // Man kann ihn von außen nur getten, Setten nur aus diesem Script
    {
        private set
        {
            currentScore = value;

            OnXpChange?.Invoke();
        }

        get
        {
            return currentScore;
        }
    }



    [SerializeField] int twirlMultiplikator = 2;

    [Space(20)]
    [Header("Score States")]
    [SerializeField] ComboState[] allComboStates;

    private ComboState currentComboState;
    public ComboState CurrentComboState
    {
        set
        {
            currentComboState = value;
            OnComboStateChange?.Invoke(currentComboState.scoreColor);
        }
    }

    private float comboTimer;
    private float comboScore;
    public float ComboScore { get { return comboScore; } private set { comboScore = value; } }
    private int comboStateIndex;
    [SerializeField] private bool comboIsActive;


    private float contactScore;
    public float ContactScore { get { return contactScore; } private set { contactScore = value; } }








    [HideInInspector] public int highscore;
    [HideInInspector] public int lastListScore;

    WhingMovement01 myPlayer;
    DistanceTracker disTracker;

    #region events
    public event Action OnXpChange;
    public event Action<Color> OnComboStateChange;

    #endregion

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
        disTracker = myPlayer.GetComponent<DistanceTracker>();


        disTracker.OnContactBreak += AddContactScore;
    }



    private void Start()
    {
        CurrentScore = 0;
        ScoreList loadData = SaveSystem.LoadScore();
        if (loadData != null)
        {
            highscore = loadData.scoreDataList[0].score;
            lastListScore = loadData.scoreDataList[loadData.scoreDataList.Count - 1].score; // Score des letzten auf der Liste
        }


        //ComboStates
        comboTimer = 0;
        comboScore = 0;
        comboStateIndex = 0;
        CurrentComboState = allComboStates[comboStateIndex];



        Debug.Log("Last Highscore: " + highscore.ToString());
    }


    private void Update()
    {//---------------------------ACHTUNG! MUSS NOCH PAUSE UNABHÄNGIG WERDEN!!!
        RunComboEffect();
    }

    private void RunComboEffect()
    {
        if (comboTimer == 0)
        {
            comboIsActive = false;
            comboScore = 0;
            comboStateIndex = 0;
            CurrentComboState = allComboStates[comboStateIndex];
        }

        //State Levelup
        if (comboScore >= currentComboState.neededPointsToNextState)
        {
            // Wenn noch ein State übrig ist, wird zum nächst höheren State gewechselt
            if (comboStateIndex < allComboStates.Length - 1)
            {
                comboStateIndex++;

                CurrentComboState = allComboStates[comboStateIndex];
                // Combo timer wieder voll auf neuen max

                comboTimer = currentComboState.maxTimerTime;
            }

        }

        // Timer runterzählen
        comboTimer = Mathf.Clamp(comboTimer - Time.deltaTime, 0, float.MaxValue);


        Debug.Log("Combotimer: " + comboTimer + ", ComboScore: " + comboScore);

    }

    private void AddContactScore() // wird durch ein Event im Distance Tracker getriggert wenn der Spark Kontakt abbricht
    {
        contactScore = 0;
    }

    public void AddScore(float newScorePoints)
    {
        // Combo update
        comboIsActive = true;
        comboTimer = currentComboState.maxTimerTime;

        newScorePoints *= currentComboState.stateMultiplyer;

        // Twirl Multiplyer
        if (myPlayer.Twirl)
        {
            newScorePoints *= twirlMultiplikator;
        }
        CurrentScore += newScorePoints;
        comboScore += newScorePoints;
        contactScore += newScorePoints;

    }



}
