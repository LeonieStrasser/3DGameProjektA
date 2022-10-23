using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane_Movement : MonoBehaviour
{
    public GameObject Player;

    [Header("Speed")]
    public float speed = 0;

    [Header("Movement")]
    public float verticalMovement = 0f;
    public float horizontalMovement = 0f;

    Vector2 rightWhingControlStick;
    float rightControlY;

    Vector2 lefttWhingControlStick;
    float lefttControlY;

    [SerializeField][Range(0, 0.5f)] float inputSensitivity;

    private Rigidbody rb;

    private void Start()
    {
       rb = Player.GetComponent<Rigidbody>();
    }

    void Update()
    {

        // Vorwärsbewegung 
        transform.position += transform.forward * verticalMovement * speed * Time.deltaTime;

        //Geschwindigkeit
        speed -= transform.forward.y * 2.0f * Time.deltaTime;

        if(speed < 2.0f)
        {
            speed = 2.0f;
        }

        if(speed > 5.0f)
        {
            speed = 5.0f;
        }


        //Rotation auf den Achsen
        transform.Rotate(Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal") * -1.0f);

        //Kollision Ground
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


}
