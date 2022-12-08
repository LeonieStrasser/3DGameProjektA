using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string folderLocalisation = "/scores.mango";
   public static void SaveScore(int playerScore, string playerName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + folderLocalisation;
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData(playerScore, playerName);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScore()
    {
        string path = Application.persistentDataPath + folderLocalisation;

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData data = formatter.Deserialize(stream) as ScoreData;  // Daten werden von Binary in ScoreData Class umgewandelt
            stream.Close();

            return data;

        }else
        {
            Debug.LogError("SaveScore not found in " + path);
            return null;
        }
    }
}
