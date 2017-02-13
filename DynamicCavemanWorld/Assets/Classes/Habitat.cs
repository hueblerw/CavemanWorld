﻿using UnityEngine;
using System.Collections;

public class Habitat {

    // Constant
    private double EnvironmentalShiftFactor = .01; // +/- 1% a year
    private float RiverEffectFactor = .1f;  // 10% of river volume added to the tiles rainfall

    // Variables
    public string dominantType;
    public double[] typePercents;
    public float percentOcean;
    private int activeHabitats;

    // Constructor
    public Habitat(int[] habitatCounters, float oceanPer)
    {
        typePercents = new double[13];
        percentOcean = oceanPer;
        if(percentOcean != 1f)
        {
            CreateInitialPercentage(habitatCounters);
        }
        // Once the habitats have been loaded figure out which one is dominant
        dominantType = CheckDominantType();
    }

    // Habitat Yearly Update Method
    public void UpdateHabitatYear(int hotDays, int coldDays, float rain, float riverLevel, bool snowCovered, System.Random randy)
    {
        // if all ocean don't change it
        if (percentOcean != 1f)
        {
            int index;
            int destroyIndex;
            // if covered in snow glacier advances
            if (snowCovered)
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
                    // Update what is the dominant type if the type was not previously a majority - i.e. > 50% of the habitat
                    if (typePercents[index] < 0.51)
                    {
                        dominantType = CheckDominantType();
                    }
                }
            }
        } 
    }

    // Return a string with the habitat stats in it
    public override string ToString()
    {
        string data = "Habitats: ";
        for (int i = 0; i < typePercents.Length; i++){
            if(typePercents[i] != 0.0)
            {
                data += "\n" + typePercents[i] * 100.0 + "% " + IndexToString(i);
            }
        }
        return data;
    }


    // private methods
    // Wetness determination based on the year's rainfall
    public static string DetermineWetness(float water)
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
    public static string DetermineTemp(int hotDays, int coldDays)
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
    public static int DetermineHabitatFavored(string wetness, string temp)
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
            if (prob < percentCounter)
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

    // Convert the index to a habitat string
    private string IndexToString(int index)
    {
        string name = "";
        switch (index)
        {
            case 0:
                name = "glacier";
                break;
            case 1:
                name = "dry tundra";
                break;
            case 2:
                name = "tundra";
                break;
            case 3:
                name = "boreal";
                break;
            case 4:
                name = "artic marsh";
                break;
            case 5:
                name = "desert";
                break;
            case 6:
                name = "plains";
                break;
            case 7:
                name = "forest";
                break;
            case 8:
                name = "swamp";
                break;
            case 9:
                name = "hot desert";
                break;
            case 10:
                name = "savannah";
                break;
            case 11:
                name = "monsoon forest";
                break;
            case 12:
                name = "rainforest";
                break;
            case 13:
                name = "ocean";
                break;
        }

        return name;
    }

    // determine the dominant type
    private string CheckDominantType()
    {
        return IndexToString(GetDominantIndex());
    }


    // get the index for the dominant habitat
    public int GetDominantIndex()
    {
        if (percentOcean != 1)
        {
            int maxIndex = 0;
            for (int i = 0; i < typePercents.Length; i++)
            {
                if (typePercents[i] > typePercents[maxIndex])
                {
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
        else
        {
            return 13;
        }
    }


    // Initialize the Habitats based on the counter array
    private void CreateInitialPercentage(int[] habitatCounters)
    {
        for (int i = 0; i < habitatCounters.Length; i++)
        {
            typePercents[i] = habitatCounters[i] / 20.0;
        }
    }

}
