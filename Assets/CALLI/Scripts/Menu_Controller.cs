using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField] private int defaultSen = 1;
    public int mainControllerSen = 1;

    [Header("Toggle Settings")]

    [SerializeField] private Toggle invertYToggle = null;

    [Header("Confirmation")]

    [SerializeField] private GameObject confirmationPrompt = null;


    [Header("Levels To Load")]

    public string newGameLevel;
    private string levelToLoad;

    public string loadTutorial;

    public string loadCredits;


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

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSensitivity(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }

    //public void GameplayApply() // AN CONTROLLER INPUT ANPASSEN!
    //{
    //    if (invertYToggle.isOn)
    //    {
    //        PlayerPrefs.SetInt("masterInvertY", 1);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("masterInvertY", 0);
    //    }

    //    PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    //    StartCoroutine(ConfirmationBox());
    //}

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultSound;
            soundSlider.value = defaultSound;
            soundTextValue.text = defaultSound.ToString("0.0");
            musicSlider.value = defaultMusic;
            musicTextValue.text = defaultMusic.ToString("0.0");
            VolumeApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

}
