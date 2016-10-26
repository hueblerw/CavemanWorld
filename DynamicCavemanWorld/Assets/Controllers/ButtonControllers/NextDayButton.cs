using UnityEngine;
using System.Collections;

public class NextDayButton : MonoBehaviour {

	void OnClick()
    {
        if (MainController.day == 119)
        {
            MainController.day = 0;
            MainController.year += 1;
        }
        else
        {
            MainController.day += 1;
        }
        // Do we need to make the TileMouseController reload?
        Debug.Log("New Day!");
    }

}
