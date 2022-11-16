using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] GameObject packageMarkerPrefab;



    LevelManager myLevelManager;
    GameObject myMarker;

    private void Awake()
    {
        myLevelManager = FindObjectOfType<LevelManager>();
    }
    private void Start()
    {
        // UI Element erstellen, das die STartzone anzeigt
        myMarker = Instantiate(packageMarkerPrefab, Vector3.zero, Quaternion.identity);
        myMarker.GetComponent<PackageMarkerUI>().SetFollowTarget(this.transform);
    }

    private void OnTriggerEnter(Collider other) // Wenn der Player durchfliegt, startet er das Race
    {
        if (other.tag == "Player")
        {
            myLevelManager.StartRace();
            Destroy(this.gameObject);
        }
    }
}
