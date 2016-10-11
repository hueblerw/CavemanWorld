using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SingleValueLayer Elevation = new SingleValueLayer("Elevation", "Semi-static");
        Debug.Log("Hello World!");
        Debug.Log(Elevation);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
