using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScriptActivator : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] activateScripts;

    private void Start()
    {
       

        if (activateScripts.Length > 0)
        {
            foreach (var item in activateScripts)
            {
                item.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (activateScripts.Length > 0)
            {
                foreach (var item in activateScripts)
                {
                    item.enabled = true;
                }
            }
        }
    }
}
