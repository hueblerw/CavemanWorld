using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static");
        Debug.Log("Hello World!");
        Debug.Log(elevation.layerName);
        Debug.Log(elevation.getType());
        Debug.Log(SingleValueLayer.WORLDX);
        Debug.Log(SingleValueLayer.WORLDZ);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
