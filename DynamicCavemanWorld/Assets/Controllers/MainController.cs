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
    public Canvas cntrlInfo;
    public bool clockRunning;
    public float clockSpeed = 1.0f;

	// Use this for initialization
	void Start () {
        clockRunning = false;
        Debug.Log("Enter the World Scene!");
        // Assign the world created from the loading screen.
        TheWorld = LoadingScreenController.TheWorld;
        // Generate the model files eventually

        // Construct the model from text files
        if (TheWorld == null)
        {
            TheWorld = new World(50, 50, false);
        }     

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
        // Update the mouse-over tile info
        mouseController.UpdateTileInfo();
    }


    // Advances a day everytime speed seconds if the clock is running
    IEnumerator RunTheClock()
    {
        while (clockRunning)
        {
            NextDay();
            yield return new WaitForSeconds(clockSpeed);
        }
    }


    // Moves us to the next day
    public void NextDay()
    {
        if (day == 119)
        {
            NewYear();
        }
        else
        {
            day += 1;
            mouseController.UpdateTheDate();
        }
        
    }


    // Moves us up 15 days
    public void AdvanceFifteenDays()
    {
        if (day >= 104)
        {
            NewYear();
        }
        else
        {
            day += 15;
            mouseController.UpdateTheDate();
        }
    }


    // Moves us to the new year.
    public void NewYear()
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


    // Generate's and displays a new random world
    public void GenerateNewRandomWorld()
    {
        // Construct a random world of a given size
        // First get the world dimensions from the main interface.     
        if (mouseController.findTextWithName("X-Dim").text == "")
        {
            LoadingScreenController.WorldX = 50;
        }
        else
        {
            LoadingScreenController.WorldX = int.Parse(mouseController.findTextWithName("X-Dim").text);
        }

        if (mouseController.findTextWithName("Y-Dim").text == "")
        {
            LoadingScreenController.WorldZ = 50;
        }
        else
        {
            LoadingScreenController.WorldZ = int.Parse(mouseController.findTextWithName("Y-Dim").text);
        }
        // Trigger the Loading Screen
        // Then build a new world of those dimensions
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");
    }
    

    // Hides the Control Information Screen
    public void ToggleCntrlInfoScreen()
    {
        cntrlInfo.enabled = false;
    }


    // Toggles the Clock on and off
    public void ToggleTheClock()
    {
        if (clockRunning)
        {
            clockRunning = false;
        }
        else
        {
            clockRunning = true;
            StartCoroutine(RunTheClock());
        }
    }

}
