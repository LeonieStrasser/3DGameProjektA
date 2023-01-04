using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectType
{
    public enum type
    {
        zahnradKlein,
        zahnradGro�,
        drehK�rbis,
        kleinerCube,
        none
    }
}

[System.Serializable]
public class BonusType
{
    public enum bonusType
    {
        addFuel,
        scoreMultiplyer,
        moveObject
    }
}
