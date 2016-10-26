using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MainController : MonoBehaviour {

    public static int year;
    public static int day;
    public TileMouseOver mouseController;
	// Use this for initialization
	void Start () {
        // Generate the model files eventually
        
        // Construct the model from text files
        Debug.Log("Literally, Hello World!");
        World TheWorld = new World(50, 50);
        Debug.Log("World Model Made!");

        // Construct the elevation view
        // Get the Mesh components
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        // Create and attach the mesh
        Mesh mesh = ElevationView.BuildMesh(TheWorld.elevationVertices);
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        // Create and attach the texture
        meshRenderer.sharedMaterial.mainTexture = ElevationView.BuildTexture(TheWorld);
        Debug.Log("Elevation View Made!");
        // Set the time
        day = 0;
        year = 1;
        // Initialize the Game interaction Controllers
        mouseController = new TileMouseOver();
    }
	
	// Update is called once per frame

}
