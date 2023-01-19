using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectType
{
    public enum type
    {
        zahnradKlein,
        zahnradGroß,
        drehKürbisKlein,
        drehKürbisMittel,
        kleinerCube,
        drehenderKran,
        zahnradBiggest,
        zahnradMittel,
        kralle,
        mobileKralle,
        müllpresse,
        pillarKlein,
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
