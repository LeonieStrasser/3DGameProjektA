using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptActivator : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] behaviourToActivate;
    [SerializeField] ObjectType.type myObjectType;

    BonusManager myBonusManager;

    private void Awake()
    {
        myBonusManager = FindObjectOfType<BonusManager>();
    }
    private void Start()
    {
        myBonusManager.OnActivateMoving += ActivateCheck;

        foreach (var item in behaviourToActivate)
        {
            item.enabled = false;
        }
    }

    private void ActivateCheck(ObjectType.type typeToCheck)
    {
        if (myObjectType == typeToCheck)
        {
            foreach (var item in behaviourToActivate)
            {
                item.enabled = true;
            }
        }
    }

}
