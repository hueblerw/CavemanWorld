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
        typePercents = new float[13];
    }

    // Habitat Yearly Update Method
    public void UpdateHabitatYear(int hotDays, int coldDays, float rain, int snowCovered)
    {
        int index;
        if (snowCovered == 120)
        {
            index = 0;
        }
        else
        {
            // Wetness determination statement
            string wetness = DetermineWetness(rain);
            string temp = DetermineTemp(hotDays, coldDays);
            // Get the favored habitat
            index = DetermineHabitatFavored(wetness, temp);
        }

    }


    // private methods
    // Wetness determination based on the year's rainfall
    private string DetermineWetness(float rain)
    {
        string wetness;
        if (rain < 20f)
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

        return wetness;
    }

    // Tempearteness determination based on the year's hot and coldays
    private string DetermineTemp(int hotDays, int coldDays)
    {
        // Temperature ifs
        string temperature = "temperate";
        if (hotDays > 40 && coldDays < 10)
        {
            temperature = "tropical";
        }
        else
        {
            if (hotDays < 10 && coldDays > 40)
            {
                temperature = "artic";
            }
        }

        return temperature;
    }

    // Determine the Index of the habitat the weather favored this year
    private int DetermineHabitatFavored(string wetness, string temp)
    {
        int index = 0;
        // account for temp
        switch (temp)
        {
            case "artic":
                index = 0;
                break;
            case "temperate":
                index = 4;
                break;
            case "tropical":
                index = 8;
                break;
        }
        // account for wetness
        switch (wetness)
        {
            case "dry":
                index += 1;
                break;
            case "moderate":
                index += 2;
                break;
            case "wet":
                index += 3;
                break;
            case "very wet":
                index += 4;
                break;
        }

        return index;
    }
}
