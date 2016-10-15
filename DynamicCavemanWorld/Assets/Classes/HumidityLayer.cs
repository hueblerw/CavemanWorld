using UnityEngine;
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
    public float[,] worldArray = new float[WORLDX, WORLDZ];
    // public float[,,] worldArray = new float[timeIntervals.Length, WORLDX, WORLDZ];
        // THIS IS WHAT THE END GOAL ARRAY LOOKS LIKE
        // FOR NOW WE ARE JUST USING A SINGLE NUMBER FOR THE WHOLE YEAR FOR TESTING PURPOSES

    // Constructor
    public HumidityLayer(string name, int roundTo)
    {
        this.layerName = name;
        this.rounded = roundTo;
    }

    // Layer initialization Method
    public void readCSVFile(string filePath)
    {

        StreamReader sr = new StreamReader(filePath);
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
        this.worldArray = data;

    }


    // World Rainfall Generation Methods
    public void GenerateWorldsYearOfRain()
    {
        DailyLayer yearsRainfall = new DailyLayer("Rainfall", 1);
        int day = 0;
        // Run 120 times
        float[,] stormArray = new float[WORLDX, WORLDZ];
        stormArray = GenerateStormCenters();
        // Begin to recurse
        // Finish recursing
        yearsRainfall.addWorldDay(day, getRainFromStorms(stormArray));
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
                    stormOrigins[x, z] = -1f;
                }   
            }
        }

        return stormOrigins;
    }
    
    // Spread the Storms from those centers
    private float[,] SpreadStorms(int decay, float[,] stormArray)
    {
        System.Random randy = new System.Random();
        float[,] nextWave = stormArray;
        // Spread to Neighbors
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                if(stormArray[x, z] == -1f)
                {
                    nextWave = SpreadToCellsAround(x, z, stormArray, randy, decay);
                }
                // Replace me with my storm strength
                if (decay == 0)
                {
                    nextWave[x, z] = (float)(Math.Pow(randy.NextDouble(), 3) * worldArray[x, z] + .1);
                }
                else
                {
                    // get neighbor value and multiply times stuff
                }
                
            }
        }

        return nextWave;
    }
        
    // Return the rainfall for that day globally
    private float[,] getRainFromStorms(float[,] stormArray)
    {

    }

    // Spread to Cells Around Calculation
    private float[,] SpreadToCellsAround(int x, int z, float[,] stormArray, System.Random randy, int decay)
    {
        float[,] nextWave = stormArray;
        float spreadChance;
        // Add the four possible values if legal
        if (x != 0)
        {
            spreadChance = stormArray[x - 1, z] * 9f + 5f;
            if (randy.Next(0, 100) < spreadChance)
            {
                nextWave[x - 1, z] = -1f;
            }
        }
        if (z != 0)
        {
            spreadChance = stormArray[x, z - 1] * 9f + 5f;
            if (randy.Next(0, 100) < spreadChance)
            {
                nextWave[x, z - 1] = -1f;
            }
        }
        if (x != WORLDX - 1)
        {
            spreadChance = stormArray[x + 1, z] * 9f + 5f;
            if (randy.Next(0, 100) < spreadChance)
            {
                nextWave[x + 1, z] = -1f;
            }
        }
        if (z != WORLDZ + 1)
        {
            spreadChance = stormArray[x, z - 1] * 9f + 5f - decay;
            if (randy.Next(0, 100) < spreadChance)
            {
                nextWave[x, z + 1] = -1f;
            }
        }

        return nextWave;
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
