using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WhingMovement01 : MonoBehaviour
{
    [Header("Movement")]
    [Space(10)]

    [Tooltip("Speed der am Start gesetzt wird.")]
    [SerializeField] float startSpeed = 10f;
    [Tooltip("Faktor um den sich der Flugk�rper verschnellert, wenn er abw�rts fliegt. Selber Faktor um den sich der Flugk�rper verlangsamt, wenn er aufw�rts fliegt. ")] // eigentlich w�r es sinnvoll wenn der faktor aufw�rts gr��er ist - Wegen der Schwerkraft
    [SerializeField] float fallVelocity = 90f;

    [SerializeField] float glideUpVelocity;

    [Tooltip("Maximal erreichbarer Speed im Sturzflug.")]
    [SerializeField] float maxSpeed = float.MaxValue;
    [Tooltip("Geschwindigkeit mit der der Flieger an der x-Achse rotiert.")]
    [SerializeField] [Range(10, 400)] float rotationSpeedUpDown;
    [Tooltip("Geschwindigkeit mit der der Flieger an der y-Achse rotiert.")]
    [SerializeField] [Range(10, 400)] float rotationSpeedLeftRight;
    [Tooltip("Geschwindigkeit mit der der Flieger an der z-Achse rotiert rotiert.")]
    [SerializeField] [Range(0, 80)] float stabilizeSpeed;
    [Tooltip("Kraft mit der der Flugk�rper richtung Boden gedr�ckt wird.")]
    [SerializeField] [Range(0, 0.1f)] float gravity;
    [Tooltip("Geschwindigkeits Obergrenze ab der die Kraft nach Unten anf�ngt zu wirken. (Von da an wirkt sie umso st�rker, je langsamer das Flugobjekt wird)")]
    [SerializeField] float gravitySpeedBoundery = 20f;
    [Space(10)]
    [Tooltip("Sensitivit�t f�r den Joystick Input.")]
    [SerializeField] [Range(0, 0.5f)] float inputSensitivity;

    [Space(20)]
    [Header("Whing Animation")]
    [Space(10)]
    [SerializeField] GameObject rightWhing;
    [SerializeField] GameObject leftWhing;
    [SerializeField] float whingRotationSpeed = 10f;
    [SerializeField] float maxRotation;
    [SerializeField] float minRotation;
    [SerializeField] float neutralRotation;

    private Rigidbody myRigidbody;

    float currentSpeed;


    Vector2 rightWhingControlStick;
    float rightControlX;
    float rightControlY;

    Vector2 lefttWhingControlStick;
    float lefttControlX;
    float lefttControlY;

    float currentRotationUpDown;

    float currentRotationLeftRight;

    float currentRotationForward;

    Quaternion downRotation;


    // Animation
    Quaternion currentRightRotationTarget;
    Quaternion currentLeftRotationTarget;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentSpeed = startSpeed;                                                                  // Startspeed wird gesetzt
        downRotation = Quaternion.identity;
        downRotation.x = 1;
    }
    private void Update()
    {
       // Debug.Log("Speed: " + currentSpeed);

        Move();
        //AddGravity();
        RotateWhings();
    }

    void OnRightWhing(InputValue value)                                                             // Inputs vom rechten Joystick werden ausgelesen
    {
        rightWhingControlStick = value.Get<Vector2>();
        rightControlX = rightWhingControlStick.x;
        rightControlY = -rightWhingControlStick.y;

        if (rightControlY < inputSensitivity && rightControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            rightControlY = 0;
        }

        if (rightControlX < inputSensitivity && rightControlX > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            rightControlX = 0;
        }
    }

    void OnLeftWhing(InputValue value)                                                              // Inputs vom linken Joystick werden ausgelesen
    {
        lefttWhingControlStick = value.Get<Vector2>();
        lefttControlX = -lefttWhingControlStick.x;
        lefttControlY = -lefttWhingControlStick.y;

        if (lefttControlY < inputSensitivity && lefttControlY > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            lefttControlY = 0;
        }

        if (lefttControlX < inputSensitivity && lefttControlX > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            lefttControlX = 0;
        }
    }

    private void Move()
    {
        if (transform.forward.y < 0)
        {
            // Speed wird schneller und langsamer je nach Blickruchtung hoch oder Runter
            currentSpeed = (myRigidbody.velocity.magnitude) + (fallVelocity * -transform.forward.y);
        }
        else
        {
            currentSpeed = currentSpeed - (glideUpVelocity * transform.forward.y);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);                                        // Beschl�unigt nur bis zum Maximalspeed 

        // Vorw�rtsbewegung
        //myRigidbody.position += transform.forward * currentSpeed * Time.deltaTime;
        myRigidbody.AddForce(transform.forward * currentSpeed);



        //Rotation hoch und runter
        currentRotationUpDown = rotationSpeedUpDown * (rightControlY / 2 + lefttControlY / 2);
        Quaternion deltaXRotation = Quaternion.Euler(new Vector3(currentRotationUpDown, 0, 0) * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaXRotation);

        //Rotation rechts und links
        currentRotationLeftRight = rotationSpeedLeftRight * ((rightControlX - lefttControlX) / 2);
        Quaternion deltaYRotation = Quaternion.Euler(new Vector3(0, currentRotationLeftRight, 0) * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaYRotation);

        // Rotation an der Blickrichtung
        currentRotationForward = stabilizeSpeed * ((rightControlY - lefttControlY) / 2);
        Quaternion deltaZRotation = Quaternion.Euler(new Vector3(0, 0, -currentRotationForward) * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaZRotation);



        //Quaternion rightRotation = rightWhing.transform.rotation;                                    // Einen Mittelwert aus den Fl�gelvektoren berechnen
        //Quaternion leftRotation = leftWhing.transform.rotation;
        //Quaternion midRotation = Quaternion.Slerp(rightRotation, leftRotation, 0.5f);


        //myRigidbody.transform.rotation = Quaternion.Lerp(transform.rotation, midRotation, stabilizeSpeed * Time.deltaTime);     // Aktuelle Rotation an der z Achse richtung des Mittelwerts anpassen - !!!Hier ist noch was nicht ganz richtig am Start
    }

    //private void AddGravity()
    //{
    //    if (currentSpeed < gravitySpeedBoundery)
    //    {
    //        float slowMultoplyer = 1 - (Mathf.InverseLerp(0, gravitySpeedBoundery, currentSpeed));
    //        float t = Time.deltaTime * gravity * gravitySpeedBoundery;

    //        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, t);
    //    }
    //}

    private void RotateWhings()
    {
        // right Whing
        Quaternion newTargetRotationRight = Quaternion.identity;
        switch (rightControlY)                                                                      // Je nach Input des Joysticks wird ein anderer Ziel - Vektor f�r den Fl�gel eingesetzt
        {
            case < 0:
                newTargetRotationRight.z = maxRotation;
                break;
            case 0:
                newTargetRotationRight.z = neutralRotation;
                break;
            case > 0:
                newTargetRotationRight.z = minRotation;
                break;
            default:
                break;
        }

        currentRightRotationTarget = newTargetRotationRight;

        // Right Whing Rotate in Target Direction over Time
        rightWhing.transform.localRotation = Quaternion.Lerp(rightWhing.transform.localRotation, currentRightRotationTarget, whingRotationSpeed * Time.deltaTime);


        // left Whing
        Quaternion newTargetRotationLeft = Quaternion.identity;
        switch (lefttControlY)                                                                      // Je nach Input des Joysticks wird ein anderer Ziel - Vektor f�r den Fl�gel eingesetzt
        {
            case < 0:
                newTargetRotationLeft.z = -maxRotation;
                break;
            case 0:
                newTargetRotationLeft.z = -neutralRotation;
                break;
            case > 0:
                newTargetRotationLeft.z = -minRotation;
                break;
            default:
                break;
        }
        currentLeftRotationTarget = newTargetRotationLeft;

        // left Whing Rotate in Target Direction over Time
        leftWhing.transform.localRotation = Quaternion.Lerp(leftWhing.transform.localRotation, currentLeftRotationTarget, whingRotationSpeed * Time.deltaTime);
    }
      private void OnTriggerEnter(Collider other) //Triggert Boost wenn Obj Booster Collider berührt
    {
        if(other.gameObject.tag == "Booster")
        {
            Debug.Log("BOOST!" + currentSpeed);
            currentSpeed = currentSpeed * 2;
        }
    }


}
