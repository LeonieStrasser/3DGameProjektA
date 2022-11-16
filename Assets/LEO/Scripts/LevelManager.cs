using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Timer")]
    [SerializeField] float startTime;

    [Space(20)]
    [Header("Races")]
    [SerializeField] GameObject startZonePrefab;
    [SerializeField] GameObject goalZonePrefab;

    [SerializeField] float raceMaxTime;

    [Space(5)]
    [SerializeField] float maxBonusTime;

    // LEVEL TIMER
    private float levelTimer;
    public float LevelTimer
    {
        get => levelTimer;
        set
        {
            levelTimer = value;
            levelProgress = levelTimer / startTime; // Direkt den Level Progress ausrechnen
        }
    }

    private float levelProgress;
    public float LevelProgress { get => levelProgress;  }



    // GAMESTATES
    enum gameStates
    {
       
    }
    enum raceState
    {
        noRace,
        raceIsRunning
    }
    raceState thisRace;

    public event Action OnGameLoose;


    // CURRENT RACE
    [Header("debug")]
    [SerializeField] GameObject currentGoal;  // SOLLTE SPÄTER NICHT MEHR IM INSPECTOR ZU SEHEN SEIN _ NUR ZUM DBUGGEN
    [SerializeField] GameObject startZone;    // SOLLTE SPÄTER NICHT MEHR IM INSPECTOR ZU SEHEN SEIN _ NUR ZUM DBUGGEN

    private float raceTimer;
    private float RaceTimer
    {
        set
        {
            raceTimer = value;
            raceTimeProgress = raceTimer/raceMaxTime;
        }
    }
    private float raceTimeProgress; // Zahl zwischen 0 und 1 - Für den Balken im UI
    public float RaceTimeProgress { get => raceTimeProgress; }

    

    private void Start()
    {
        LevelTimer = startTime;
        thisRace = raceState.noRace;
    }

    private void Update()
    {
        RunWorldTimer();

        if(thisRace == raceState.raceIsRunning)
        {
            RunRaceTimer();
        }
    }
    private void RunWorldTimer() // World-Timer läuft ab
    {
        LevelTimer = Mathf.Clamp(levelTimer - Time.deltaTime, 0, startTime);

        if (levelTimer == 0)
            GameLoose();
    }

    private void GameLoose()
    {
        Debug.Log("GAME LOOSE!");
        OnGameLoose?.Invoke();
    }

    private void SpawnNextRace()
    {
        // STart und Ziel fürs nächste Rennen müssen gespawnt werden

    }

    private void RunRaceTimer()
    {
        RaceTimer = Mathf.Clamp(raceTimer - Time.deltaTime, 0, raceMaxTime); // Timer runterzählen
    }

    private void AddBonusTimeToWorldTimer()
    {
        LevelTimer += RaceTimeProgress * maxBonusTime;
        raceTimeProgress = 0;
    }




    public void StartRace()
    {
        thisRace = raceState.raceIsRunning;
        RaceTimer = raceMaxTime;

        // Hier sollte das STart Objekt getötet werden
    }

    public void FinishRace()
    {
        thisRace = raceState.noRace;
        AddBonusTimeToWorldTimer();



        // Hier sollte das Zielobjekt getötet werden und ein neues Race gespawnt

        // Hier sollte die Bonuszeit auf den Timer gerechnet wewrden
    }


}
