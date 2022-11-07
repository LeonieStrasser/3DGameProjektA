using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_Path : MonoBehaviour
{
    
    public Transform GetWaypoint (int waypointIndex) //F�r Waypoint mit spezifischen Index
    {
        return transform.GetChild(waypointIndex); //um Zugriff auf die Child Waypoints zu bekommen
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1; // damit er Index hoch gez�hlt wird

        if (nextWaypointIndex == transform.childCount) 
        {
            nextWaypointIndex = 0; // wieder auf den ersten Punkt zur�ck setzen
        }

        return nextWaypointIndex; //Script erneut durch laufen lassen
    }

}
