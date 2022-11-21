using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMvmnt : MonoBehaviour
{
    public float Xrange = 0f;  // Amount to move left and right from the start point
    public float Yrange = 0f;
    public float Zrange = 0f;
    public float speed = 2.0f; 
    private Vector3 startPos;
    private Vector3 v;

    
    
 
    void Start () 
    {
        startPos = transform.position;
    }
     
    void Update () 
    {
        v = startPos;
        v.x += Xrange * Mathf.Sin (Time.time * speed);
        v.y += Yrange * Mathf.Sin (Time.time * speed);
        v.z += Zrange * Mathf.Sin (Time.time * speed);
        transform.position = v;

    }
}
