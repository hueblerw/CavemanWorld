using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour {

    Text loadingUpdater;
    public static World TheWorld;
    public static int WorldX;
    public static int WorldZ;

    private const float WAIT = .1f;

    // Use this for initialization
    void Start () {
        
        if (TheWorld == null)
        {
            Debug.Log("Load World");
            StartCoroutine(LateStart());
        }
        else
        {
            loadingUpdater = findTextWithName("LoadingStatus");
            loadingUpdater.text = "Generating New World";
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


    // Private Methods
    public Text findTextWithName(string name)
    {
        GameObject panel = GameObject.Find("Panel");
        Text[] texts = panel.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == name)
            {
                return texts[i];
            }
        }
        return null;
    }



    // Update is called once per frame
    void Update () {
        // loadingUpdater = "World Creation!";
	}
}
