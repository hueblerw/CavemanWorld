﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HumidityLayer
{
    // Constants
    private const double SPAWN_MULT = .1 * 100.0;
    private const double SPREAD_MULT = 1.0;

    // Variables
    public string layerName;
    private string type = "Semi-static";
    private int rounded;
    public static int WORLDX = SingleValueLayer.WORLDX;
    public static int WORLDZ = SingleValueLayer.WORLDZ;
    public int[] timeIntervals;
    public float[][,] worldArray = new float[6][,];
    public bool spread;

    // Constructor
    public HumidityLayer(string name, int roundTo)
    {
        this.layerName = name;
        this.rounded = roundTo;
    }

    // Layer initialization Method
    public void readCSVFiles(string filePathPrefix, int numFiles)
    {

        for (int n = 1; n < numFiles + 1; n++)
        {
            StreamReader sr = new StreamReader(filePathPrefix + "HumidityNiceMapA-" + n + ".csv");
            float[,] data = new float[WORLDX, WORLDZ];
            int Row = 0;
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(',');
                for (int i = 0; i < Line.Length; i++)
                {
                    data[i, Row] = (float)Convert.ToDouble(Line[i]);
                }
                Row++;
            }
            this.worldArray[n] = data;
        }   

    }


    // World Rainfall Generation Methods
    public DailyLayer GenerateWorldsYearOfRain()
    {
        DailyLayer yearsRainfall = new DailyLayer("Rainfall", 1);
        float[,] stormArray = new float[WORLDX, WORLDZ];
        float decay = 0f;

        // Run 120 times        
        for (int day = 0; day < 120; day++)
        {
            stormArray = GenerateStormCenters();
            // Begin to loop until a storm didn't sucessfully spread
            spread = true;
            while (spread)
            {
                spread = false;
                stormArray = SpreadStorms(stormArray, decay);
                // Debug.Log("spread: " + spread);
                decay += 15f;
            }
            // Add it to the rainfall daily layer
            yearsRainfall.addWorldDay(day, stormArray);
        }

        return yearsRainfall;
    }

    // Generate Storm Centers
    private float[,] GenerateStormCenters()
    {
        float[,] stormOrigins = new float[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                if(randy.Next(0, 10000) <= (worldArray[x, z] * SPAWN_MULT))
                {
                    stormOrigins[x, z] = -25f;
                }   
            }
        }

        return stormOrigins;
    }
    
    // Spread the Storms from those centers
    private float[,] SpreadStorms(float[,] stormArray, float decay)
    {
        System.Random randy = new System.Random();
        float[,] nextWave = new float[WORLDX, WORLDZ];
        float strength;
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                if(stormArray[x, z] < 0)
                {
                    // Generate the present strength
                    if(stormArray[x, z] == -25f)
                    {
                        strength = GenerateSpawnStrength(worldArray[x, z], randy);
                    }
                    else
                    {
                        strength = -stormArray[x, z];
                    }
                    // Spread to neighbors
                    nextWave = SpreadToCellsAround(x, z, stormArray, strength, decay, randy);
                    // Record your own strength
                    nextWave[x, z] = strength;
                }
            }
        }

        return nextWave;
    }

    // Spread to Cells Around Calculation
    private float[,] SpreadToCellsAround(int x, int z, float[,] stormArray, float neighbor, float decay, System.Random randy)
    {
        float[,] nextWave = stormArray;
        
        // Add the four possible values if legal
        if (x != 0 && stormArray[x - 1, z] <= 0)
        {
            nextWave = SpawnCheck(x - 1, z, neighbor, stormArray, nextWave, decay, randy);
        }
        if (z != 0 && stormArray[x, z - 1] <= 0)
        {
            nextWave = SpawnCheck(x, z - 1, neighbor, stormArray, nextWave, decay, randy);
        }
        if (x != WORLDX - 1 && stormArray[x + 1, z] <= 0)
        {
            nextWave = SpawnCheck(x + 1, z, neighbor, stormArray, nextWave, decay, randy);
        }
        if (z != WORLDZ - 1 && stormArray[x, z + 1] <= 0)
        {
            nextWave = SpawnCheck(x, z + 1, neighbor, stormArray, nextWave, decay, randy);
        }
        
        return nextWave;
    }

    // get Spawn Strenth
    private float GenerateSpawnStrength(float humidity, System.Random randy)
    {
        double multiplier = Math.Pow(randy.NextDouble(), 3.0);
        float output = (humidity + .1f) * (float) multiplier;
        return (float) Math.Round(output, 1);
    }

    // get Spread Strength
    private float GenerateSpreadStrength(float neighborStrength, float humidity, System.Random randy)
    {
        double addifier = randy.Next(20, 50);
        float output = (3f * humidity) + (float)addifier;
        output = (output / 100f) * neighborStrength;
        return (float) Math.Round(output, 1);
    }

    // Add a new spawned square
    private float[,] SpawnCheck(int a, int b, float neighbor, float[,] stormArray, float [,] nextWave, float decay, System.Random randy)
    {
        float spreadChance = worldArray[a, b] * 9f + 5f - decay;
        if (randy.Next(0, 100) < spreadChance * SPREAD_MULT)
        {
            float strength = -GenerateSpreadStrength(neighbor, stormArray[a, b], randy);
            if (strength < 0)
            {
                nextWave[a, b] = strength;
                spread = true;
            }
        }

        return nextWave;
    }

    // Need a method to calculate the square's humidity number based upon the day of the year
    private float CalculateHumidityFromBase(int day, int x, int z)
    {
        int arrayNum = day / 20;
        int remainder = day % 20;
        int nextNum;
        float humidity;

        // Get the index of the next array
        if(arrayNum == 5)
        {
            nextNum = 0;
        }
        else
        {
            nextNum = arrayNum + 1;
        }

        // Use the linear equation formula to find today's humidity
        humidity = (worldArray[nextNum][x, z] - worldArray[arrayNum][x, z]) * (remainder / 20) + worldArray[arrayNum][x, z];
        return humidity;
    }

    // Getter methods
    public string getType()
    {
        return type;
    }

    public int getRounding()
    {
        return rounded;
    }

}
