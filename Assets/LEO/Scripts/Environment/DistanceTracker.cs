using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceTracker : MonoBehaviour
{
    [SerializeField] [Range(0, 0.5f)] float spawnDelay;
    [SerializeField] SphereCollider distanceTrigger;
    [SerializeField] int points;
    [Space(10)]
    [SerializeField] bool activateExaktDistanceMultiplyer = false;

    [Space(20)]
    [Header("Distance States")]
    [SerializeField] float detectionRadius;
    [SerializeField] float maxValueMediumDistance;
    [SerializeField] float maxValueCloseDistance;
    [SerializeField] float multiplyerMediumZone;
    [SerializeField] float multiplyerCloseZone;
    [SerializeField] GameObject spawnVFX;
    [SerializeField] GameObject spawnVFXMediumState;
    [SerializeField] GameObject spawnVFXCloseState;

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

            float distanceToDetector = Vector3.Distance(distanceTrigger.transform.position, closestPoint);
            if (distanceToDetector > 0.001) // Stellt sicher dass der Closest point funktioniert hat - im fail-Fall ist der Closest Point der selbe wie this.position
            {
                StartCoroutine(vfxSpawnTimer(closestPoint, distanceToDetector));
            }
            else
            {
                Debug.LogWarning("Tryed to spawn a Distance-VFX on the player! Da war wahrscheinlich ein Mesh Collider schuld! Objektname: " + other.gameObject.name);
            }
        }



    }

    IEnumerator vfxSpawnTimer(Vector3 spawnPosition, float distanceToCenter)
    {
        spawnTimer = true;
        DistanceEffect(distanceToCenter, spawnPosition);
        yield return new WaitForSeconds(spawnDelay);
        spawnTimer = false;

    }

    private void DistanceEffect(float distance, Vector3 spawnPosition)
    {
        float exaktDistanceMultiplyer = 1 - (Mathf.Clamp(distance, 0, detectionRadius) / detectionRadius); // Je kürzer die Distanz desto höher der Wert von 0 bis 1

        int roundedPoints = points;
        if (activateExaktDistanceMultiplyer)
            roundedPoints = Mathf.RoundToInt(points * exaktDistanceMultiplyer);

        Debug.Log(distance.ToString());

        if (distance > maxValueMediumDistance)
        {
            // weite Distanz VFX

            Instantiate(spawnVFX, spawnPosition, Quaternion.identity);
            ScoreSystem.Instance.AddScore(roundedPoints);
        }
        else
         if (distance < maxValueMediumDistance && distance > maxValueCloseDistance)
        {
            // mittlere Distanz VFX

            Instantiate(spawnVFXMediumState, spawnPosition, Quaternion.identity);
            ScoreSystem.Instance.AddScore(Mathf.RoundToInt(roundedPoints * multiplyerMediumZone));
        }
        else
        if (distance < maxValueCloseDistance)
        {
            // clostest Distanz VFX

            Instantiate(spawnVFXCloseState, spawnPosition, Quaternion.identity);
            ScoreSystem.Instance.AddScore(Mathf.RoundToInt(roundedPoints * multiplyerCloseZone));
        }
    }
}
