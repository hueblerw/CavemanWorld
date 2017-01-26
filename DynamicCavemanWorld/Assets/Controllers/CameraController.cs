using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // Update is called once per frame
    public float speed = 250.0f;
    float x;
    float y;
    float z;

    void Start()
    {
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
            z += (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            z -= (speed * Time.deltaTime);
        }
        transform.position = new Vector3(x, y, z);
    }
}
