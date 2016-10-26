using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TileMouseOver : MonoBehaviour {

    public Text TileInfo;
    public Text DateInfo;
    public Collider mapCollider;
    public Renderer mapRenderer;

    public TileMouseOver(Collider collider, Renderer renderer)
    {
        // Tile Info
        TileInfo = findTextWithName("TileInfoDisplay");
        TileInfo.text = "Tile Info:";
        // Date Info
        DateInfo = findTextWithName("DateDisplay");
        UpdateTheDate();
        Debug.Log("Mouse Over Initialized!");
        // Collider & Renderer
        mapCollider = collider;
        mapRenderer = renderer;
    }

    // Update is called once per frame
    // Update Tile Info on MouseOver
    public void UpdateTileInfo()
    {
        // TileInfo.text = "OMG A MOUSE!!!";
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (mapCollider.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            // Get the Tile Coordinates
            int[] coor = ConvertToTileCoordinates(hitInfo.point);
            // Highlight
            // Update the Tile Info
            Debug.Log(coor[0] + ", " + coor[1] + ", " + coor[2]);
        }
        else
        {
            // Un-highlight
        }
    }

    // Update the Display of the Date
    public void UpdateTheDate()
    {
        DateInfo.text = "Day " + (MainController.day + 1) + ", Year " + MainController.year;
    }

    // Private Methods
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

    private int[] ConvertToTileCoordinates(Vector3 point)
    {
        int[] coor = new int[3];
        coor[0] = (int) Math.Truncate(point.x / 5.0);
        coor[1] = (int) Math.Truncate(point.y / 1.0);
        coor[2] = (int) Math.Truncate(point.z / 5.0);
        return coor;
    }
    
}
