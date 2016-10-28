using UnityEngine;
using System.Collections;

public class Habitat {

    // Constant
    private double EnvironmentalShiftFactor = .01; // +/- 1% a year
    private float RiverEffectFactor = .1f;  // 10% of river volume added to the tiles rainfall

    // Variables
    public string dominantType;
    public double[] typePercents;

    // Constructor
    public Habitat()
    {
        typePercents = new double[13];
    }

    // Habitat Yearly Update Method
    public void UpdateHabitatYear(int hotDays, int coldDays, float rain, float riverLevel, int snowCovered, System.Random randy)
    {
        int index;
        int destroyIndex;
        // if covered in snow glacier advances
        if (snowCovered == 120)
        {
            // do snow stuff - *** UNWRITTEN YET!!! ***
        }
        else
        {
            // Wetness determination statement
            string wetness = DetermineWetness(rain + riverLevel * RiverEffectFactor);
            string temp = DetermineTemp(hotDays, coldDays);
            // Get the favored habitat
            index = DetermineHabitatFavored(wetness, temp);
            // Update the habitat array if not maxed out
            if (typePercents[index] < 1.0)
            {
                typePercents[index] += EnvironmentalShiftFactor;
                // Generate the random destruction
                destroyIndex = RandomHabitat(randy);
                typePercents[destroyIndex] -= EnvironmentalShiftFactor;
            }   
        }
    }


    // private methods
    // Wetness determination based on the year's rainfall
    private string DetermineWetness(float water)
    {
        string wetness;
        if (water < 20f)
        {
            wetness = "dry";
        }
        else
        {
            if (water < 40f)
            {
                wetness = "moderate";
            }
            else
            {
                if (water < 60f)
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

    // Generate a random habitat to destroy
    private int RandomHabitat(System.Random randy)
    {
        double prob = randy.NextDouble();
        double percentCounter = 0.0;
        int i = 0;
        bool done = false;
        while (!done)
        {
            percentCounter += typePercents[i];
            if (percentCounter < prob)
            {
                done = true;
            }
            else
            {
                i++;
            }
        }

        return i;
    }

}
