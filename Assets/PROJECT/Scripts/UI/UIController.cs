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
    [SerializeField] GameObject contactScoreTemplate;
    [SerializeField] GameObject contactTextContainer;
    [SerializeField] float scoreAddDelayAfterContactBreak;
    [SerializeField] float countDelay = 1;
    [SerializeField] [Tooltip("Zeit die der score bei einem Schub Punkte zum Hiochz�hlen braucht.")] float totalCountUpTime = 2;
    [SerializeField] Animator xpAnimator;
    TextMeshProUGUI contactScoreText;

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


    // Count Up Text

    int scoreToReach = 0;
    float currentViewScore;

    float countTimer = 0;


    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
        myPlayer = FindObjectOfType<WhingMovement01>();
        disTracker = myPlayer.GetComponent<DistanceTracker>();


    }
    private void Start()
    {

        ScoreSystem.Instance.OnXpChange += UpdateContactScoreText;
        ScoreSystem.Instance.OnComboStateChange += UpdateXpState;
        myManager.OnGameLoose += ActivateLooseScreen;
        myManager.OnGameResume += DeactivatePauseScreen;
        myManager.OnRaceStart += ActivateRaceTimeBar;
        myManager.OnRaceStop += DeactivateRaceTimeBar;
        disTracker.OnContactBreak += DeactivateContactScore;
        disTracker.OnContact += ActivateContactScore;

        // Contact score objekt wird zum instantiaten vorbereitet
        contactScoreTemplate.SetActive(false);


    }

    private void Update()
    {
        if (myManager.ThisRace == LevelManager.raceState.raceIsRunning) // Wenn ein Rennen l�ft aktualisiere den Bar
            progressBarImage.fillAmount = myManager.RaceTimeProgress;

        recourceBarImage.fillAmount = myPlayer.ResourceAInRelationToMax;

        myManager.OnGamePaused += ActivatePauseScreen;

        CountUpXPText();
    }

    private void CountUpXPText()
    {

        // Hier z�hlt der SCore die Zahlen einzeln hoch
        if (scoreToReach > currentViewScore)
        {
            countTimer += Time.deltaTime;
            if (countTimer > countDelay)
            {
                countTimer = 0;
                currentViewScore += 1 + (scoreToReach - currentViewScore) / (totalCountUpTime / countDelay);
                xpText.text = Mathf.RoundToInt(currentViewScore).ToString();
            }

            xpAnimator.SetBool("countIsActive", true);
        }
        else
        {
            xpAnimator.SetBool("countIsActive", false);
        }
    }


    private void ActivateLooseScreen(int score, int lastHighscore, int lastListScore)
    {
        ingameHUD.SetActive(false);

        foreach (var item in scoreText)
        {
            item.text = "SCORE " + score;

        }
        highScoreText.text = "LAST HIGHSCORE " + lastHighscore;
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

            rankText.text = GetRank(score) + "."; // Hier k�nnte man je nach rank andere Effekte auftauchen lassen

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

        Debug.LogWarning("Wenn der Code hier ankommt, ist was faul. der Score sollte auf jeden fall �ber einem der Scores aus der Liste liegen. ", gameObject); // Ansonsten sollte diese Methode nicht aufgerufen werden sondern der "No Ranked" screen auftauchen.
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

    private void UpdateXPTextReachValue()
    {
        StartCoroutine(ScoreAddUpdateTimer());
    }
    IEnumerator ScoreAddUpdateTimer()
    {
        yield return new WaitForSeconds(scoreAddDelayAfterContactBreak);
        scoreToReach = Mathf.RoundToInt(ScoreSystem.Instance.CurrentScore);

        // Animation
        xpAnimator.SetTrigger("scoreBurst");
    }



    private void UpdateXpState(Color stateColor)
    {
        xpText.color = stateColor;

    }




    #region contactScore
    private void SpawnNewContactText()
    {
        GameObject newMarker = Instantiate(contactScoreTemplate, contactTextContainer.transform);
        contactScoreText = newMarker.GetComponentInChildren<TextMeshProUGUI>();
        newMarker.SetActive(true);
    }

    private void UpdateContactScoreText()
    {
        if (contactScoreText)
            contactScoreText.text = ScoreSystem.Instance.ContactScore.ToString();
    }

    private void ActivateContactScore()
    {
        SpawnNewContactText();

    }

    private void DeactivateContactScore()
    {
        if (contactScoreText.gameObject.activeSelf)
        {
            contactScoreText.GetComponentInParent<UI_Marker>().DeactivatePlayerFollow();
        }
        contactScoreText = null;

        UpdateXPTextReachValue();

    }



    #endregion

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
