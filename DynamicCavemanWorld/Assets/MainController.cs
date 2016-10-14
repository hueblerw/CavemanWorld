using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Generate the model files eventually

        // Construct the model from text files
        Debug.Log("Literally, Hello World!");
        World TheWorld = new World(50, 50);
        Debug.Log("World Model Made!");

        // Construct the elevation view
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        Mesh mesh = ElevationView.BuildMesh(TheWorld.elevation, meshFilter, meshCollider, meshRenderer);
        meshFilter.mesh = mesh;
        Debug.Log("Elevation View Made!");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
