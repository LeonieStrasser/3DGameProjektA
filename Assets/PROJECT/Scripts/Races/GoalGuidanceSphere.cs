using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGuidanceSphere : MonoBehaviour
{
    LevelManager myManager;

    [SerializeField] float scaleFactor;
    float timer;
    Vector3 scaleZero = new Vector3(0, 0, 0);
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * scaleFactor;
        if (myManager.CurrentGameState == LevelManager.gameState.running)
        {

            transform.localScale = Vector3.Lerp(transform.localScale, scaleZero, timer);
        }
        if (timer > 1) Destroy(gameObject);
    }
    public void SetLevelManager(LevelManager lvlManager) // Sollte direkt beim instantiaten zugewiesen werden / übergeben
    {
        myManager = lvlManager;
    }
}


