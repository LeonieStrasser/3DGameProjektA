using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceTracker : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    [SerializeField] GameObject spawnVFX;
    [SerializeField] [Range(0, 0.5f)] float spawnDelay;
    [SerializeField] SphereCollider distanceTrigger;
    [SerializeField] int points;

    bool spawnTimer;

    private void Start()
    {
        distanceTrigger.radius = detectionRadius;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger && other.tag != "Player")
        {
            Vector3 closestPoint = other.ClosestPoint(distanceTrigger.gameObject.transform.position);
            if (Vector3.Distance(distanceTrigger.transform.position, closestPoint) > 0.001)
            {
                StartCoroutine(vfxSpawnTimer(closestPoint));
            }else
            {
                Debug.LogWarning("Tryed to spawn a Distance-VFX on the player! Da war wahrscheinlich ein Mesh Collider schuld! Objektname: " + other.gameObject.name);
            }
        }



    }

    IEnumerator vfxSpawnTimer(Vector3 spawnPosition)
    {
        spawnTimer = true;
        Instantiate(spawnVFX, spawnPosition, Quaternion.identity);
        ScoreSystem.Instance.AddScore(points);
        yield return new WaitForSeconds(spawnDelay);
        spawnTimer = false;

    }
}
