using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour {

    Component loadingUpdater;
    public static World TheWorld;
    public static int WorldX;
    public static int WorldZ;
    public TileMouseOver mouseController;
    private const float WAIT = .1f;

    // Use this for initialization
    void Start () {
        loadingUpdater = GetComponent("LoadingStatus");
        if (TheWorld == null)
        {
            Debug.Log("Load World");
            StartCoroutine(LateStart());
        }
        else
        {
            Debug.Log("Generate World");
            StartCoroutine(LateGenerator());
        }
    }


    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(WAIT);

        TheWorld = WorldFromFile();
        SceneManager.LoadScene("WorldView");
    }


    IEnumerator LateGenerator()
    {
        yield return new WaitForSeconds(WAIT);

        TheWorld = WorldofSize(WorldX, WorldZ);
        SceneManager.LoadScene("WorldView");
    }


    private World WorldFromFile()
    {
        // Generate the model files eventually

        // Construct the model from text files
        Debug.Log("Literally, Hello World!");
        World InitWorld = new World(50, 50, false);
        Debug.Log("World Model Made!");
        return InitWorld;
    }


    // Generate a Random World Method
    private World WorldofSize(int x, int y)
    {
        // Generate the model files eventually

        // Construct the model from text files
        Debug.Log("Literally, Hello World!");
        World InitWorld = new World(x, y, true);
        Debug.Log("World Model Made!");
        return InitWorld;
    }


    // Update is called once per frame
    void Update () {
        // loadingUpdater = "World Creation!";
	}
}
