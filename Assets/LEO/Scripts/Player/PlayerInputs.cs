using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    public void OnPause()
    {
        SceneManager.LoadScene(0);
    }
}
