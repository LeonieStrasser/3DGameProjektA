using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// An add-on module for Cinemachine Virtual Camera that overrides Up

[SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class VcamOverride : CinemachineExtension
{
    [Tooltip("Use this object's local Up direction")]
    public Transform m_Up = null;
    
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && m_Up != null)
            state.ReferenceUp = m_Up.up;
    }
}

