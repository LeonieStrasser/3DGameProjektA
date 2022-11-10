using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransformTaker : MonoBehaviour
{
    public GameObject otherObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 newRot = new Vector3 (otherObject.transform.eulerAngles.x, otherObject.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler (newRot);
    }
}
