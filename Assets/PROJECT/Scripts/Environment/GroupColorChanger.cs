using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupColorChanger : MonoBehaviour
{
    [SerializeField] ColorChanger[] allLamps;
    [SerializeField] Material newMaterial;
    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in allLamps)
        {
            item.ChangeColor(newMaterial);
        }
    }
}
