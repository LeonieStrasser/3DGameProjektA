using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] Material newColor;
    [SerializeField] MeshRenderer[] meshesToChange;
    [SerializeField] GameObject spawnVFX;
    [SerializeField] float lifetimeElectricity;

    private void OnTriggerEnter(Collider other)
    {
        ChangeColor(newColor);
    }

    public void ChangeColor(Material newMaterial)
    {
        foreach (var item in meshesToChange)
        {
            item.material = newMaterial;
        }
        Destroy(Instantiate(spawnVFX, this.transform.position, this.transform.rotation), lifetimeElectricity);
    }
}
