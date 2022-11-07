using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollect : MonoBehaviour
{
    BombTimer playerBomb;
    CollectableBombTimer collectTimer;
    [SerializeField] GameObject packageMarkerPrefab;
    GameObject myMarker;

    private void Awake()
    {
        playerBomb = FindObjectOfType<BombTimer>();
        collectTimer = FindObjectOfType<CollectableBombTimer>();
    }
    private void Start()
    {
        myMarker = Instantiate(packageMarkerPrefab, Vector3.zero, Quaternion.identity);
        myMarker.GetComponent<PackageMarkerUI>().SetFollowTarget(this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collectTimer.ResetTimer();
            playerBomb.PickUpBomb();
            Destroy(this.gameObject);
        }
    }
}
