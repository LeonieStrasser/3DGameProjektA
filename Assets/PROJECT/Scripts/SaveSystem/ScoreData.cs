using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int score;
    public string playerName;
    public string date;

    public ScoreData(int newScore, string newName)
    {
        score = newScore;
        playerName = newName;
        date = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyy   HH:mm");
    }

    public int GetScore() { return score; }
}




[System.Serializable]
public class ScoreList
{
    public List<ScoreData> scoreDataList = new List<ScoreData>();

    public ScoreList(ScoreData[] newDataArray)
    {
        foreach (var item in newDataArray)
        {
            scoreDataList.Add(item);
        }
    }

    public List<ScoreData> GetScoreList() { return scoreDataList; }
}
