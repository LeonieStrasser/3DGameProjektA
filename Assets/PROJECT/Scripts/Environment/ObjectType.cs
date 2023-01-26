using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectType
{
    public enum type
    {
        zahnrad,
        drehenderKran,
        kralle,
        müllpresse,
        cube,
        windmill,
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
