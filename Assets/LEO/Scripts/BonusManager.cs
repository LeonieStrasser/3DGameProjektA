using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusManager : MonoBehaviour
{
    [SerializeField] ObjectType.type typeToActivate;
    [SerializeField] float addProcent;
    [SerializeField] float effectTime;
    [SerializeField] float multiplyer;

    WhingMovement01 myPlayer;

    public event Action<ObjectType.type> OnActivateMoving;
    public event Action<float> OnTimeEffectStart;
    public event Action<float> OnTimeEffectEnd;

    private void Awake()
    {
        myPlayer = FindObjectOfType<WhingMovement01>();
    }

    public void ActivateMovingObject()//(ObjectType.type typeToActivate)
    {
        OnActivateMoving?.Invoke(this.typeToActivate);
    }

    public void AddFuelVolume()//(float addProcent)
    {
        myPlayer.AddMaxRecourcePoints(this.addProcent);
    }

    public void StartPointMultiplyTime()// (float effectTime, float multiplyer)
    {
        OnTimeEffectStart?.Invoke(this.multiplyer);
        StartCoroutine(TimeEffect(this.effectTime));
    }


    IEnumerator TimeEffect(float effectTime)
    {
        yield return new WaitForSeconds(effectTime);
        OnTimeEffectEnd?.Invoke(1); // damit der Multiplyer wieder 1 ist nach Ablauf der Zeit
    }
}
