using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HumidityLayer
{
    // Constants
    private const double SPAWN_MULT = .01;
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
        // Run 120 times
        string[,] stormArray = new string[WORLDX, WORLDZ];
        stormArray = GenerateStormCenters();
        // Begin to recurse
        // Finish recursing
        yearsRainfall.addWorldDay(getRainFromStorms(stormArray));
    }

    // Generate Storm Centers
    private string[,] GenerateStormCenters()
    {

    }
    
    // Spread the Storms from those centers
    private string[,] SpreadStorms(string[,] stormArray)
    {
        // Recurse the Storm spread
    }
        
    // Return the rainfall for that day globally
    private float[,] getRainFromStorms(string[,] stormArray)
    {

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
