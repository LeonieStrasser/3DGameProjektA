using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    [SerializeField] 
    private Waypoint_Path _waypointPath;

    [SerializeField] // Float Field um die Geschwindigkeit einzustellen, mit der sich die Platform bewegt
    private float _speed;

    private int _targetWaypointIndex; // Hold the index of the waypoint the platform is moving towards

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
