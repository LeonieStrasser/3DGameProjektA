using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamsPlayer : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] CinemachineVirtualCamera secondCam;

    public static CinemachineVirtualCamera ActiveCamera = null;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(CameraSwitcher.IsActiveCamera(mainCam))
            {
                CameraSwitcher.SwitchCamera(secondCam);
            }
            else if(CameraSwitcher.IsActiveCamera(secondCam))
            {
                CameraSwitcher.SwitchCamera(mainCam);
            }
        }
    }
}
