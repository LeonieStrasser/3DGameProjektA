using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaceEffekte", menuName = "Costum/ComboState")]
public class ComboState : ScriptableObject
{
    public float maxTimerTime;
    public float neededPointsToNextState;
    public float stateMultiplyer;

    [Header("Feedback")]
    public Color scoreColor;


}
