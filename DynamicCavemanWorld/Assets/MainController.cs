﻿using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Literally, Hello World!");
        World TheWorld = new World();
        Debug.Log("World Model Made!");
        ElevationView.BuildMesh(TheWorld.elevation);
        Debug.Log("Elevation View Made!");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
