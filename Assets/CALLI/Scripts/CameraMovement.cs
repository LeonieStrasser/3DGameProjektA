using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    WhingMovement01 playerMovement;

    public float distanceToPlayer = 0.2f;
    public float focalPointCamera = 20.0f;
    enum cameraState
    {
        normal,
        nosedive
    }
    bool switchPhase;

    [SerializeField] cameraState currentState;

    Quaternion currentRotation;

    Quaternion downRotation;

    public float downRotationSpeed;
    [SerializeField] float camSwitchSpeed = 100f;
    [SerializeField] [Tooltip("Abweichungswinkel vom Kamera LookAt auf den Player " +
        "Fixpunkt bei dem die Kamera wieder in ihr normales Behaviour wechselt")] float snapBackValue = 1;



    private void Awake()
    {
        playerMovement = FindObjectOfType<WhingMovement01>();
        playerMovement.OnDeadzoneValueChanged += ChangeCamera;
    }

    private void Start()
    {
        currentState = cameraState.normal;
        downRotation = Quaternion.identity;
        downRotation.x = 1;

    }

    void FixedUpdate()
    {
        if (currentState == cameraState.normal)
        {
            NormalMovement();
        }
        else if (currentState == cameraState.nosedive)
        {
            Nosedive();
        }
    }

    void Nosedive()
    {
        // Camera Position
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

        // Camera Rotation
        downRotation.y = 0;
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, downRotation, downRotationSpeed);

    }
    void NormalMovement()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * focalPointCamera);


        // Camera Position
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

        // Camera Rotation
        if (!switchPhase)
        {
            Camera.main.transform.LookAt(transform.position + transform.forward * focalPointCamera);
        }
        else
        {
            Vector3 targetDirection = (transform.position + transform.forward * focalPointCamera) - Camera.main.transform.position;

            Quaternion rotationToTarget = Quaternion.LookRotation(targetDirection, Vector3.up);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToTarget, camSwitchSpeed);

            //Wenn die Cam wieder in die richtige Richtung guckt, snapp wieder direkt auf den fixpunkt
            if(Vector3.Angle(targetDirection, Camera.main.transform.forward)< snapBackValue)
            {
                switchPhase = false;
            }
        }
    }

    void ChangeCamera(bool up, bool down)
    {
        currentRotation = Camera.main.transform.rotation;
        if (currentState == cameraState.nosedive)
        {
            currentState = cameraState.normal;
            switchPhase = true;
        }
        else
        {
            currentState = cameraState.nosedive;
        }
    }
}

