using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Game Timer")]
    [SerializeField] Image progressBarImage;


    LevelTimer myLevelTimer;

    private void Awake()
    {
        myLevelTimer = FindObjectOfType<LevelTimer>();
    }

    private void Update()
    {
        progressBarImage.fillAmount = myLevelTimer.LevelProgress;
    }
}
