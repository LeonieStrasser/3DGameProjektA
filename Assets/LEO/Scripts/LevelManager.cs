using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{


    [Space(20)]
    [Header("Races")]


    [SerializeField] RaceData[] allRaces;
    [SerializeField] float raceMaxTime;


    [Header("Loose")]
    [SerializeField] float looseScreenDelay;

    [Header("Inputs")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InputActionReference pauseAction;





    // GAMESTATES

    public enum raceState
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
    public raceState ThisRace { get =>  thisRace; }




    // CURRENT RACE
    [Header("debug")]
    private GameObject currentGoal;
    private GameObject currentStartZone;
    private int currentSuccessPoints;

    private float raceTimer;
    private float RaceTimer
    {
        set
        {
            raceTimer = value;
            raceTimeProgress = raceTimer / raceMaxTime;
        }
    }
    private float raceTimeProgress; // Zahl zwischen 0 und 1 - F�r den Balken im UI
    public float RaceTimeProgress { get => raceTimeProgress; }



    private int raceNumber = 0;

    [System.Serializable]
    public struct RaceData
    {
        [SerializeField] public GameObject startZone;
        [SerializeField] public GameObject goal;
        [SerializeField] public float raceMaxTime;
        [SerializeField] public int pointsForSuccess;
    }





    #region events

    public event Action OnGameLoose;
    public event Action OnGamePaused;
    public event Action OnGameResume;
    public event Action OnRaceStart;
    public event Action OnRaceStop;

    #endregion
    private void Start()
    {
        ResumeGame();
        thisRace = raceState.noRace;

        // Erstes Rennen wird gespawnt und zugewiesen
        raceNumber = -1;

        if (allRaces.Length > 0)
            SpawnNextRace();
        else
            Debug.LogWarning("Kein Race in der Liste!");
    }

    private void Update()
    {
        if (currentGameState == gameState.running)
        {
            if (thisRace == raceState.raceIsRunning)
            {
                RunRaceTimer();
            }
        }
        if (pauseAction.action.WasPressedThisFrame())
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (currentGameState == gameState.running)
        {
            OnGamePaused?.Invoke();
            currentGameState = gameState.pause;
            Time.timeScale = 0;
        }
        else if (currentGameState == gameState.pause)
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
        currentSuccessPoints = allRaces[raceNumber].pointsForSuccess;

        currentStartZone.SetActive(true);
    }

    private void RunRaceTimer()
    {
        RaceTimer = Mathf.Clamp(raceTimer - Time.deltaTime, 0, raceMaxTime); // Timer runterz�hlen
        if(raceTimer <= 0)
        {
            RaceFail();
        }
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

        OnRaceStart?.Invoke();
    }

    public void FinishRace()
    {
        thisRace = raceState.noRace;

        currentGoal.SetActive(false);

        ScoreSystem.Instance.AddScore(currentSuccessPoints);

        SpawnNextRace();

        OnRaceStop?.Invoke();
    }

    private void RaceFail()
    {
        thisRace = raceState.noRace;
        currentStartZone.SetActive(true);
        currentGoal.SetActive(false);

        OnRaceStop?.Invoke();

    }

    private IEnumerator GameLooseDelayTimer()
    {
        yield return new WaitForSeconds(looseScreenDelay);
        OnGameLoose?.Invoke();
    }

}
