    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
     
    public class CamFixDingScript : MonoBehaviour
    {
        public Camera Camera;
        public Transform Target;
        public float Damping;
        public Vector3 ScreenSpaceOffset;
     
        Vector3 m_CurrentVelocity;
        Vector3 m_DampedPos;
     
        void OnEnable()
        {
            if (Camera == null)
                Camera = Camera.main;
            if (Target != null)
                m_DampedPos = Target.position;
        }
     
        void LateUpdate()
        {
            if (Target != null)
            {
                var pos = Target.position;
                m_DampedPos = Damping < 0.01f
                    ? pos : Vector3.SmoothDamp(m_DampedPos, pos, ref m_CurrentVelocity, Damping);
                pos = m_DampedPos;
                if (Camera != null)
                {
                    pos = Camera.transform.worldToLocalMatrix * pos;
                    pos += ScreenSpaceOffset;
                    pos = Camera.transform.localToWorldMatrix * pos;
                }
                transform.position = pos;
            }
        }
    }
