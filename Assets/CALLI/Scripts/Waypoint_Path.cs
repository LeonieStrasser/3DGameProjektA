using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_Path : MonoBehaviour
{
    
    public Transform GetWaypoint (int waypointIndex) //Für Waypoint mit spezifischen Index
    {
        return transform.GetChild(waypointIndex); //um Zugriff auf die Child Waypoints zu bekommen
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1; // damit er Index hoch gezählt wird

        if (nextWaypointIndex == transform.childCount) 
        {
            nextWaypointIndex = 0; // wieder auf den ersten Punkt zurück setzen
        }

        return nextWaypointIndex; //Script erneut durch laufen lassen
    }

}
