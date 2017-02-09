using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour {

    Component loadingUpdater;

    // Use this for initialization
    void Start () {
        loadingUpdater = GetComponent("LoadingStatus");
	}
	
	// Update is called once per frame
	void Update () {
        // loadingUpdater = "World Creation!";
	}
}
