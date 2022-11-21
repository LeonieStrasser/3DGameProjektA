using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Game Timer")]
    [SerializeField] Image progressBarImage;
    [SerializeField] Image raceProgressBarImage;

    [Header("Resource Bar")]
    [SerializeField] Image recourceBarImage;

    [Space(20)]
    [Header("UI-Sreens")]
    [SerializeField] GameObject looseScreen;

    LevelManager myLevelTimer;
    WhingMovement01 myPlayer;

    private void Awake()
    {
        myLevelTimer = FindObjectOfType<LevelManager>();
        myPlayer = FindObjectOfType<WhingMovement01>();
    }
    private void Start()
    {
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

}
