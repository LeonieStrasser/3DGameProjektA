using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class Menu_Controller : MonoBehaviour
{
    [Header("Volume Settings")] // Sound & Music verknüpfen!!
    [SerializeField] private TMP_Text soundTextValue = null;
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private float defaultSound = 0.5f;

    [SerializeField] private TMP_Text musicTextValue = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private float defaultMusic = 0.5f;

    [Header("Gameplay Settings")] //Controller Sensitivity verknüpfen!!
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    private float defaultSen;
    private float defaultSenMin;
    private float defaultSenMax;


    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;


    [Header("Levels To Load")]

    public string newGameLevel;

    private string levelToLoad;

    public string loadTutorial;

    public string loadCredits;


    private void Start()
    {
        defaultSenMin = 0;
        controllerSenSlider.minValue = 0;
        defaultSenMax = controllerSenSlider.maxValue;
    }

    //Einstellungen für das Main Menu
    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadingGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel"); //locale variable nimmt bezug zu dem letzten Speicherstand des Levels
        }
    }

    public void LoadingTutorialDialogYes()
    {
        SceneManager.LoadScene(loadTutorial);
    }

    public void LoadingCreditsDialogYes()
    {
        SceneManager.LoadScene(loadCredits);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public void SetSound(float sound)
    {
        AudioListener.volume = sound;
        soundTextValue.text = sound.ToString("0.0");
    }

    public void SetMusic(float music)
    {
        AudioListener.volume = music;
        musicTextValue.text = music.ToString("0.0");
    }

    public void UIControllerSensitivity() // AN CONTROLLER SENSITIVITY ANPASSEN
    {   
        controllerSenTextValue.text = Mathf.RoundToInt(controllerSenSlider.value).ToString("0");
    }

    public void UIVolumeSlider()
    {
        //soundTextValue.text = Mathf.RoundToInt().ToString("0.0");
    }

    public void GameplayApply() 
    {
        int newInvertValue;
        if (invertYToggle.isOn)
        {
            newInvertValue = 1;
        }
        else
        {
            newInvertValue = 0;
        }

        //Hier kommt immer ein Wert zwischen 0 und 2 raus
        float convertSensitivity = defaultSen / defaultSenMax;
        SaveSystem.SaveOptions(newInvertValue, convertSensitivity); /*volume);*/

        //Visual Feedback
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultSound;
            soundSlider.value = defaultSound;
            soundTextValue.text = defaultSound.ToString("0.0");
            musicSlider.value = defaultMusic;
            musicTextValue.text = defaultMusic.ToString("0.0");
            //VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            defaultSen = defaultSenMax / 2;
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
