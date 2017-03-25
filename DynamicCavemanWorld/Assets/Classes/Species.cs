using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species {

    // Static Variables
    public static int numOfSpecies;

    // Variables
    public int id;
    public string name;
    public float foodRequired;
    public int[] foodTypeIndicies;
    public float meatValue;
    public int fatReserveNum;
    public int attack;
    public int defense;
    public float speed;
    public Vector2[] habitatTolerance = new Vector2[World.NUM_OF_HABITAT_TYPES];
    public bool domesticable;
    public int maxMemory;
    public float reproductionRate;
    public float deathRate;

    // constructor for an individual species
    // Reads in the tab delimited string passed in from file.
    public Species (string tabDelineatedString)
    {
        string[] traits = tabDelineatedString.Split('\t');
        // set the straight forward variables
        id = Convert.ToInt32(traits[0]);
        name = traits[1];
        meatValue = (float) Convert.ToDouble(traits[3]);
        fatReserveNum = Convert.ToInt32(traits[4]);
        attack = Convert.ToInt32(traits[5]);
        defense = Convert.ToInt32(traits[6]);
        speed = (float)Convert.ToDouble(traits[7]);
        maxMemory = Convert.ToInt32(traits[10]);
        reproductionRate = (float)Convert.ToDouble(traits[11].Replace("%", string.Empty));
        deathRate = (float)Convert.ToDouble(traits[12].Replace("%", string.Empty));
        // Set the food variables
        traits[2] = traits[2].Replace("\"", string.Empty);
        string[] temps = traits[2].Split('-');
        foodRequired = (float)Convert.ToDouble(temps[0]);
        string[] tempsTwo = temps[1].Split(',');
        foodTypeIndicies = GetFoodTypeIndices(tempsTwo);
        // Set the habitat preferences
        habitatTolerance = SetHabitatTolerance(traits[8]);
        // Convert to domesticatible
        if (traits[9] == "TRUE")
        {
            domesticable = true;
        }
        else 
        {
            domesticable = false;
        }
        Debug.Log(traits[9]);
    }


    // STATIC METHODS
    // Read in the entire array of species from file and parse them into single line strings
    public static Species[] ReadSpeciesFromFile()
    {
        string[] potentialSpecies = LoadTextStringsFromFile();
        numOfSpecies = potentialSpecies.Length;
        Species[] listOfSpecies = new Species[numOfSpecies];
        for (int i = 0; i < numOfSpecies; i++)
        {
            listOfSpecies[i] = new Species(potentialSpecies[i]);
        }
        return listOfSpecies;
    }


    private static string[] LoadTextStringsFromFile()
    {
        // Load from file
        TextAsset TSVFile = Resources.Load(@"CSV\Species_Stats") as TextAsset;
        string[] potentialSpecies = TSVFile.text.Split('\n');
        // Filter out comment lines
        List<string> stringList = new List<string>();
        for (int i = 0; i < potentialSpecies.Length; i++)
        {
            // if first character is not a '#' add to the String List
            if (!potentialSpecies[i].StartsWith("#"))
            {
                stringList.Add(potentialSpecies[i]);
            }
        }
        potentialSpecies = stringList.ToArray();
        return potentialSpecies;
    }


    // INSTANCE METHODS
    // Calculate and return the reproduction rate for the species instance
    private int[] GetFoodTypeIndices(string[] tempsTwo)
    {
        int[] indexes;
        // Food types
        // 0 - grazing
        // 1 - scrub
        // 2 - foilage
        // 3 - seeds
        // 4 - gather
        // 5 - meat
        

        return indexes;
    }


    private Vector2[] SetHabitatTolerance(string habitatString)
    {
        Vector2[] habitatAvoidance = new Vector2[World.NUM_OF_HABITAT_TYPES];

        // STUFF

        return habitatAvoidance;
    }
}
