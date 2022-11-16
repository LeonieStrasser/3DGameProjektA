using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Game Timer")]
    [SerializeField] Image progressBarImage;


    [Space(20)]
    [Header("UI-Sreens")]
    [SerializeField] GameObject looseScreen;

    LevelManager myLevelTimer;

    private void Awake()
    {
        myLevelTimer = FindObjectOfType<LevelManager>();
    }
    private void Start()
    {
        myLevelTimer.OnGameLoose += ActivateLooseScreen;
    }

    private void Update()
    {
        progressBarImage.fillAmount = myLevelTimer.LevelProgress;
    }

    private void ActivateLooseScreen()
    {
        looseScreen.SetActive(true);
    }

}
