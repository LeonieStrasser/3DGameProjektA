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
    [Tooltip("Faktor um den sich der Flugkörper verschnellert, wenn er abwärts fliegt. Selber Faktor um den sich der Flugkörper verlangsamt, wenn er aufwärts fliegt. ")] // eigentlich wär es sinnvoll wenn der faktor aufwärts größer ist - Wegen der Schwerkraft
    [SerializeField] float fallVelocity = 90f;
    [Tooltip("Maximal erreichbarer Speed im Sturzflug.")]
    [SerializeField] float maxSpeed = float.MaxValue;
    [Tooltip("Geschwindigkeit mit der der Flieger an der x-Achse rotiert.")]
    [SerializeField] [Range(10, 400)] float rotationSpeedUpDown;
    [Tooltip("Geschwindigkeit mit der der Flieger an der y-Achse rotiert.")]
    [SerializeField] [Range(10, 400)] float rotationSpeedLeftRight;
    [Tooltip("Geschwindigkeit mit der der Flieger an der z-Achse rotiert rotiert.")]
    [SerializeField] [Range(0, 10)] float stabilizeSpeed;
    [Tooltip("Kraft mit der der Flugkörper richtung Boden gedrückt wird.")]
    [SerializeField] [Range(0, 0.1f)] float gravity;
    [Tooltip("Geschwindigkeits Obergrenze ab der die Kraft nach Unten anfängt zu wirken. (Von da an wirkt sie umso stärker, je langsamer das Flugobjekt wird)")]
    [SerializeField] float gravitySpeedBoundery = 20f;
    [Space(10)]
    [Tooltip("Sensitivität für den Joystick Input.")]
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
    float rightControlY;

    Vector2 lefttWhingControlStick;
    float lefttControlY;

    float currentRotationUpDown;

    float currentRotationLeftRight;

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
        Debug.Log("Speed: " + currentSpeed);

        Move();
        //AddGravity();
        RotateWhings();
    }

    void OnRightWhing(InputValue value)                                                             // Inputs vom rechten Joystick werden ausgelesen
    {
        rightWhingControlStick = value.Get<Vector2>();
        rightControlY = -rightWhingControlStick.y;

        if (rightControlY < inputSensitivity && rightControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            rightControlY = 0;
        }
    }

    void OnLeftWhing(InputValue value)                                                              // Inputs vom linken Joystick werden ausgelesen
    {
        lefttWhingControlStick = value.Get<Vector2>();
        lefttControlY = -lefttWhingControlStick.y;

        if (lefttControlY < inputSensitivity && lefttControlY > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            lefttControlY = 0;
        }
    }

    private void Move()
    {
        //// Speed wird schneller und langsamer je nach Blickruchtung hoch oder Runter
        //currentSpeed += fallVelocity * -transform.forward.y * Time.deltaTime;
        //currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);                                        // Beschläunigt nur bis zum Maximalspeed 

        //// Vorwärtsbewegung
        //transform.position += transform.forward * currentSpeed * Time.deltaTime;

        ////Rotation hoch und runter
        //currentRotationUpDown = rotationSpeedUpDown * (rightControlY / 2 + lefttControlY / 2);
        //transform.RotateAround(transform.position, transform.right, Time.deltaTime * currentRotationUpDown);

        ////Rotation rechts und links
        //currentRotationLeftRight = (rightControlY - lefttControlY) / 2;
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * currentRotationLeftRight * rotationSpeedLeftRight);

        //// Rotation an der Blickrichtung

        //Quaternion rightRotation = rightWhing.transform.rotation;                                    // Einen Mittelwert aus den Flügelvektoren berechnen
        //Quaternion leftRotation = leftWhing.transform.rotation;
        //Quaternion midRotation = Quaternion.Slerp(rightRotation, leftRotation, 0.5f);

        //transform.rotation = Quaternion.Lerp(transform.rotation, midRotation, stabilizeSpeed * Time.deltaTime);     // Aktuelle Rotation an der z Achse richtung des Mittelwerts anpassen - !!!Hier ist noch was nicht ganz richtig am Start




        // Speed wird schneller und langsamer je nach Blickruchtung hoch oder Runter
        currentSpeed = (myRigidbody.velocity.magnitude) + (fallVelocity * -transform.forward.y * Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);                                        // Beschläunigt nur bis zum Maximalspeed 

        // Vorwärtsbewegung
        myRigidbody.position += transform.forward * currentSpeed * Time.deltaTime;

        //Rotation hoch und runter
        currentRotationUpDown = rotationSpeedUpDown * (rightControlY / 2 + lefttControlY / 2);
        myRigidbody.transform.RotateAround(transform.position, transform.right, Time.deltaTime * currentRotationUpDown);

        //Rotation rechts und links
        currentRotationLeftRight = (rightControlY - lefttControlY) / 2;
        myRigidbody.transform.RotateAround(transform.position, transform.up, Time.deltaTime * currentRotationLeftRight * rotationSpeedLeftRight);

        // Rotation an der Blickrichtung

        Quaternion rightRotation = rightWhing.transform.rotation;                                    // Einen Mittelwert aus den Flügelvektoren berechnen
        Quaternion leftRotation = leftWhing.transform.rotation;
        Quaternion midRotation = Quaternion.Slerp(rightRotation, leftRotation, 0.5f);

        myRigidbody.transform.rotation = Quaternion.Lerp(transform.rotation, midRotation, stabilizeSpeed * Time.deltaTime);     // Aktuelle Rotation an der z Achse richtung des Mittelwerts anpassen - !!!Hier ist noch was nicht ganz richtig am Start
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
        switch (rightControlY)                                                                      // Je nach Input des Joysticks wird ein anderer Ziel - Vektor für den Flügel eingesetzt
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
        switch (lefttControlY)                                                                      // Je nach Input des Joysticks wird ein anderer Ziel - Vektor für den Flügel eingesetzt
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

}
