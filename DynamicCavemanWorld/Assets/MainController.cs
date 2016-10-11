using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
        elevation.readCSVFile();
        Debug.Log("Hello World!");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
