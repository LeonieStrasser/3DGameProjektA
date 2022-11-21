using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] Color newColor;
    Material myMaterial;
    private void Awake()
    {
        myMaterial = GetComponent<MeshRenderer>().material;   
    }
    private void OnTriggerEnter(Collider other)
    {
        myMaterial.color = newColor;
    }
}
