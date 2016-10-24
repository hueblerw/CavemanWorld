using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileMouseOver : MonoBehaviour {

    public Text TileInfo;
    public Text TimeInfo;

	// Update is called once per frame
    public TileMouseOver(int day, int year)
    {
        TileInfo.text = "Square Info:";
        TimeInfo.text = "Year: " + year + "        Day: " + day;
    }

    public void UpdateTileInfo()
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
