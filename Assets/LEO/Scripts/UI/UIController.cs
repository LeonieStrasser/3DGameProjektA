using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Game Timer")]
    [SerializeField] Image progressBarImage;
    [SerializeField] Image raceProgressBarImage;

    [Header("Resource Bar")]
    [SerializeField] Image recourceBarImage;

    [Header("XP")]
    [SerializeField] TextMeshProUGUI xpText;



    [Space(20)]
    [Header("UI-Sreens")]
    [SerializeField] GameObject looseScreen;

    [SerializeField] GameObject pauseScreen;

    [Header("Buttons")]
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

    }

    private void Update()
    {
        progressBarImage.fillAmount = myManager.LevelProgress;
        raceProgressBarImage.fillAmount = myManager.CurrentBonusTimeInWorldTimeProgress;
        recourceBarImage.fillAmount = myPlayer.ResourceAInRelationToMax;

        myManager.OnGamePaused += ActivatePauseScreen;
    }

    private void ActivateLooseScreen()
    {
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
}
