using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MainController : MonoBehaviour {

    public Text TileInfo;
    public Text TimeInfo;
    public int year;
    public int day;
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
        day = 1;
        year = 1;
        // Initialize the Game interaction Controllers
        // InitTheMouse(day, year);
    }
	
	// Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            // UpdateTileInfo();
            // Debug.Log("So I found the world!");
        }
        else
        {
            // Great!
        }
    }

    // Mouse overlay methods
    private void InitTheMouse(int day, int year)
    {
        TileInfo.text = "Square Info:";
        TimeInfo.text = "Year: " + year + "        Day: " + day;
    }

    private void UpdateTileInfo()
    {
        TileInfo.text = "OMG A MOUSE!!!";
    }

}
