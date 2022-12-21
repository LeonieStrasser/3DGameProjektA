using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public GameObject cutsceneCam;
    public float cutsceneTime;

    private void Start()
    {
        cutsceneCam.SetActive(true);
        StartCoroutine(FinishStartCutscene());
    }

    IEnumerator FinishStartCutscene()
    {
        yield return new WaitForSeconds(cutsceneTime);
        cutsceneCam.SetActive(false);
        Destroy(cutsceneCam);
    } 
}
