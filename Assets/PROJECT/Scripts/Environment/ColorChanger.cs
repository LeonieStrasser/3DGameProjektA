using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] Material newColor;
    [SerializeField] MeshRenderer[] meshesToChange;
    [SerializeField] GameObject spawnVFX;

    private void OnTriggerEnter(Collider other)
    {
        ChangeColor(newColor);
        Instantiate(spawnVFX);
    }

    public void ChangeColor(Material newMaterial)
    {
        foreach (var item in meshesToChange)
        {
            item.material = newMaterial;
        }
    }
}
