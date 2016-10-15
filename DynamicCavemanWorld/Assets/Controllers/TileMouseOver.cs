using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
	}
}
