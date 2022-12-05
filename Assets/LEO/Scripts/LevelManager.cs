using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Timer")]
    [SerializeField] float startTime;

    [Space(20)]
    [Header("Races")]


    [SerializeField] float raceMaxTime;

    [Space(5)]
    [SerializeField] float maxBonusTime;

     [Header("Loose")]
    [SerializeField] float looseScreenDelay;

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

    public enum gameState
    {
        running,
        pause
    }
    raceState thisRace;

    gameState currentGameState;
    public gameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

    public event Action OnGameLoose;
    public event Action OnGamePaused;

    public event Action OnGameResume;


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
    private float raceTimeProgress; // Zahl zwischen 0 und 1 - F�r den Balken im UI
    public float RaceTimeProgress { get => raceTimeProgress; }

    private float currentBonusTime;
    public float CurrentBonusTime { get => currentBonusTime; }

    private float currentBonusTimeInWorldTimeProgress;
    public float CurrentBonusTimeInWorldTimeProgress { get => currentBonusTimeInWorldTimeProgress; }

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InputActionReference pauseAction;


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
        ResumeGame();   
        LevelTimer = startTime;
        thisRace = raceState.noRace;

        // Erstes Rennen wird gespawnt und zugewiesen
        raceNumber = -1;

        if(allRaces.Length > 0)
             SpawnNextRace();
             else
             Debug.LogWarning("Kein Race in der Liste!");
    }

    private void Update()
    {   
        if(currentGameState == gameState.running)
        {
            if (thisRace == raceState.raceIsRunning)
            {
                RunRaceTimer();
            }
        }
            if(pauseAction.action.WasPressedThisFrame())
            {
                PauseGame();
            } 
    }

    void PauseGame()
    {
        if(currentGameState == gameState.running)
        {
            OnGamePaused?.Invoke();
            currentGameState = gameState.pause;
            Time.timeScale = 0;
        }
        else if(currentGameState == gameState.pause)
        {
            ResumeGame();
        }
    }

     public void ResumeGame()
    {
        OnGameResume?.Invoke();
        Time.timeScale = 1;
        currentGameState = gameState.running;
    }

    public void GameLoose()
    {
        StartCoroutine(GameLooseDelayTimer());
    }

    private void SpawnNextRace()
    {
        raceNumber++; // Racenummer wird erstmal hochgerechnet - hei�t die Rennen laufen der Reihe nach ab. Sp�ter sollten wir hier ein zuf�lliges Ziehen ohne zur�cklegen reinbauen.
        if (raceNumber > allRaces.Length - 1) { raceNumber = 1; } // Tutorial Strecke wird nicht wiederholt

        currentStartZone = allRaces[raceNumber].startZone;
        currentGoal = allRaces[raceNumber].goal;
        raceMaxTime = allRaces[raceNumber].raceMaxTime;

        currentStartZone.SetActive(true);
    }

    private void RunRaceTimer()
    {
        RaceTimer = Mathf.Clamp(raceTimer - Time.deltaTime, 0, raceMaxTime); // Timer runterz�hlen
    }

    private void AddBonusTimeToWorldTimer()
    {
        LevelTimer += CurrentBonusTime;
        raceTimeProgress = 0;
        currentBonusTime = 0;
        currentBonusTimeInWorldTimeProgress = 0;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
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
    
    private IEnumerator GameLooseDelayTimer()
    {
        yield return new WaitForSeconds(looseScreenDelay);
        OnGameLoose?.Invoke();
    }
    
}
