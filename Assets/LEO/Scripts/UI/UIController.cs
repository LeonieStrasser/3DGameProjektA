using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject ingameHUD;

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
    [SerializeField] GameObject newHighscorePannel;
    [SerializeField] GameObject rankedScorePannel;
    [SerializeField] GameObject noRankPannel;
    [SerializeField] GameObject inputPannel;
    [SerializeField] GameObject menuButtonPannel;
    [SerializeField] TextMeshProUGUI[] scoreText;
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
        ScoreSystem.Instance.OnComboStateChange += UpdateXpState;
        myManager.OnGameLoose += ActivateLooseScreen;
        myManager.OnGameResume += DeactivatePauseScreen;
        myManager.OnRaceStart += ActivateRaceTimeBar;
        myManager.OnRaceStop += DeactivateRaceTimeBar;



    }

    private void Update()
    {
        if (myManager.ThisRace == LevelManager.raceState.raceIsRunning) // Wenn ein Rennen l�ft aktualisiere den Bar
            progressBarImage.fillAmount = myManager.RaceTimeProgress;

        recourceBarImage.fillAmount = myPlayer.ResourceAInRelationToMax;

        myManager.OnGamePaused += ActivatePauseScreen;
    }

    private void ActivateLooseScreen(int score, int lastHighscore, int lastListScore)
    {
        ingameHUD.SetActive(false);

        foreach (var item in scoreText)
        {
            item.text = "Score: " + score;

        }
        highScoreText.text = "Last Highscore: " + lastHighscore;
        looseScreen.SetActive(true);


        if (score > lastHighscore)
        {
            // NEW HIGHSCORE Pannel anzeigen
            newHighscorePannel.SetActive(true);
            inputPannel.SetActive(true);
            menuButtonPannel.SetActive(false);

        }
        else if (score > lastListScore)
        {
            // RANKED PANNEL anzeigen
            rankedScorePannel.SetActive(true);
            inputPannel.SetActive(true);
            menuButtonPannel.SetActive(false);
        }
        else
        {
            // NORMALES LOOSE PANNEL anzeigebn
            noRankPannel.SetActive(true);
            inputPannel.SetActive(false);
            menuButtonPannel.SetActive(true);
        }
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

    private void UpdateXpState(Color stateColor)
    {
        xpText.color = stateColor;
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
