using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasd_Mvmnt : MonoBehaviour
{
    public KeyCode MoveUp;
    public KeyCode MoveDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          {
            transform.position += new Vector3(0, 3, 0) * Time.deltaTime;
            
            if(transform.position.y > 2.7f)
            {
                transform.position = new Vector3(transform.position.x, 2.7f, 0);
            }
        }

         if(Input.GetKey(MoveDown))
        {
            transform.position -= new Vector3(0, 3, 0) * Time.deltaTime;
            
            if(transform.position.y < -2.7f)
            {
                transform.position = new Vector3(transform.position.x, -2.7f, 0);
            }
        }
    }
}
