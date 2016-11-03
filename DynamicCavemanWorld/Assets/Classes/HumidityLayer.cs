using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HumidityLayer
{
    // Constants
    private const double SPAWN_MULT = .25 * 100.0;
    private const double SPREAD_MULT = 1.0;
    private const float DECAY_CONST = 15.0f;

    // Variables
    public string layerName;
    private string type = "Semi-static";
    private int rounded;
    private int numFiles;
    public static int WORLDX;
    public static int WORLDZ;
    public int[] timeIntervals;
    public float[][,] worldArray;
    public bool spread;

    // Constructor
    public HumidityLayer(string name, int numFiles, int roundTo)
    {
        this.layerName = name;
        this.rounded = roundTo;
        this.numFiles = numFiles;
        WORLDX = SingleValueLayer.WORLDX;
        WORLDZ = SingleValueLayer.WORLDZ;
        worldArray = new float[numFiles][,];
    }

    // Layer initialization Method
    public void readCSVFiles(string filePathPrefix)
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
            this.worldArray[n - 1] = data;
        }   

    }


    // World Rainfall Generation Methods
    public DailyLayer GenerateWorldsYearOfRain()
    {
        DailyLayer yearsRainfall = new DailyLayer("Rainfall", 1);
        float[,] stormArray = new float[WORLDX, WORLDZ];
        float decay = 0f;
        System.Random randy = new System.Random();

        // Run 120 times        
        for (int day = 0; day < 120; day++)
        {
            // Debug.Log(day + "-" + WORLDX + ", " + WORLDZ);
            stormArray = GenerateStormCenters(day, randy);
            // Begin to loop until a storm didn't sucessfully spread
            decay = 0f;
            spread = true;
            while (spread)
            {
                spread = false;
                stormArray = SpreadStorms(stormArray, day, decay);
                // Debug.Log("spread: " + spread);
                decay += DECAY_CONST;
            }
            // Add it to the rainfall daily layer
            yearsRainfall.addWorldDay(day, stormArray);
        }

        return yearsRainfall;
    }

    // Generate Storm Centers
    private float[,] GenerateStormCenters(int day, System.Random randy)
    {
        float[,] stormOrigins = new float[WORLDX, WORLDZ];
        
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                if(randy.Next(0, 10000) <= (CalculateHumidityFromBase(day, x, z) * SPAWN_MULT + 5f))
                {
                    stormOrigins[x, z] = -25f;
                }   
            }
        }

        return stormOrigins;
    }
    
    // Spread the Storms from those centers
    private float[,] SpreadStorms(float[,] stormArray, int day, float decay)
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
                        strength = GenerateSpawnStrength(CalculateHumidityFromBase(day, x, z), randy);
                    }
                    else
                    {
                        strength = -stormArray[x, z];
                    }
                    // Spread to neighbors
                    nextWave = SpreadToCellsAround(day, x, z, stormArray, strength, decay, randy);
                    // Record your own strength
                    nextWave[x, z] = strength;
                }
            }
        }

        return nextWave;
    }

    // Spread to Cells Around Calculation
    private float[,] SpreadToCellsAround(int day, int x, int z, float[,] stormArray, float neighbor, float decay, System.Random randy)
    {
        float[,] nextWave = stormArray;
        
        // Add the four possible values if legal
        if (x != 0 && stormArray[x - 1, z] <= 0)
        {
            nextWave = SpawnCheck(day, x - 1, z, neighbor, stormArray, nextWave, decay, randy);
        }
        if (z != 0 && stormArray[x, z - 1] <= 0)
        {
            nextWave = SpawnCheck(day, x, z - 1, neighbor, stormArray, nextWave, decay, randy);
        }
        if (x != WORLDX - 1 && stormArray[x + 1, z] <= 0)
        {
            nextWave = SpawnCheck(day, x + 1, z, neighbor, stormArray, nextWave, decay, randy);
        }
        if (z != WORLDZ - 1 && stormArray[x, z + 1] <= 0)
        {
            nextWave = SpawnCheck(day, x, z + 1, neighbor, stormArray, nextWave, decay, randy);
        }
        
        return nextWave;
    }

    // get Spawn Strenth
    private float GenerateSpawnStrength(float humidity, System.Random randy)
    {
        double multiplier = Math.Pow(randy.NextDouble(), 2.0);
        float output = (humidity + .1f) * (float) multiplier;
        return (float) Math.Round(output, 1);
    }

    // get Spread Strength
    private float GenerateSpreadStrength(float neighborStrength, float humidity, System.Random randy)
    {
        double addifier = randy.Next(70, 90);
        float output = (2f * humidity) + (float) addifier;
        output = (output / 100f) * neighborStrength;
        // Kill off half the .1's
        if (Math.Round(output, 1) == .1 && randy.Next(0, 10) < 5)
        {
            output = 0f;
        }
        return (float) Math.Round(output, rounded);
    }

    // Add a new spawned square
    private float[,] SpawnCheck(int day, int a, int b, float neighbor, float[,] stormArray, float [,] nextWave, float decay, System.Random randy)
    {
        float spreadChance = CalculateHumidityFromBase(day, a, b) * 8f + 10f - decay;
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
    public float CalculateHumidityFromBase(int day, int x, int z)
    {
        int arrayNum = day / 20;
        int remainder = day % 20;
        int nextNum;
        float humidity;

        // Get the index of the next array
        if(arrayNum == numFiles - 1)
        {
            nextNum = 0;
        }
        else
        {
            nextNum = arrayNum + 1;
        }

        // Use the linear equation formula to find today's humidity
        humidity = (worldArray[nextNum][x, z] - worldArray[arrayNum][x, z]) * (remainder / 20) + worldArray[arrayNum][x, z];
        // Modify it for balance purposes
        humidity = (float) Math.Round(Math.Sqrt(10f * humidity), rounded);
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

    public int getNumFiles()
    {
        return numFiles;
    }

}
