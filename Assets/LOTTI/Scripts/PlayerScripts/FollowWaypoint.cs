using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    /*public BoostWP CurrentWaypoint;
    public float moveSpeed = 70;
    private void Start() 
    {
        if(CurrentWaypoint != null)
        {
            return;
        }
        BoostWP[] waypoints = FindObjectsOfType<BoostWP>();

        float lastMinDistance = float.MaxValue;
        foreach (var wp in waypoints)
        {
            float distance = Vector3.Distance(transform.position, wp.transform.position);
            if(distance < lastMinDistance)
            {
                CurrentWaypoint = wp;
                lastMinDistance = distance;
            }
        }
    }

    private void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentWaypoint.transform.position, moveSpeed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < 0.05f)
            {
                Debug.Log("Moving On");
                CurrentWaypoint = CurrentWaypoint.Nextwaypoint;
            }
    }*/

}
