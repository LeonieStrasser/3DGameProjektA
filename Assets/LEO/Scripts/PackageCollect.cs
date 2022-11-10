using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCollect : MonoBehaviour
{
    BombTimer playerBomb;
    DeliveryManager collectTimer;
    [SerializeField] GameObject packageMarkerPrefab;
    GameObject myMarker;

    private void Awake()
    {
        playerBomb = FindObjectOfType<BombTimer>();
        collectTimer = FindObjectOfType<DeliveryManager>();
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
