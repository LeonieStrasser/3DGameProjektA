using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamsPlayer : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] CinemachineVirtualCamera secondCam;
    private WhingMovement01 wM01;

    public static CinemachineVirtualCamera ActiveCamera = null;

    private void Start()
    {
        wM01.OnDeadzoneValueChanged += CamSwap;
    }

    private void OnDestroy()
    {
        wM01.OnDeadzoneValueChanged -= CamSwap;
    }

    private void Awake()
    {
       wM01 = GetComponent<WhingMovement01>();
    }

    private void OnEnable()
    {
        CameraSwitcher.Register(mainCam);
        CameraSwitcher.Register(secondCam);
        CameraSwitcher.SwitchCamera(mainCam);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(mainCam);
        CameraSwitcher.Unregister(secondCam);
    }

    private void CamSwap(bool lastUpBool, bool lastDownBool)
    {
        if(CameraSwitcher.IsActiveCamera(mainCam))
            {
                CameraSwitcher.SwitchCamera(secondCam);
                if(wM01.flipControls == true)
                {
                    wM01.flipControls = false;
                }
            }
            else if(CameraSwitcher.IsActiveCamera(secondCam))
            {
                CameraSwitcher.SwitchCamera(mainCam);
                if(wM01.flipControls == false)
                {
                    wM01.flipControls = true;
                }
            }
    }

/*
    private void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(CameraSwitcher.IsActiveCamera(mainCam))
            {
                CameraSwitcher.SwitchCamera(secondCam);
                if(wM01.flipControls == true)
                {
                    wM01.flipControls = false;
                }
            }
            else if(CameraSwitcher.IsActiveCamera(secondCam))
            {
                CameraSwitcher.SwitchCamera(mainCam);
                if(wM01.flipControls == false)
                {
                    wM01.flipControls = true;
                }
            }
        }
    }
*/
}
