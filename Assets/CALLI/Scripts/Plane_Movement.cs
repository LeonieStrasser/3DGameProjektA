using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane_Movement : MonoBehaviour
{
    public GameObject Player;

    Vector3 moveCamTo;

    [Header("Speed")]
    public float speed = 0;

    [Header("Movement")]
    public float verticalMovement = 0f;
    public float horizontalMovement = 0f;

    Vector2 rightWhingControlStick;
    float rightControlY;
    float leftControlX;

    Vector2 lefttWhingControlStick;
    float leftControlY;
    float rightControlX;

    float currentRotationLeftRight;

    [SerializeField][Range(10, 400)] float rotationSpeedLeftRight;

    [SerializeField][Range(0, 0.5f)] float inputSensitivity;

    private Rigidbody rb;

    private void Start()
    {
        rb = Player.GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {

        Vector3 moveCamTo = transform.position - transform.forward * 2.0f + Vector3.up * 2.0f;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 20.0f);

        //Movement();

        // Vorwärsbewegung 
        transform.position += transform.forward * verticalMovement * speed * Time.deltaTime;

        //Geschwindigkeit
        speed -= transform.forward.y * 2.0f * Time.deltaTime;

        //Minimum und Maximum speed, damit Player nie zum Stillstand kommt und nicht unkontrollierbar wird

        if (speed < 2.0f)
        {
            speed = 2.0f;
        }

        if(speed > 40.0f)
        {
            speed = 40.0f;
        }


        //Rotation auf den Achsen
        transform.Rotate(Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal") * -1.0f);

        //Collision Ground
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if (terrainHeight > Player.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
        }

    }

    void OnRightWhing(InputValue value)                                                             // Inputs vom rechten Joystick werden ausgelesen
    {
        rightWhingControlStick = value.Get<Vector2>();
        rightControlY = -rightWhingControlStick.y;
        rightControlX = rightWhingControlStick.x;


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
        leftControlY = -lefttWhingControlStick.y;


        if (leftControlY < inputSensitivity && leftControlY > -inputSensitivity)                   // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            leftControlY = 0;
        }

        if (leftControlX < inputSensitivity && leftControlX > -inputSensitivity)                  // Input wird 0 gesetzt wenn er unter der Input Sensitivity liegt
        {
            leftControlX = 0;
        }
    }

    private void Movement()
    {
        //Rotation rechts und links
        currentRotationLeftRight = rotationSpeedLeftRight * ((rightControlX - leftControlX) / 2);
        Quaternion deltaYRotation = Quaternion.Euler(new Vector3(0, currentRotationLeftRight, 0) * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaYRotation);
    }
}
