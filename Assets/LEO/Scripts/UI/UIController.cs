using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Race Timer")]
    [SerializeField] GameObject timeBarObject;
    [SerializeField] Image progressBarImage;
    [SerializeField] Animator fuelBarAnim;

    [Header("Resource Bar")]
    [SerializeField] Image recourceBarImage;

    [Header("XP")]
    [SerializeField] TextMeshProUGUI xpText;



    [Space(20)]
    [Header("LooseScreen")]
    [SerializeField] GameObject looseScreen;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;



    [Header("Pause Screen")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Button resumeButton;
    [SerializeField] Button backButtonControls;

    LevelManager myManager;
    WhingMovement01 myPlayer;

    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
        myPlayer = FindObjectOfType<WhingMovement01>();

    }
    private void Start()
    {
        ScoreSystem.Instance.OnXpChange += UpdateXpText;
        myManager.OnGameLoose += ActivateLooseScreen;
        myManager.OnGameResume += DeactivatePauseScreen;
        myManager.OnRaceStart += ActivateRaceTimeBar;
        myManager.OnRaceStop += DeactivateRaceTimeBar;


    }

    private void Update()
    {
        if (myManager.ThisRace == LevelManager.raceState.raceIsRunning) // Wenn ein Rennen läft aktualisiere den Bar
            progressBarImage.fillAmount = myManager.RaceTimeProgress;

        recourceBarImage.fillAmount = myPlayer.ResourceAInRelationToMax;

        myManager.OnGamePaused += ActivatePauseScreen;
    }

    private void ActivateLooseScreen(int score, int lastHighscore)
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "Last Highscore: " + lastHighscore;
        looseScreen.SetActive(true);
    }

    private void ActivatePauseScreen()
    {
        pauseScreen.SetActive(true);
        resumeButton.Select();
    }

    public void DeactivatePauseScreen()
    {
        pauseScreen.SetActive(false);
    }

    private void UpdateXpText(int newScore)
    {
        xpText.text = newScore.ToString();
    }

    private void ActivateRaceTimeBar()
    {
        timeBarObject.SetActive(true);
        fuelBarAnim.SetBool("raceRun", true);
    }

    private void DeactivateRaceTimeBar()
    {
        timeBarObject.SetActive(false);
        fuelBarAnim.SetBool("raceRun", false);
    }
}
