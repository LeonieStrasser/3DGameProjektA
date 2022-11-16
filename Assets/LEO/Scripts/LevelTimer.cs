using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{

    [SerializeField] float startTime;
    private float levelTime;
    public float LevelTime
    {
        get => levelTime;
        set
        {
            levelTime = value;
            levelProgress = levelTime / startTime; // Direkt den Level Progress ausrechnen
        }
    }

    private float levelProgress;
    public float LevelProgress { get => levelProgress; set => levelProgress = value; }

    private void Start()
    {
        LevelTime = startTime;
    }

    private void Update()
    {
        RunTime();
    }
    private void RunTime()
    {
        LevelTime = Mathf.Clamp(levelTime - Time.deltaTime, 0, startTime);

        if (levelTime == 0)
            GameLoose();
    }

    private void GameLoose()
    {
        Debug.Log("GAME LOOSE!");

    }


}
