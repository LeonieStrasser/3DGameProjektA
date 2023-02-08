using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject ingameHUD;

    [Header("Race UI")]
    [SerializeField] GameObject timeBarObject;
    [SerializeField] Image progressBarImage;
    [SerializeField] TextMeshProUGUI raceText;
    [SerializeField] Animator raceTextAnim;
    [SerializeField] string raceStartText;
    [SerializeField] string[] raceFinishText;
    [SerializeField] string raceFailText;
    [SerializeField] TextMeshProUGUI progressTextmesh;
    [SerializeField] string raceProgressText;
    [SerializeField] Animator raceProgressTextAnim;

    [Header("Resource Bar")]
    [SerializeField] Image[] recourceBarImage;

    [Header("XP")]
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] GameObject contactScoreTemplate;
    [SerializeField] GameObject contactTextContainer;
    [SerializeField] Color normalPointColor;
    [SerializeField] Color fuelPointColor;
    [SerializeField] Color lightPointColor;
    [SerializeField] float racePointFontSize = 50;
    [SerializeField] float scoreAddDelayAfterContactBreak;
    [SerializeField] float countDelay = 1;
    [SerializeField] [Tooltip("Zeit die der score bei einem Schub Punkte zum Hiochz�hlen braucht.")] float totalCountUpTime = 2;
    [SerializeField] Animator xpAnimator;
    TextMeshProUGUI contactScoreText;

    [Header("Multiplyer")]
    [SerializeField] GameObject twirlMultiplyMarker;


    [Space(20)]
    [Header("LooseScreen")]
    [SerializeField] GameObject looseScreen;
    [SerializeField] GameObject newHighscorePannel;
    [SerializeField] GameObject rankedScorePannel;
    [SerializeField] GameObject noRankPannel;
    [SerializeField] GameObject inputPannel;
    [SerializeField] GameObject menuButtonPannel;
    [SerializeField] Button retryButton;
    [SerializeField] TextMeshProUGUI[] scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] TextMeshProUGUI rankText;

    [SerializeField] GameObject BlendPlanes;
    [SerializeField] Animator blendAnim;


    [Header("Pause Screen")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject pauseButtons;
    [SerializeField] GameObject pauseControlls;
    [SerializeField] GameObject slide1Controlls;
    [SerializeField] GameObject slide2Controlls;
    [SerializeField] GameObject slide3Controlls;
    [SerializeField] GameObject slide4Controlls;
    [SerializeField] Button resumeButton;
    [SerializeField] Button saveScoreButton;
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
        ScoreSystem.Instance.OnAddScoreImediatly += SpawnNewImediateText;
        myManager.OnGameLoose += ActivateLooseScreen;
        myManager.OnCrashed += BlendToLooseScreen;
        myManager.OnGameResume += DeactivatePauseScreen;
        myManager.OnRaceStart += ActivateRaceTimeBar;
        myManager.OnRaceStart += RaceStartText;
        myManager.OnRaceStop += DeactivateRaceTimeBar;
        myManager.OnRaceFinish += RaceFinishText;
        myManager.OnRaceFail += RaceFailText;
        myPlayer.OnTwirlStart += ActivateTwirlUIBoolCheck;
        myPlayer.OnTwirlEnd += ActivateTwirlUIBoolCheck;
        disTracker.OnContactBreak += DeactivateContactScore;
        disTracker.OnContact += ActivateContactScore;

        // Contact score objekt wird zum instantiaten vorbereitet
        BlendPlanes.SetActive(false);
        contactScoreTemplate.SetActive(false);


        Cursor.visible = false;

    }

    private void Update()
    {
        if (myManager.ThisRace == LevelManager.raceState.raceIsRunning) // Wenn ein Rennen l�ft aktualisiere den Bar
            progressBarImage.fillAmount = myManager.RaceTimeProgress;

        foreach (var item in recourceBarImage)
        {
            item.fillAmount = myPlayer.ResourceAInRelationToMax;
        }

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

            if (xpAnimator.GetBool("countIsActive") == false)
            {
                AudioManager.instance.PointsAdd();
            }
            xpAnimator.SetBool("countIsActive", true);
        }
        else
        {
            xpAnimator.SetBool("countIsActive", false);
        }
    }

    private void BlendToLooseScreen()
    {
        BlendPlanes.SetActive(true);
        blendAnim.SetTrigger("GameOver");
    }

    private void ActivateLooseScreen(int score, int lastHighscore, int lastListScore)
    {
        Cursor.visible = true;
        ingameHUD.SetActive(false);

        foreach (var item in scoreText)
        {
            item.text = score.ToString();

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
            saveScoreButton.Select();

        }
        else if (score > lastListScore)
        {
            // RANKED PANNEL anzeigen
            rankedScorePannel.SetActive(true);
            inputPannel.SetActive(true);
            WriteLastNameToInputField();
            menuButtonPannel.SetActive(false);
            saveScoreButton.Select();
            rankText.text = GetRank(score) + "."; // Hier k�nnte man je nach rank andere Effekte auftauchen lassen

        }
        else
        {
            // NORMALES LOOSE PANNEL anzeigebn
            noRankPannel.SetActive(true);
            inputPannel.SetActive(false);
            menuButtonPannel.SetActive(true);
            retryButton.Select();
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
        Cursor.visible = true;
    }

    public void DeactivatePauseScreen()
    {
        pauseButtons.SetActive(true);
        slide1Controlls.SetActive(true);
        slide2Controlls.SetActive(false);
        slide3Controlls.SetActive(false);
        slide4Controlls.SetActive(false);
        pauseControlls.SetActive(false);
        pauseScreen.SetActive(false);

        Cursor.visible = false;
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
        contactScoreText.color = normalPointColor;
        newMarker.SetActive(true);
    }

    private void UpdateContactScoreText()
    {
        if (contactScoreText)
        {
            contactScoreText.text = Mathf.RoundToInt(ScoreSystem.Instance.ContactScore).ToString();
            AudioManager.instance.PointsUpStart();
            AudioManager.instance.PointsUpActive();
        }
    }

    private void ActivateContactScore()
    {
        SpawnNewContactText();
        if (myPlayer.Twirl)
        {
            ActivateTwirlUI();
        }
    }

    private void DeactivateContactScore()
    {
        if (contactScoreText.gameObject.activeSelf)
        {
            contactScoreText.GetComponentInParent<UI_Marker>().DeactivatePlayerFollow();
            AudioManager.instance.PointsUpStop();
            AudioManager.instance.PointsMovingDown();
        }
        contactScoreText = null;

        UpdateXPTextReachValue();
        DeactivateTwirlUI();

    }
    #endregion
    #region imediatlyScore

    private void SpawnNewImediateText(float scoreValue, ScoreSystem.scoreType typeOfScore)
    {
        // Neues UI TextMarker Objekt spawnen
        GameObject newMarker = Instantiate(contactScoreTemplate, contactTextContainer.transform);
        // Text setzen
        TextMeshProUGUI markerTMP = newMarker.GetComponentInChildren<TextMeshProUGUI>();
        markerTMP.text = Mathf.RoundToInt(scoreValue).ToString();
        newMarker.SetActive(true);
        // color setzen
        switch (typeOfScore)
        {
            case ScoreSystem.scoreType.fuelBubblePoints:
                markerTMP.color = fuelPointColor;
                break;
            case ScoreSystem.scoreType.lightPoints:
                markerTMP.color = lightPointColor;
                break;
            case ScoreSystem.scoreType.racePoints:
                markerTMP.color = normalPointColor;
                markerTMP.fontSize = racePointFontSize;
                break;
            default:
                break;
        }

        // Vom Player entkoppeln
        StartCoroutine(ImetiatlyPointDisconnectTimer(newMarker));
    }
    IEnumerator ImetiatlyPointDisconnectTimer(GameObject marker)
    {
        yield return new WaitForEndOfFrame();
        marker.GetComponentInChildren<UI_Marker>().DeactivatePlayerFollow();
        UpdateXPTextReachValue();
    }

    #endregion
    #region twirlUI
    private void ActivateTwirlUI()
    {

        twirlMultiplyMarker.SetActive(true);
    }

    private void DeactivateTwirlUI()
    {

        twirlMultiplyMarker.SetActive(false);
    }
    private void ActivateTwirlUIBoolCheck(bool twirlState)
    {
        if (disTracker.DistanceActive)
            twirlMultiplyMarker.SetActive(twirlState);
    }


    #endregion




    private void ActivateRaceTimeBar()
    {
        timeBarObject.SetActive(true);
    }

    private void DeactivateRaceTimeBar()
    {
        timeBarObject.SetActive(false);

    }

    private void RaceStartText()
    {
        raceText.text = raceStartText;
        raceTextAnim.SetTrigger("activate");
    }

    private void RaceFinishText()
    {
        int randomNr = Random.Range(0, raceFinishText.Length);
        raceText.text = raceFinishText[randomNr];
        raceTextAnim.SetTrigger("activate");

        progressTextmesh.text = myManager.RaceProgress + " / " + myManager.AllRacesCount + " " + raceProgressText;
        // Textanimation
        raceProgressTextAnim.SetTrigger("raceFinish");
    }

    private void RaceFailText()
    {
        raceText.text = raceFailText;
        raceTextAnim.SetTrigger("activate");
    }

}
