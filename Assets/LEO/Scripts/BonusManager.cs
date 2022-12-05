using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusManager : MonoBehaviour
{
    public event Action<ObjectType.type> OnActivateMoving;

    
  public void ActivateMovingObject(ObjectType.type typeToActivate)
    {
        OnActivateMoving?.Invoke(typeToActivate);
    }
}
