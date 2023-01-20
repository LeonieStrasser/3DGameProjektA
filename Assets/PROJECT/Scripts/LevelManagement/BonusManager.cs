using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class BonusManager : MonoBehaviour
{



    WhingMovement01 myPlayer;

    public event Action<ObjectType.type> OnActivateMoving;
    public event Action<float> OnTimeEffectStart;
    public event Action<float> OnTimeEffectEnd;


    


    private void Awake()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();


    }

    public void ActivateBonusByKey(BonusType.bonusType bonusIndexKey, float fuelVolume, float scoreEffectTime, float scoreMultiplyer, ObjectType.type typeToActivate)
    {
        switch (bonusIndexKey)
        {
            default:
                break;
            case BonusType.bonusType.addFuel:
                AddFuelVolume(fuelVolume);
                break;
            case BonusType.bonusType.scoreMultiplyer:
                StartPointMultiplyTime(scoreEffectTime, scoreMultiplyer);
                break;
            case BonusType.bonusType.moveObject:
                ActivateMovingObject(typeToActivate);
                break;
        }
    }

    private void AddFuelVolume(float addProcent)
    {
        myPlayer.AddMaxRecourcePoints(addProcent);

        Debug.Log("AddFuelVolume ist Aktiv");
    }


    private void StartPointMultiplyTime(float effectTime, float multiplyer)
    {
        OnTimeEffectStart?.Invoke(multiplyer);
        StartCoroutine(TimeEffect(effectTime));

        Debug.Log("StartPointMultiplyTime ist Aktiv");
    }

    
    private void ActivateMovingObject(ObjectType.type typeToActivate)
    {
        OnActivateMoving?.Invoke(typeToActivate);

        Debug.Log("ActivateMovingObject ist Aktiv");

    }




    IEnumerator TimeEffect(float effectTime)
    {
        yield return new WaitForSeconds(effectTime);
        OnTimeEffectEnd?.Invoke(1); // damit der Multiplyer wieder 1 ist nach Ablauf der Zeit

        Debug.Log("Zeit Abgelaufen - hehe");

    }


    [SerializeField] ObjectType.type debugMoveType;
    [Button]
    public void DebugMoveObjects()
    {
        ActivateMovingObject(debugMoveType);
    }
}
