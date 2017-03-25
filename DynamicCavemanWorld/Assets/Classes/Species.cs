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

    }


    // STATIC METHODS
    // Read in the entire array of species from file and parse them into single line strings
    public static Species[] ReadSpeciesFromFile()
    {
        string[] potentialSpecies = LoadTextStringsFromFile();
        numOfSpecies = potentialSpecies.Length;
        Species listOfSpecies = new Species[numOfSpecies];

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
            
        }
        potentialSpecies = stringList.ToArray();
        return potentialSpecies;
    }


    // INSTANCE METHODS
    // Calculate and return the reproduction rate for the species instance

    
}
