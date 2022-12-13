using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    int score;
    string playerName;
    string date;

    public ScoreData(int newScore, string newName)
    {
        score = newScore;
        playerName = newName;
        date = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyy   HH:mm");
    }

    public int GetScore() { return score; }
}
