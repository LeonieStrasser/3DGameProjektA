using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string folderLocalisation = "/JSONsaveFile.json";
    public static void SaveScore(int playerScore, string playerName)
    {

        ScoreData data = new ScoreData(playerScore, playerName);

        // Die aktuelle bestenlieste wird geladen - ist noch keine da wird ne neue angelegt
        List<ScoreData> dataList = LoadScore()?.scoreDataList;
        if (dataList == null)
            dataList = new List<ScoreData>();

        // Der neue Score wird der Scoreliste hinzugef�gt und richtig eingeordnet/sortiert
        dataList.Add(data);
        dataList = dataList.OrderByDescending (q => q.score).ToList();

        // Die sortierte ScoreListe wird in einen Array umgewandelt um ihn an die save methode �bergeben zu k�nnen
        ScoreData[] scoreArray = new ScoreData[dataList.Count];
        for (int i = 0; i < scoreArray.Length; i++)
        {
            scoreArray[i] = dataList[i];
        }

        // Ein ScoreList wird aus dem Array erstellt
        ScoreList newHighscoreListData = new ScoreList(scoreArray);

        // Abspeichern der neuen ScoreListe
        string path = Application.persistentDataPath + folderLocalisation;

        File.WriteAllText(path, JsonUtility.ToJson(newHighscoreListData));

    }

    public static ScoreList LoadScore()
    {
        string path = Application.persistentDataPath + folderLocalisation;

        if (File.Exists(path))
        {



            ScoreList data = JsonUtility.FromJson<ScoreList>(File.ReadAllText(path));
            return data;

        }
        else
        {
            Debug.LogError("SaveScore not found in " + path);
            return null;
        }
    }
}


