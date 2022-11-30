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
    public float LevelProgress { get => levelProgress; }



    // GAMESTATES

    enum raceState
    {
        noRace,
        raceIsRunning
    }
    raceState thisRace;

    public event Action OnGameLoose;


    // CURRENT RACE
    [Header("debug")]
    private GameObject currentGoal;
    private GameObject currentStartZone;

    private float raceTimer;
    private float RaceTimer
    {
        set
        {
            raceTimer = value;
            raceTimeProgress = raceTimer / raceMaxTime;
            currentBonusTime = RaceTimeProgress * maxBonusTime;
            currentBonusTimeInWorldTimeProgress = (currentBonusTime / startTime) + LevelProgress;
        }
    }
    private float raceTimeProgress; // Zahl zwischen 0 und 1 - Für den Balken im UI
    public float RaceTimeProgress { get => raceTimeProgress; }

    private float currentBonusTime;
    public float CurrentBonusTime { get => currentBonusTime; }

    private float currentBonusTimeInWorldTimeProgress;
    public float CurrentBonusTimeInWorldTimeProgress { get => currentBonusTimeInWorldTimeProgress; }



    [SerializeField] RaceData[] allRaces;
    private int raceNumber = 0;

    [System.Serializable]
    public struct RaceData
    {
        [SerializeField] public GameObject startZone;
        [SerializeField] public GameObject goal;
        [SerializeField] public float raceMaxTime;
    }


    private void Start()
    {
        LevelTimer = startTime;
        thisRace = raceState.noRace;


        // Erstes Rennen wird gespawnt und zugewiesen
        raceNumber = -1;
        SpawnNextRace();

       
    }

    private void Update()
    {
        RunWorldTimer();

        if (thisRace == raceState.raceIsRunning)
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
        raceNumber++; // Racenummer wird erstmal hochgerechnet - heißt die Rennen laufen der Reihe nach ab. Später sollten wir hier ein zufälliges Ziehen ohne zurücklegen reinbauen.
        if (raceNumber > allRaces.Length - 1) { raceNumber = 1; } // Tutorial Strecke wird nicht wiederholt

        currentStartZone = allRaces[raceNumber].startZone;
        currentGoal = allRaces[raceNumber].goal;
        raceMaxTime = allRaces[raceNumber].raceMaxTime;

        currentStartZone.SetActive(true);
    }

    private void RunRaceTimer()
    {
        RaceTimer = Mathf.Clamp(raceTimer - Time.deltaTime, 0, raceMaxTime); // Timer runterzählen
    }

    private void AddBonusTimeToWorldTimer()
    {
        LevelTimer += CurrentBonusTime;
        raceTimeProgress = 0;
        currentBonusTime = 0;
        currentBonusTimeInWorldTimeProgress = 0;
    }




    public void StartRace()
    {

        thisRace = raceState.raceIsRunning;
        RaceTimer = raceMaxTime;

        currentStartZone.SetActive(false);
        currentGoal.SetActive(true);
    }

    public void FinishRace()
    {
        thisRace = raceState.noRace;
        AddBonusTimeToWorldTimer();

        currentGoal.SetActive(false);

        SpawnNextRace();
    }




}
