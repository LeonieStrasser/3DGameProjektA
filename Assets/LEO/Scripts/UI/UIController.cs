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

    LevelManager myLevelTimer;
    WhingMovement01 myPlayer;

    private void Awake()
    {
        myLevelTimer = FindObjectOfType<LevelManager>();
        myPlayer = FindObjectOfType<WhingMovement01>();
    }
    private void Start()
    {
        ScoreSystem.Instance.OnXpChange += UpdateXpText;
        myLevelTimer.OnGameLoose += ActivateLooseScreen;
    }

    private void Update()
    {
        progressBarImage.fillAmount = myLevelTimer.LevelProgress;
        raceProgressBarImage.fillAmount = myLevelTimer.CurrentBonusTimeInWorldTimeProgress;
        recourceBarImage.fillAmount = myPlayer.ResourceAInRelationToMax;
    }

    private void ActivateLooseScreen()
    {
        looseScreen.SetActive(true);
    }

    void ActivatePauseScreen()
    {
        pauseScreen.SetActive(true);
    }

    void DeactivatePauseScreen()
    {
        pauseScreen.SetActive(false);
    }

    private void UpdateXpText(int newScore)
    {
        xpText.text = newScore.ToString();
    }
}
