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
        nosedive,
        skyDive
    }
    bool switchPhase;

    [SerializeField] cameraState currentState;

    Quaternion currentRotation;


    public float diveRotationSpeed;
    [SerializeField] float camSwitchSpeed = 100f;
    [SerializeField] float switchTime = 1f;
    [SerializeField] AnimationCurve switchSpeedCurve;
    [SerializeField]
    [Tooltip("Abweichungswinkel vom Kamera LookAt auf den Player " +
        "Fixpunkt bei dem die Kamera wieder in ihr normales Behaviour wechselt")]
    float snapBackValue = 1;
    float elapsedSwitchPhaseTime = 0;


    private void Awake()
    {
        playerMovement = FindObjectOfType<WhingMovement01>();
        playerMovement.OnDeadzoneValueChanged += ChangeCamera;
    }

    private void Start()
    {
        currentState = cameraState.normal;

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
        else if (currentState == cameraState.skyDive)
        {
            SkyDive();
        }
    }

    void Nosedive()
    {
        // Camera Position
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

        // Camera Rotation

        Vector3 directionDown = Vector3.down;
        Vector3 directionUpWhileDive = Vector3.Cross(directionDown, Camera.main.transform.right); // Rechnet aus down und camera.right den up Vektor aus den die Kamera von hioer haben müsste
        //Debug.DrawRay(transform.position, directionDown * 100f, Color.red);
        //Debug.DrawRay(transform.position, directionUpWhileDive * 100f, Color.blue);

        Quaternion rotationToDown = Quaternion.LookRotation(directionDown, directionUpWhileDive); // rechnet die richtige Zielrotation aus
       
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToDown, diveRotationSpeed); // Dreht die cam von ihrer ausgangsrotation über speed auf die Zielrotation

    }

    void SkyDive()
    {
        // Camera Position
        Vector3 moveCamTo = transform.position - transform.forward * distanceToPlayer + Vector3.up * 2.0f;
        float bias = 0.84f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

        // Camera Rotation

        Vector3 directionForwardUp = Vector3.up;
        Vector3 directionUpWhileDive = Vector3.Cross(directionForwardUp, Camera.main.transform.right);
        Debug.DrawRay(transform.position, directionForwardUp * 100f, Color.red);
        Debug.DrawRay(transform.position, directionUpWhileDive * 100f, Color.blue);

        Quaternion rotationToUp = Quaternion.LookRotation(directionForwardUp, directionUpWhileDive);

        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToUp, diveRotationSpeed);
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
            elapsedSwitchPhaseTime += Time.fixedDeltaTime;

            Vector3 targetDirection = (transform.position + transform.forward * focalPointCamera) - Camera.main.transform.position;

            Quaternion rotationToTarget = Quaternion.LookRotation(targetDirection, Vector3.up);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToTarget, switchSpeedCurve.Evaluate(elapsedSwitchPhaseTime / switchTime) * camSwitchSpeed);

            //Wenn die Cam wieder in die richtige Richtung guckt, snapp wieder direkt auf den fixpunkt
            if (Vector3.Angle(targetDirection, Camera.main.transform.forward) < snapBackValue)
            {
                switchPhase = false;
            }
        }
    }

    void ChangeCamera(bool up, bool down)
    {
        Debug.Log("up is: " + up.ToString());
        currentRotation = Camera.main.transform.rotation;
        if (!up && !down)
        {
            currentState = cameraState.normal;
            switchPhase = true;
            elapsedSwitchPhaseTime = 0;
        }
        else if(down)
        {
            currentState = cameraState.nosedive;
        }
        else
        {
            currentState = cameraState.skyDive;
        }
    }
}

