using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd {

    // variables
    public Vector2[] yearsLocationArray = new Vector2[120];
    public int numOfCreatures;
    public Species species;
    public float herdFatReserve;
    public LocationMemory[] memories;
    // constants
    private const int MIN_HERD_SIZE = 5;
    private const int MAX_HERD_SIZE = 30;

    // constructor
    public Herd(Species mySpecies)
    {
        species = mySpecies;
        // Create a random viable location and a random number of creatures from 5 to 30
        yearsLocationArray[0] = new Vector2(20, 12);
        numOfCreatures = Random.Range(MIN_HERD_SIZE, MAX_HERD_SIZE);
        // Create a location memories array the length of the creatures maxMemory recall.
        memories = new LocationMemory[species.maxMemory];
        // memories[0] = new LocationMemory(location, initFood, 0);
        // Create a starting fat reserve
        herdFatReserve = species.fatReserveNum * numOfCreatures;
    }


    // Calculate migration desire


    // Calculate migration range


    // Create memories
    // Add new location

    
    // Change predator threat

    
    // Location Logic
    // Generate a random appopriate location


    // Create locations for the year


    // To String Method
    public override string ToString()
    {
        string info = "Herd of " + numOfCreatures + " " + species;
        // Testing string info
        info += "\n" + "location: " + location;
        info += " - " + "fat reserve: " + herdFatReserve;
        return info;
    }

}
