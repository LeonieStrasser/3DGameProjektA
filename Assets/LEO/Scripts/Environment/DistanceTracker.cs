using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceTracker : MonoBehaviour
{
    [SerializeField] [Range(0, 0.5f)] float spawnDelay;
    [SerializeField] SphereCollider[] distanceTrigger;
    [Tooltip("WInkel in dem von der Fl�gelrichtung aus EdgeVFXe noch gespawnt werden.")]
    [SerializeField] [Range(90, 180)] float detectionAngle;
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
        foreach (var item in distanceTrigger)
        {
            item.radius = detectionRadius;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger && other.tag != "Player")
        {
            if (!spawnTimer)
                foreach (var item in distanceTrigger)
                {

                    Vector3 closestPoint = other.ClosestPoint(item.gameObject.transform.position);

                    float distanceToDetector = Vector3.Distance(item.transform.position, closestPoint);
                    if (distanceToDetector > 0.001) // Stellt sicher dass der Closest point funktioniert hat - im fail-Fall ist der Closest Point der selbe wie this.position
                    {
                        StartCoroutine(vfxSpawnTimer(closestPoint, distanceToDetector, item.transform.position));
                    }
                    else
                    {
                        Debug.LogWarning("Tryed to spawn a Distance-VFX on the player! Da war wahrscheinlich ein Mesh Collider schuld! Objektname: " + other.gameObject.name);
                    }
                }
        }



    }

    IEnumerator vfxSpawnTimer(Vector3 spawnPosition, float distanceToCenter, Vector3 detectorPosition)
    {
        if (CheckDirectionAngle(spawnPosition, detectorPosition))
        {
            spawnTimer = true;
            DistanceEffect(distanceToCenter, spawnPosition);
            yield return new WaitForSeconds(spawnDelay);
            spawnTimer = false;
        }
    }

    private void DistanceEffect(float distance, Vector3 spawnPosition)
    {
        float exaktDistanceMultiplyer = 1 - (Mathf.Clamp(distance, 0, detectionRadius) / detectionRadius); // Je k�rzer die Distanz desto h�her der Wert von 0 bis 1

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

    private bool CheckDirectionAngle(Vector3 spawnPoint, Vector3 detectorPosition)
    {
        Vector3 playerSpawnDir = (spawnPoint - detectorPosition).normalized;
        Vector3 playerDetectorDir = (detectorPosition - this.transform.position).normalized;
        Vector3 leftRightDirection;




        if (Vector3.Angle(this.transform.right, playerDetectorDir) < 90)
        {
            leftRightDirection = transform.right;
        }
        else
        {
            leftRightDirection = -transform.right;
        }
        float angleBetweenSpawnAndDetectorDir = Vector3.Angle(playerSpawnDir, leftRightDirection);

        Debug.DrawLine(transform.position, detectorPosition, Color.blue);                                   // DEBUG DRAWLINES!!!!!
        Debug.DrawRay(detectorPosition, leftRightDirection * 10, Color.red);

        if (angleBetweenSpawnAndDetectorDir < detectionAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
