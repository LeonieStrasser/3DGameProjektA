using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMvmnt : MonoBehaviour
{
    public Transform target;
    public float t;
   private void FixedUpdate() 
   {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.Lerp(a, b, t);
   }
}
