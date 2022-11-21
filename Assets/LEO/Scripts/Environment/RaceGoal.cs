using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceGoal : MonoBehaviour
{
    LevelManager myLevelmanager;

    private void Awake()
    {
        myLevelmanager = FindObjectOfType<LevelManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            myLevelmanager.FinishRace();
        }
    }
}
