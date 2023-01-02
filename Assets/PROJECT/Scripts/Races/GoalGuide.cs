using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGuide : MonoBehaviour
{
    [SerializeField] GameObject spawnSphere;
    [SerializeField] float timeDelay = 2;

    LevelManager myManager;
    bool timerIsRunning = false;

    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (myManager.CurrentGameState == LevelManager.gameState.running)
        {
            if (!timerIsRunning)
            {
                timerIsRunning = true;
                StartCoroutine(spawnTimer());
            }
        }
    }

    IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(timeDelay);
        GameObject newSphere = Instantiate(spawnSphere, this.transform.position, Quaternion.identity);
        newSphere.GetComponent<GoalGuidanceSphere>().SetLevelManager(myManager);

        timerIsRunning = false;
    }
}
