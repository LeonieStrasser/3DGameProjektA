using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[CreateAssetMenu(fileName = "RaceEffekte", menuName = "Costum/RaceEffekte")]
public class ScriptableObjectEffekte : ScriptableObject
{
    public Sprite effectSprite;
    public string effectName;
    public string effectDescription;
    public int spawnChance;
    public BonusType.bonusType bonusType;
    public bool singleUse;

    [Header("BonusManager Variables")]
    public ObjectType.type typeToActivate;
    [Tooltip("für Prozent 0.0x davor setzen!")] public float fuelVolume;
    public float scoreEffectTime;
    public float scoreMultiplyer;

    public ScriptableObjectEffekte(string effectName, int spawnChance)
    {
        this.effectName = effectName;
        this.spawnChance = spawnChance;
    }
}
