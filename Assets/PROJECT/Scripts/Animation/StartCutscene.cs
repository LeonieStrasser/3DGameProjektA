using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public GameObject cutsceneCam;
    private WhingMovement01 playerMovement;
    private CutsceneMovement cutscnMovement;
    public float cutsceneTime;

    private void Awake()
    {
        playerMovement = FindObjectOfType<WhingMovement01>();
        cutscnMovement = FindObjectOfType<CutsceneMovement>();
    }
    private void Start()
    {
        cutsceneCam.SetActive(true);

        playerMovement.enabled = false;
        cutscnMovement.enabled = true;
        //playerMovemengt.enabled = false;
        StartCoroutine(FinishStartCutscene());
    }

    IEnumerator FinishStartCutscene()
    {
        yield return new WaitForSeconds(cutsceneTime);
        cutsceneCam.SetActive(false);
       // playerMovemengt.enabled = true;
        Destroy(cutsceneCam);

        playerMovement.enabled = true;
        cutscnMovement.enabled = false;
    } 
}
