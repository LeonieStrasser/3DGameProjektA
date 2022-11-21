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

    [SerializeField] cameraState currentState;

    Quaternion currentRotation;

    Quaternion downRotation;

    public float downRotationSpeed;



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
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        downRotation.y = 0;
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, downRotation, downRotationSpeed);

    }
    void NormalMovement()
    {
        //Camera Movement
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * focalPointCamera);
    }

    void ChangeCamera(bool up, bool down)
    {
        currentRotation = Camera.main.transform.rotation;
        if (currentState == cameraState.nosedive)
        {
            currentState = cameraState.normal;
            //Camera.main.transform.position = this.transform.position;
        }
        else
        {
            currentState = cameraState.nosedive;
        }
    }
}

