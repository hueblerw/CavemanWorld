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
    public static World TheWorld;
    public Texture2D textureMap;
    public string currentView;

	// Use this for initialization
	void Start () {
        // Generate the model files eventually
        
        // Construct the model from text files
        Debug.Log("Literally, Hello World!");
        TheWorld = new World(50, 50);
        Debug.Log("World Model Made!");

        // Construct the elevation view
        // Get the Mesh components
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        // Create and attach the mesh
        Mesh mesh = WorldView.BuildMesh(TheWorld.elevationVertices);
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        // Create and attach the texture
        meshRenderer.sharedMaterial.mainTexture = WorldView.BuildElevationTexture(TheWorld);
        currentView = "Elevation";
        Debug.Log("Elevation View Made!");
        // Set the time
        day = 0;
        year = 1;
        // Initialize the Game interaction Controllers
        mouseController = new TileMouseOver(meshCollider, meshRenderer);
    }
	
	// Update is called once per frame
    void Update()
    {
        mouseController.UpdateTileInfo();
    }

    // Moves us to the next day
    public void NextDay()
    {
        if (day == 119)
        {
            day = 0;
            year += 1;
            mouseController.UpdateTheDate();
            TheWorld.NewYear();
            if (currentView == "Habitat")
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.sharedMaterial.mainTexture = WorldView.BuildHabitatTexture(TheWorld, textureMap);
            }
        }
        else
        {
            day += 1;
            mouseController.UpdateTheDate();
        }
        
    }

    // Toggles between the two world display methods
    public void ToggleView()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        switch (currentView)
        {
            case "Elevation":
                meshRenderer.sharedMaterial.mainTexture = WorldView.BuildHabitatTexture(TheWorld, textureMap);
                mouseController.ToggleButtonDisplayName(currentView);
                currentView = "Habitat";
                Debug.Log("Habitat View Made!");
                break;
            case "Habitat":
                meshRenderer.sharedMaterial.mainTexture = WorldView.BuildElevationTexture(TheWorld);
                mouseController.ToggleButtonDisplayName(currentView);
                currentView = "Elevation";
                Debug.Log("Elevation View Made!");
                break;
        }     
    }

}
