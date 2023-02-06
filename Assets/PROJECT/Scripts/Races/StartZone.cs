using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class StartZone : MonoBehaviour
{
    [SerializeField] GameObject packageMarkerPrefab;
   
    [HideInInspector] public int raceID;

    TextMeshPro idText;


    public int RaceID { get { return raceID; } }

    LevelManager myLevelManager;
    GameObject myMarker;

    private void Awake()
    {
        myLevelManager = FindObjectOfType<LevelManager>();

    }
    private void Start()
    {
        //// UI Element erstellen, das die STartzone anzeigt
        //myMarker = Instantiate(packageMarkerPrefab, Vector3.zero, Quaternion.identity);
        //myMarker.GetComponent<PackageMarkerUI>().SetFollowTarget(this.transform);

        idText = GetComponentInChildren<TextMeshPro>();
        idText.text = "#" + raceID;
    }

    private void OnTriggerEnter(Collider other) // Wenn der Player durchfliegt, startet er das Race
    {
        if (other.tag == "Player")
        {
            myLevelManager.StartRace(raceID);
            
        }
    }
}
