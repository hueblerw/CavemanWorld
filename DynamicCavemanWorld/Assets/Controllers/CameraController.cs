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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            x += (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x -= (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            z -= (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            z += (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            y += (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightControl))
        {
            y -= (speed * Time.deltaTime);
        }

        float[] coor = CreateBoundedCooridnates(x, y, z);

        transform.position = new Vector3(coor[0], coor[1], coor[2]);
    }

    // Bounds the area the camera can move to.
    private float[] CreateBoundedCooridnates(float x, float y, float z)
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
        float[] coor = new float[3] { x, y, z };
        return coor;
    }
}
