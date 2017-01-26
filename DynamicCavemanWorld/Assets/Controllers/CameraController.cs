using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // Update is called once per frame
    public float speed = 1.0f;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(0, 0, speed * Time.deltaTime,);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}
