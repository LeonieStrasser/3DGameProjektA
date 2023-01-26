using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using TMPro;

public class LevelManager : MonoBehaviour
{


    [Space(20)]
    [Header("Races")]


    [SerializeField] RaceData[] allRaces;
    [SerializeField] float raceMaxTime;
    [SerializeField] float raceSpawnFeedbackDelay;


    [Header("Loose")]
    [SerializeField] float looseScreenDelay;
    public GameObject gameoverCam;
    public GameObject ingameHUD;
    public float gameoverTillCutsceneTime;

    [Header("Inputs")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InputActionReference pauseAction;


    [Header("SaveDataInputs")]
    [SerializeField] private TMP_InputField nameInput;



    // GAMESTATES

    public enum raceState
    {
        noRace,
        raceIsRunning
    }

    public enum gameState
    {
        running,
        pause,
        gameOver
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
    public raceState ThisRace { get => thisRace; }




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



    [System.Serializable]
    public struct RaceData
    {
        [SerializeField] public GameObject startZone;
        [SerializeField] public GameObject goal;
        [SerializeField] public float raceMaxTime;
        [SerializeField] public int pointsForSuccess;
        [ResizableTextArea] [SerializeField] public string notes;
        [ResizableTextArea] [SerializeField] public string difficulty;
    }


    // Bonus Management
    EffectHandle myEffectHandle;


    // player
    [HideInInspector] public WhingMovement01 myPlayer;
    Vector3 playerPosition;
    public Vector3 PlayerPosition { get => playerPosition; }

    #region events

    public event Action<int, int, int> OnGameLoose;
    public event Action OnGamePaused;
    public event Action OnGameResume;
    public event Action OnRaceStart;
    public event Action OnRaceStop;

    #endregion


    private void Awake()
    {
        myEffectHandle = FindObjectOfType<EffectHandle>();
        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    private void Start()
    {
        ResumeGame();
        thisRace = raceState.noRace;



        if (allRaces.Length > 0)
            SpawnAllRaces();
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

        playerPosition = myPlayer.transform.position;
    }

    void PauseGame()
    {
        if (currentGameState == gameState.running)
        {
            OnGamePaused?.Invoke();
            currentGameState = gameState.pause;
            Time.timeScale = 0;
            AudioManager.instance.PauseRaceInProgress();
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
        AudioManager.instance.SetRaceInProgress();
    }

    public void GameLoose()
    {
        currentGameState = gameState.gameOver;
        int myScore = Mathf.RoundToInt(ScoreSystem.Instance.CurrentScore);
        int lastHighscore = ScoreSystem.Instance.highscore;
        int lastListScore = ScoreSystem.Instance.lastListScore;
        StartCoroutine(GameLooseDelayTimer(myScore, lastHighscore, lastListScore));

        ingameHUD.SetActive(false);
        StartCoroutine(GameoverCamWait());
    }

    IEnumerator GameoverCamWait()
    {
        yield return new WaitForSeconds(gameoverTillCutsceneTime);
        gameoverCam.SetActive(true);
    }

    private void SpawnAllRaces()
    {
        foreach (var item in allRaces)
        {
            item.startZone.SetActive(true);
        }

        // Audio Feedback
        StartCoroutine(RaceSpawnFeedbackDelay());
    }

    private void DeactivateAllRaces()
    {
        foreach (var item in allRaces)
        {
            item.startZone.SetActive(false);
        }
    }

    private void SetCurrentRace(RaceData newRace)
    {
        currentStartZone = newRace.startZone;
        currentGoal = newRace.goal;
        raceMaxTime = newRace.raceMaxTime;
        currentSuccessPoints = newRace.pointsForSuccess;
    }

    IEnumerator RaceSpawnFeedbackDelay()
    {
        yield return new WaitForSeconds(raceSpawnFeedbackDelay);
        AudioManager.instance.NewRaceSpawn(); // <-- New Race Spawn Sound
    }

    private void RunRaceTimer()
    {
        RaceTimer = Mathf.Clamp(raceTimer - Time.deltaTime, 0, raceMaxTime); // Timer runterz�hlen
        if (raceTimer <= 0)
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

    public void StartRace(int raceID)
    {
        //Finde Raus zu welchem Race die RaceID gehört
        RaceData triggeredRace = new RaceData();
        bool raceFound = false;

        foreach (var item in allRaces)
        {
            StartZone itemStartZone = item.startZone.GetComponent<StartZone>();
            if (itemStartZone.RaceID == raceID)
            {
                triggeredRace = item;
                raceFound = true;
            }
            else
            {
                // Deaktiviere alle anderen Race STarts
                itemStartZone.gameObject.SetActive(false);
            }
        }

        if (!raceFound)
        {
            Debug.LogError("Can´t find a race in the array with this goalID: " + raceID, gameObject);
            SpawnAllRaces();
            return;
        }


        SetCurrentRace(triggeredRace);

        thisRace = raceState.raceIsRunning;
        RaceTimer = raceMaxTime;



        // Start/goal handler
        currentStartZone.SetActive(false);
        currentGoal.SetActive(true);
        //-------------------------

        OnRaceStart?.Invoke();

        AudioManager.instance.StartRace(); // <-- Start Race SFX
        AudioManager.instance.RaceInProgressStart(); // <-- Race Music
        AudioManager.instance.SetRaceInProgress();
    }

    public void FinishRace()
    {
        thisRace = raceState.noRace;

        // Start/goal handler
        currentGoal.SetActive(false);
        //-----------------

        ScoreSystem.Instance.AddScore(currentSuccessPoints);

        myEffectHandle.StartBonusEffect();

        SpawnAllRaces();

        OnRaceStop?.Invoke();

        // Audio Feedbacks
        AudioManager.instance.RaceInProgressStop();
        AudioManager.instance.RaceFinished(); // <-- Finish Race Sound , needs time to play (BAM, finished race, Points, ... 1 or 2 sec delay -> dann Gambling
    }

    private void RaceFail()
    {
        thisRace = raceState.noRace;
        currentGoal.SetActive(false);

        // Hier müssen die Start Zones wieder angestellt werden
        SpawnAllRaces();

        OnRaceStop?.Invoke();

        AudioManager.instance.RaceInProgressStop();
        AudioManager.instance.RaceTimeUp();

    }

    private IEnumerator GameLooseDelayTimer(int score, int lastHighscore, int lastListScore)
    {
        yield return new WaitForSeconds(looseScreenDelay);
        OnGameLoose?.Invoke(score, lastHighscore, lastListScore);


    }


    public void SaveNewScore()
    {
        int score = Mathf.RoundToInt(ScoreSystem.Instance.CurrentScore);
        SaveSystem.SaveScore(score, nameInput.text);
    }

}
