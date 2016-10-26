using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileMouseOver : MonoBehaviour {

    public Text TileInfo;
    public Text DateInfo;

    void Start()
    {
        // Tile Info
        TileInfo = findTextWithName("TileInfoDisplay");
        TileInfo.text = "Tile Info:";
        // Date Info
        DateInfo = findTextWithName("DateDisplay");
        Debug.Log("Mouse Over Initialized!");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTileInfo();
    }

    private void UpdateTileInfo()
    {
        // TileInfo.text = "OMG A MOUSE!!!";
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            GetComponent<Renderer>().material.color = Color.red;
            // Debug.Log(hitInfo.point);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private Text findTextWithName(string name)
    {
        GameObject canvas = GameObject.Find("Canvas");
        Text[] texts = canvas.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == name)
            {
                return texts[i];
            }
        }
        return null;
    }

    public void UpdateTheDate()
    {
        DateInfo.text = "Day " + (MainController.day + 1) + ", Year " + MainController.year;
    }
}
