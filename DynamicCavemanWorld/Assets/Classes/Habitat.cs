using UnityEngine;
using System.Collections;

public class Habitat {

    // Constant
    private float EnvironmentalShiftFactor = .01f;

    // Variables
    public string dominantType;
    public float[] typePercents;

    // Constructor
    public Habitat()
    {

    }

    // Habitat Yearly Update Method
    public void UpdateHabitatYear(int hotDays, int coldDays, float rain, int snowCovered)
    {

        // Wetness determination statement
        string wetness;
        if(rain < 20f)
        {
            wetness = "dry";
        }
        else
        {
            if (rain < 40f)
            {
                wetness = "moderate";
            }
            else
            {
                if (rain < 60f)
                {
                    wetness = "wet";
                }
                else
                {
                    wetness = "very wet";
                }
            }
        }

        // Temperature ifs
        string temperature = "temperate";
        if (hotDays > 40 && coldDays < 10)
        {
            temperature = "tropical";
        }
        else {
            if (hotDays < 10 && coldDays > 40)
            {
                temperature = "artic";
            }
        }

        // Get the favored habitat

        
    }
}
