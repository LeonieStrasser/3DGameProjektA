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

    [SerializeField] float glideUpVelocity;

    [Tooltip("Minimaler Speed beim Hochfliegen.")]
    [SerializeField] float minSpeed = 100f;
    [Tooltip("Maximal erreichbarer Speed im Sturzflug.")]
    [SerializeField] float maxSpeed = float.MaxValue;
    [Tooltip("Geschwindigkeit mit der der Flieger an der x-Achse rotiert.")]
    [SerializeField] [Range(10, 800)] float rotationSpeedUpDown;
    [Tooltip("Geschwindigkeit mit der der Flieger an der y-Achse rotiert.")]
    [SerializeField] [Range(10, 800)] float rotationSpeedLeftRight;
    [Tooltip("Geschwindigkeit mit der der Flieger an der z-Achse rotiert rotiert.")]
    [SerializeField] [Range(10, 800)] float stabilizeSpeed;
    [Tooltip("Kraft mit der der Flugkörper richtung Boden gedrückt wird.")]
    [SerializeField] [Range(0, 0.5f)] float gravity;
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

    [SerializeField] float boostSpeed;
    [SerializeField] float initialBoostSpeed;

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

    public bool isPlayerTopUp;
    public bool noInput;


    Quaternion downRotation;


    // Animation
    Quaternion currentRightRotationTarget;
    Quaternion currentLeftRotationTarget;

    //InputSystem
    Controls myControls;


    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myControls = new Controls();
 
    }
    void OnEnable()
    {
        myControls.Player.Enable();
    }

    private void OnDisable()
    {
        myControls.Player.Disable();
    }
    private void Start()
    {
        currentSpeed = startSpeed;                                                                  // Startspeed wird gesetzt
        downRotation = Quaternion.identity;
        downRotation.x = 1;
    }


    private void Update()
    {
        noInput = (lefttWhingControlStick == Vector2.zero && rightWhingControlStick == Vector2.zero);
        //Debug.Log("Speed: " + currentSpeed);
        if (noInput)
            CheckAxisUpY();
        BoostInput();
    }

    private void FixedUpdate()
    {
        Move();
        AddGravity();
        RotateWhings();

    }

    void OnRightWhing(InputValue value)                                                             // Inputs vom rechten Joystick werden ausgelesen
    {
        if (isPlayerTopUp)
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
        else
        {

            lefttWhingControlStick = value.Get<Vector2>();
            lefttControlX = lefttWhingControlStick.x;
            lefttControlY = lefttWhingControlStick.y;

            if (lefttControlY < inputSensitivity && lefttControlY > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                lefttControlY = 0;
            }

            if (lefttControlX < inputSensitivity && lefttControlX > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                lefttControlX = 0;
            }
        }



    }

  

    void OnLeftWhing(InputValue value)                                                              // Inputs vom linken Joystick werden ausgelesen
    {
        if (isPlayerTopUp)
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
        else
        {
            rightWhingControlStick = value.Get<Vector2>();
            rightControlX = -rightWhingControlStick.x;
            rightControlY = rightWhingControlStick.y;

            if (rightControlY < inputSensitivity && rightControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                rightControlY = 0;
            }

            if (rightControlX < inputSensitivity && rightControlX > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                rightControlX = 0;
            }

        }



    }

    void OnBoost(InputValue value)
    {
        ActivateBoost();
    }

    private void Move()
    {
        if (transform.forward.y < 0)
        {
            // Speed wird schneller und langsamer je nach Blickruchtung hoch oder Runter
            currentSpeed = (myRigidbody.velocity.magnitude);
            currentSpeed += (fallVelocity * -transform.forward.y);
        }
        else
        {
            currentSpeed -= (glideUpVelocity * transform.forward.y);
            if (myRigidbody.velocity.magnitude < currentSpeed)
                currentSpeed = myRigidbody.velocity.magnitude;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);                                        // Beschläunigt nur bis zum Maximalspeed 

        // Vorwärtsbewegung
        //myRigidbody.position += transform.forward * currentSpeed * Time.deltaTime;

        myRigidbody.AddForce(transform.forward * currentSpeed * 10, ForceMode.Force);





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



        //Quaternion rightRotation = rightWhing.transform.rotation;                                    // Einen Mittelwert aus den Flügelvektoren berechnen
        //Quaternion leftRotation = leftWhing.transform.rotation;
        //Quaternion midRotation = Quaternion.Slerp(rightRotation, leftRotation, 0.5f);


        //myRigidbody.transform.rotation = Quaternion.Lerp(transform.rotation, midRotation, stabilizeSpeed * Time.deltaTime);     // Aktuelle Rotation an der z Achse richtung des Mittelwerts anpassen - !!!Hier ist noch was nicht ganz richtig am Start
    }

    private void CheckAxisUpY()
    {

        if (this.transform.up.y > 0)
        {
            isPlayerTopUp = true;
        }
        else
        {
            isPlayerTopUp = false;
        }
    }


    private void AddGravity()
    {
        if (currentSpeed < gravitySpeedBoundery)
        {
            myRigidbody.useGravity = true;
            //float slowMultiplyer = 1 - (Mathf.InverseLerp(0, gravitySpeedBoundery, currentSpeed));
            //float t = Time.deltaTime * gravity * slowMultiplyer;

            ////transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, t);
            //Quaternion deltaDownRotation = Quaternion.Euler(Vector3.down * t);
            //myRigidbody.MoveRotation(myRigidbody.rotation * deltaDownRotation);
        }
        else
        {
            myRigidbody.useGravity = false;
        }
    }

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

    void ActivateBoost()
    {
       // myRigidbody.AddForce(transform.forward * boostSpeed * 10, ForceMode.VelocityChange);
    }

    private void BoostInput()
    {
        if (myControls.Player.Boost.WasPressedThisFrame())
        {
            myRigidbody.AddForce(transform.forward * initialBoostSpeed, ForceMode.VelocityChange);
        }
            if (myControls.Player.Boost.IsInProgress())
        {
            myRigidbody.AddForce(transform.forward * boostSpeed, ForceMode.VelocityChange);
        }
    }
}
