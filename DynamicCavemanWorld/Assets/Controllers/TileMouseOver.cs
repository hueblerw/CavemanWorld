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
            string info = MainController.TheWorld.getTileInfo(MainController.day, coor[0], coor[2]);
            string coorInfo = "(" + coor[0] + ", " + coor[1] + ", " + coor[2] + "):";
            TileInfo.text = "Tile " + coorInfo + "\n" + info;
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
    public Text findTextWithName(string name)
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

    // Convert hitInfo into a tiles coordinates
    private int[] ConvertToTileCoordinates(Vector3 point)
    {
        // Tile info error is something to do with the rotation or the y's!
        // If camera is rotated 90-degrees in y the error reverse itself from being and up-down to left-right displacement error.
        int[] coor = new int[3];
        // Debug.Log(point.x + ", " + point.y + ", " + point.z);
        coor[0] = (int) Math.Truncate(point.x / WorldView.tileSize);
        coor[1] = (int) Math.Truncate(point.y / WorldView.heightScale);
        coor[2] = (int) Math.Truncate(point.z / WorldView.tileSize);
        return coor;
    }

    public void ToggleButtonDisplayName(string current)
    {
        Text button = findTextWithName("ToggleViewButtonText");
        switch (current)
        {
            case "Elevation":
                button.text = "Elevation Layer";
                break;
            case "Habitat":
                button.text = "Habitat Layer";
                break;
        }
    }
    
}
