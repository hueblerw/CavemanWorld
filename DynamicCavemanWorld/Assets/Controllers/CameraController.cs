using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

    // Update is called once per frame
    public float speed;
    float x;
    float y;
    float z;

    void Start()
    {
        speed = 250f;
        x = 350f;
        y = 850f;
        z = 500f;
        transform.position = new Vector3(x, y, z);
    }

    void Update()
    {
        // code for translating
        if (Input.GetKey(KeyCode.D))
        {
            x += (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            x -= (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            z -= (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        if (Input.GetKey(KeyCode.W))
        {
            z += (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        // Code for zooming in and out
        if (Input.GetKey(KeyCode.LeftShift))
        {
            y -= (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            y += (speed * Time.deltaTime);
            ActivateBoundedTransform(x, y, z);
        }
        // Code for rotating the screen along the x axis.
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate((speed / 25) * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-(speed / 25) * Time.deltaTime, 0, 0);
        }
        // Code for rotating the screen along the z axis.
        if (Input.GetKey(KeyCode.Tab))
        {
            transform.Rotate(0, 0, (speed / 25) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(0, 0, -(speed / 25) * Time.deltaTime);
        }
    }

    // Bounds the area the camera can move to.
    private void ActivateBoundedTransform(float x, float y, float z)
    {
        if (x < -200f)
        {
            x = -200f;
        }
        if (z < -200f)
        {
            z = -200f;
        }
        if (y < 0f)
        {
            y = 0f;
        }
        if (y > 1200f)
        {
            y = 1200f;
        }

        transform.position = new Vector3(x, y, z);
    }
}
