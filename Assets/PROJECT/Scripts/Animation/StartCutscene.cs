using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public GameObject cutsceneCam;
    private WhingMovement01 playerMovemengt;
    public float cutsceneTime;

    private void Awake()
    {
        playerMovemengt = FindObjectOfType<WhingMovement01>();
    }
    private void Start()
    {
        cutsceneCam.SetActive(true);
        playerMovemengt.enabled = false;
        StartCoroutine(FinishStartCutscene());
    }

    IEnumerator FinishStartCutscene()
    {
        yield return new WaitForSeconds(cutsceneTime);
        cutsceneCam.SetActive(false);
        playerMovemengt.enabled = true;
        Destroy(cutsceneCam);
    } 
}
