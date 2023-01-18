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
    [SerializeField] TextMeshProUGUI contactScoreText;



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
    [SerializeField] TMP_InputField nameField;
    [SerializeField] TextMeshProUGUI rankText;


    [Header("Pause Screen")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Button resumeButton;
    [SerializeField] Button backButtonControls;

    LevelManager myManager;
    WhingMovement01 myPlayer;
    DistanceTracker disTracker;

    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
        myPlayer = FindObjectOfType<WhingMovement01>();
        disTracker = myPlayer.GetComponent<DistanceTracker>();

    }
    private void Start()
    {
        ScoreSystem.Instance.OnXpChange += UpdateXpText;
        ScoreSystem.Instance.OnComboStateChange += UpdateXpState;
        myManager.OnGameLoose += ActivateLooseScreen;
        myManager.OnGameResume += DeactivatePauseScreen;
        myManager.OnRaceStart += ActivateRaceTimeBar;
        myManager.OnRaceStop += DeactivateRaceTimeBar;
        disTracker.OnContactBreak += UpdateContactScoreText;
        //disTracker.OnContactBreak += DeactivateContactScore;


    }

    private void Update()
    {
        if (myManager.ThisRace == LevelManager.raceState.raceIsRunning) // Wenn ein Rennen läft aktualisiere den Bar
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
            WriteLastNameToInputField();
            menuButtonPannel.SetActive(false);

        }
        else if (score > lastListScore)
        {
            // RANKED PANNEL anzeigen
            rankedScorePannel.SetActive(true);
            inputPannel.SetActive(true);
            WriteLastNameToInputField();
            menuButtonPannel.SetActive(false);

            rankText.text = GetRank(score) + "."; // Hier könnte man je nach rank andere Effekte auftauchen lassen

        }
        else
        {
            // NORMALES LOOSE PANNEL anzeigebn
            noRankPannel.SetActive(true);
            inputPannel.SetActive(false);
            menuButtonPannel.SetActive(true);
        }


    }

    private void WriteLastNameToInputField()
    {
        nameField.text = PlayerPrefs.GetString(SaveSystem.LastNameKey);
    }

    private int GetRank(int score)
    {
        ScoreList loadData = SaveSystem.LoadScore();

        for (int i = 0; i < loadData.scoreDataList.Count; i++)
        {
            if (score > loadData.scoreDataList[i].score)
            {
                return i + 1; // Das ist der erreichte rang
            }
        }

        Debug.LogWarning("Wenn der Code hier ankommt, ist was faul. der Score sollte auf jeden fall über einem der Scores aus der Liste liegen. ", gameObject); // Ansonsten sollte diese Methode nicht aufgerufen werden sondern der "No Ranked" screen auftauchen.
        return int.MaxValue;
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
        UpdateContactScoreText();
       
    }

    private void UpdateXpState(Color stateColor)
    {
        xpText.color = stateColor;

    }

    private void UpdateContactScoreText()
    {
        contactScoreText.text = ScoreSystem.Instance.ContactScore.ToString();

        // Animation zum Score hin

    }

    private void DeactivateContactScore()
    {
        contactScoreText.gameObject.SetActive(false);
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
