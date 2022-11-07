using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void OnRelodeScene()
    {
        SceneManager.LoadScene(0);
    }
}
