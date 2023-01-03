using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
using Lofelt.NiceVibrations;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class WhingMovement01 : MonoBehaviour
{
    #region inspectorValues
    [Header("Movement")]

    [SerializeField] bool easyMovement;
    [SerializeField] [Range(1, 10)] float easyMovementRotSpeed;

    //[Tooltip("Speed der am Start gesetzt wird.")]
    //[SerializeField] float startSpeed = 10f;
    [Tooltip("Faktor um den sich der Flugk�rper verschnellert, wenn er abw�rts fliegt. Selber Faktor um den sich der Flugk�rper verlangsamt, wenn er aufw�rts fliegt. ")] // eigentlich w�r es sinnvoll wenn der faktor aufw�rts gr��er ist - Wegen der Schwerkraft
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
    [Tooltip("Kraft mit der der Flugk�rper richtung Boden gedr�ckt wird.")]
    [SerializeField] [Range(0, 0.5f)] float gravity;
    [Tooltip("Geschwindigkeits Obergrenze ab der die Kraft nach Unten anf�ngt zu wirken. (Von da an wirkt sie umso st�rker, je langsamer das Flugobjekt wird)")]
    [SerializeField] float gravitySpeedBoundery = 20f;

    [Space(10)]
    [Tooltip("Sensitivit�t f�r den Joystick Input.")]
    [SerializeField] [Range(0, 0.7f)] float inputSensitivity;
    [SerializeField] [Range(0, 0.7f)] float inputSensitivityLeftRight;

    [Space(20)]
    [Header("Twirl")]
    [SerializeField] [Tooltip("Mit diesem Wert kann man einstellen ab welcher Twirlgeschwindigkeit der Twirl-Effect getriggert wird")] [Range(0, 1)] float twirlInput;
    [SerializeField] [Tooltip("Inputsensitivity ab der der Twirl-Effect getriggert wird. (0 => beide Sticks m�ssen exakt die entgegengesetzte Position auf Y erreichen)")] [Range(0, 1)] float twirlSensitivity;
    [SerializeField] [Range(10, 800)] float twirlSpeed;



    [Space(20)]
    [Header("Wing Animation")]
    [SerializeField] GameObject rightWhing;
    [SerializeField] GameObject leftWhing;
    [SerializeField] float whingRotationSpeed = 10f;
    [SerializeField] float maxRotation;
    [SerializeField] float minRotation;
    [SerializeField] float neutralRotation;

    [Space(20)]
    [Header("Recources")]
    [SerializeField] float startMaxRecourceA = 10;

    [Space(5)]
    [Header("Boost Power")]
    [SerializeField] float boostSpeed;
    [SerializeField] float initialBoostSpeed;
    [SerializeField] float initialBoostCosts;
    [SerializeField] float boostCosts;

    [Space(5)]
    [Header("Slowmotion Power")]
    [SerializeField] [Range(0.1f, 1)] float slowmoTimescale;
    [SerializeField] float slowmoCosts;

    [Space(20)]
    [Header("Camera behaviour")]
    [SerializeField] [Range(0, 60)] float deadZoneRadius;


    [Space(20)]
    [Header("Player States")]
    [SerializeField] bool isPlayerTopUp;
    [SerializeField] bool straightUp;
    [SerializeField] bool straightDown;
    [SerializeField] bool noInput;
    [SerializeField] bool twirl;

    public bool Twirl
    {
        get { return twirl; }
        private set { twirl = value; }
    }

    [Space(20)]
    [Header("Feedbacks")]
    public GameObject twirlVFX;
    /// a MMFeedbacks to play when we Boost
    public MMFeedbacks BoostStartFeedback;
    public MMFeedbacks SlowMoFeedback;
    public MMFeedbacks SlowMoHoldingFeedback;
    [SerializeField] bool StraightUpDownOnceTrigger = false;
    public HapticClip straightUpDownHaptic;
    public GameObject straightUpDownVFX;
    public MMFeedbacks StraightUpFeedback;
    public MMFeedbacks StraightDownFeedback;
    // public MMFeedbacks TwirlFeedback;


    #endregion








    #region hiddenValues

    //-------------MOVEMENT

    private LevelManager myManager;
    private Rigidbody myRigidbody;

    float currentSpeed;

    Vector2 leftStickInput;
    Vector2 rightStickInput;

    Vector2 rightWhingControlStick;
    float rightControlX;
    float rightControlY;

    Vector2 lefttWhingControlStick;
    float lefttControlX;
    float lefttControlY;

    float currentRotationUpDown;
    float currentRotationLeftRight;
    float currentRotationForward;
    public bool flipControls;

    Quaternion downRotation;


    //-------------RESOURCES

    float currentMaxRecource;
    private float resourceA;
    public float ResourceA
    {
        private set
        {
            resourceA = Mathf.Clamp(value, 0, currentMaxRecource); // ACHTUNG! Sollte sich die max Resource im laufe des Games �ndern, muss hier der Code angepasst werden!!!
            resourceAInRelationToMax = resourceA / currentMaxRecource;
        }
        get
        {
            return resourceA;
        }
    }

    private float resourceAInRelationToMax;
    public float ResourceAInRelationToMax { get => resourceAInRelationToMax; }


    // Animation
    Quaternion currentRightRotationTarget;
    Quaternion currentLeftRotationTarget;

    //InputSystem
    Controls myControls;

    int lastLeftInput;
    int lastRightInput;
    #endregion


    #region events

    public event Action<bool, bool> OnDeadzoneValueChanged; // erster bool ist up, zweiter bool ist down

    #endregion






    private void Awake()
    {
        myManager = FindObjectOfType<LevelManager>();
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

        downRotation = Quaternion.identity;
        downRotation.x = 1;

        currentMaxRecource = startMaxRecourceA; // Später kann die maximal recource mehr sein - jetzt wird sie auf die beginn max gesetzt
        ResourceA = currentMaxRecource; // Tank wird auf voll gesetzt
    }


    private void Update()
    {
        if (myManager.CurrentGameState == LevelManager.gameState.running)
        {
            noInput = (lefttWhingControlStick == Vector2.zero && rightWhingControlStick == Vector2.zero); // CHeck ob der Player einen input gibt
                                                                                                          //Debug.Log("Speed: " + currentSpeed);
            

            if ((noInput || CheckInputChange()) && flipControls == true)
                CheckUpPosition();

            if (resourceA > 0)
            {
                BoostInput();
                SlowmoInput();
            }

            StraightUpDownFeedbackTrigger();
            if (!easyMovement)
                TwirlEffect();

            CheckDeadzonePositions();

        }
    }

    private void FixedUpdate()
    {
        Move();
        AddGravity();
        if (!easyMovement)
            RotateWhings();

    }





    #region inputs

    void OnRightWhing(InputValue value)                                                             // Inputs vom rechten Joystick werden ausgelesen
    {
        rightStickInput = value.Get<Vector2>();
        rightControlX = rightStickInput.x;

        if (rightControlX < inputSensitivityLeftRight && rightControlX > -inputSensitivityLeftRight)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            rightControlX = 0;
        }

        if (isPlayerTopUp)
        {
            rightWhingControlStick = rightStickInput;
            //rightControlX = rightWhingControlStick.x;
            rightControlY = -rightWhingControlStick.y;

            if (rightControlY < inputSensitivity && rightControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                rightControlY = 0;
            }

        }
        else
        {

            lefttWhingControlStick = rightStickInput;
            //lefttControlX = lefttWhingControlStick.x;
            lefttControlY = lefttWhingControlStick.y;

            if (lefttControlY < inputSensitivity && lefttControlY > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                lefttControlY = 0;
            }


        }
    }



    void OnLeftWhing(InputValue value)                                                              // Inputs vom linken Joystick werden ausgelesen
    {
        leftStickInput = value.Get<Vector2>();
        lefttControlX = -leftStickInput.x;
        if (lefttControlX < inputSensitivityLeftRight && lefttControlX > -inputSensitivityLeftRight)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            lefttControlX = 0;
        }

        if (isPlayerTopUp)
        {
            lefttWhingControlStick = leftStickInput;
            //lefttControlX = -lefttWhingControlStick.x;
            lefttControlY = -lefttWhingControlStick.y;

            if (lefttControlY < inputSensitivity && lefttControlY > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                lefttControlY = 0;
            }


        }
        else
        {
            rightWhingControlStick = leftStickInput;
            //rightControlX = -rightWhingControlStick.x;
            rightControlY = rightWhingControlStick.y;

            if (rightControlY < inputSensitivity && rightControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
            {
                rightControlY = 0;
            }



        }
    }

    void OnBoost(InputValue value)
    {
        ActivateBoost();

    }
    #endregion






    #region playerMotion



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

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);                                        // Beschl�unigt nur bis zum Maximalspeed 



        myRigidbody.AddForce(transform.forward * currentSpeed * 10, ForceMode.Force);



        if (!easyMovement)
        {
            //Rotation hoch und runter
            currentRotationUpDown = rotationSpeedUpDown * (rightControlY / 3.5f + lefttControlY / 3.5f);
            Quaternion deltaXRotation = Quaternion.Euler(new Vector3(currentRotationUpDown, 0, 0) * Time.fixedDeltaTime);


            // Rotation an der Blickrichtung

            float currentStabilizeSpeed = stabilizeSpeed; // Wenn der Twirl Aktiv ist, wird statt dem normalenm Speed der extra Twirl speed genutzt
            if (twirl)
                currentStabilizeSpeed = twirlSpeed;

            currentRotationForward = currentStabilizeSpeed * ((rightControlY - lefttControlY) / 2);
            Quaternion deltaZRotation = Quaternion.Euler(new Vector3(0, 0, -currentRotationForward) * Time.fixedDeltaTime);





            //Rotation rechts und links
            currentRotationLeftRight = (rightControlX - lefttControlX) / 2;
            Quaternion direction = Quaternion.FromToRotation(myRigidbody.transform.forward, Camera.main.transform.right * currentRotationLeftRight).normalized;
            Quaternion rotationOfDirection = Quaternion.Euler(rotationSpeedLeftRight * Time.fixedDeltaTime * direction.x, rotationSpeedLeftRight * Time.fixedDeltaTime * direction.y, rotationSpeedLeftRight * Time.fixedDeltaTime * direction.z);




            // Rotationen zusammenrechnen

            Quaternion rigidbodyBasedRotation = rotationOfDirection * myRigidbody.rotation * (deltaXRotation * deltaZRotation);

            myRigidbody.MoveRotation(rigidbodyBasedRotation);






        }
        else // EASY MOVEMENT
        {




            //Rotation hoch und runter


            Vector3 playerWorldDown = -Camera.main.transform.up;

            Vector3 x = (transform.position + transform.forward + (playerWorldDown * Time.fixedDeltaTime * -leftStickInput.y * easyMovementRotSpeed));
            transform.LookAt(x);

            ////Rotation rechts und links
            Vector3 playerWorldRight = Camera.main.transform.right;

            Vector3 y = (transform.position + transform.forward + (playerWorldRight * Time.fixedDeltaTime * leftStickInput.x * easyMovementRotSpeed));
            transform.LookAt(y);

            // Rotation an der Blickrichtung
            // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rightStickInput.x), stabilizeSpeed );
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
    #endregion



    private bool CheckInputChange()
    {
        bool HasInputChanged;
        int newLeftInput = 0;
        int newRightInput = 0;

        if (lefttControlY < -inputSensitivity)
            newLeftInput = -1;
        else if (lefttControlY > inputSensitivity)
            newLeftInput = 1;

        if (rightControlY < -inputSensitivity)
            newRightInput = -1;
        else if (rightControlY > inputSensitivity)
            newRightInput = 1;


        if (newLeftInput != lastLeftInput || newRightInput != lastRightInput)
        {

            HasInputChanged = true;

        }
        else
        {
            HasInputChanged = false;
        }

        lastRightInput = newRightInput;
        lastLeftInput = newLeftInput;

        return HasInputChanged;
    }



    private void CheckUpPosition()
    {
        // Is Top oben?
        if (Vector3.Angle(this.transform.up, Camera.main.transform.up) < 90)
        {
            isPlayerTopUp = true;
        }
        else
        {
            isPlayerTopUp = false;
        }

    }

    private void CheckDeadzonePositions()
    {
        // STeile Checken
        float upAngle = Vector3.Angle(this.transform.forward, Vector3.up);
        // ALter State wird gespeichert um ihn sp�ter mit dem neuen zu vergleichen
        bool lastUpBool = straightUp;
        bool lastDownBool = straightDown;
        // Winkel zur Welt mit Deadzone Winkel abgleichen
        straightUp = (upAngle < deadZoneRadius);
        straightDown = (upAngle < 180 + deadZoneRadius && upAngle > 180 - deadZoneRadius);
        // ALter und neuer Winkel vergleichen
        if (lastDownBool != straightDown || lastUpBool != straightUp)
        {
            OnDeadzoneValueChanged?.Invoke(straightUp, straightDown);
        }
    }

    private void StraightUpDownFeedbackTrigger()
    {
        if (StraightUpDownOnceTrigger == false)
        {
            if (straightDown == true)
            {
                //HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                //HapticController.Load(straightUpDownHaptic);
                //HapticController.Loop(true);
                //HapticController.Play();

                straightUpDownVFX.SetActive(true);
                StraightDownFeedback?.PlayFeedbacks();
            }
            if (straightUp == true)
            {
                //HapticController.Load(straightUpDownHaptic);
                //HapticController.Loop(true);
                //HapticController.Play();

                straightUpDownVFX.SetActive(true);
                StraightUpFeedback?.PlayFeedbacks();
            }

            StraightUpDownOnceTrigger = true;
        }

        if (straightDown == false && straightUp == false)
        {
            //HapticController.Stop();

            straightUpDownVFX.SetActive(false);
            StraightUpDownOnceTrigger = false;
        }
    }

    private void TwirlEffect()
    {
        if (Mathf.Abs(rightStickInput.y) >= twirlInput && Mathf.Abs(leftStickInput.y) >= twirlInput)
        {
            twirl = (rightStickInput.y + leftStickInput.y < twirlSensitivity && rightStickInput.y + leftStickInput.y > -twirlSensitivity);                 // Twirl ist wahr wenn die Sticks genau entgegengesetzt zeigen
                                                                                                                                                           //TwirlFeedback?.PlayFeedbacks();

        }
        else
        {
            twirl = false;

        }
        twirlVFX.SetActive(twirl);
    }

    void ActivateBoost()
    {
        // myRigidbody.AddForce(transform.forward * boostSpeed * 10, ForceMode.VelocityChange);
    }

    private void BoostInput()
    {
        if (myControls.Player.Boost.WasPressedThisFrame())
        {
            BoostStartFeedback?.PlayFeedbacks();

            myRigidbody.AddForce(transform.forward * initialBoostSpeed, ForceMode.VelocityChange);
            ResourceA -= initialBoostCosts; // Ressource wird verbraucht
        }
        if (myControls.Player.Boost.IsInProgress())
        {
            myRigidbody.AddForce(transform.forward * boostSpeed, ForceMode.VelocityChange);
            ResourceA -= boostCosts * Time.deltaTime; // Ressource wird verbraucht in diesem frame gemessen an der Frametime verbraucht
        }
    }
    void SlowmoInput()
    {
        if (myControls.Player.SlowMo.WasPressedThisFrame())
        {
            Time.timeScale = slowmoTimescale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            SlowMoFeedback?.PlayFeedbacks();
        }
        if (myControls.Player.SlowMo.IsInProgress())
        {
            ResourceA -= slowmoCosts * Time.deltaTime; // Ressource wird verbraucht in diesem frame gemessen an der Frametime verbraucht
            SlowMoHoldingFeedback?.PlayFeedbacks();
        }
        if (myControls.Player.SlowMo.WasReleasedThisFrame())
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }



    //-------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------PUBLIC------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------


    public void AddResourcePoints(int newPoints)
    {
        ResourceA += newPoints;
    }

    public void AddMaxRecourcePoints(float addProcent) // Um diesen Prozentteil wird der Tank erweitert
    {
        currentMaxRecource *= addProcent;
    }

}
