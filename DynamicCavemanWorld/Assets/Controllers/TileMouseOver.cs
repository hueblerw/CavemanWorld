using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileMouseOver : MonoBehaviour {

    public Text TileInfo;
    public Text TimeInfo;
    public int year;
    public int day;
	// Update is called once per frame
    void Start()
    {
        year = 1;
        day = 1;
        TileInfo.text = "Square Info:";
        TimeInfo.text = "Year: " + year + "        Day: " + day;
    }

	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            UpdateTileInfo();
        }
        else
        {
            // GetComponent<Renderer>().material.color = Color.green;
        }
	}

    void UpdateTileInfo()
    {
        TileInfo.text = "OMG A MOUSE!!!"; 
        // Somehow grab the information from the World object to display here.
            // elevation
            // tempToday
            // rainToday
            // surfaceWaterToday
            // hillPer
            // oceanPer
    }
}
