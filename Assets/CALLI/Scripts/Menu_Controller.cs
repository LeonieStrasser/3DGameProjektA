using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Controller : MonoBehaviour
{
    [Header("Levels To Load")]
    public string newGameLevel;
    private string levelToLoad;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadingGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel"); //locale variable nimmt bezug zu dem letzten Speicherstand des Levels
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
