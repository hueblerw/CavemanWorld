using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour {

    Component loadingUpdater;
    public static World TheWorld;

    // Use this for initialization
    void Start () {
        loadingUpdater = GetComponent("LoadingStatus");
        StartCoroutine(LateStart(.1f));
    }


    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        TheWorld = WorldFromFile();
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


    // Update is called once per frame
    void Update () {
        // loadingUpdater = "World Creation!";
	}
}
