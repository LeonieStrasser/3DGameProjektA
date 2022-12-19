using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[CreateAssetMenu (fileName = "RaceEffekte", menuName = "Costum/RaceEffekte")]
public class ScriptableObjectEffekte : ScriptableObject
{
    public Sprite effectSprite;
    public string effectName;
    public string effectDescription;
    public int spawnChance;

    public ScriptableObjectEffekte(string effectName, int spawnChance)
    {
        this.effectName = effectName;
        this.spawnChance = spawnChance;
    }
}
