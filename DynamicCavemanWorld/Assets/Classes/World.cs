using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World {

    // Global Layers
    public int WorldX;
    public int WorldZ;
    // Elevation Layers
    public SingleValueLayer elevation;
    public SingleValueLayer elevationVertices;
    // Temperature Layers
    private SingleValueLayer highTemp;
    private SingleValueLayer lowTemp;
    private SingleValueLayer tempMidpt;
    private SingleValueLayer variance;
    public EquationLayer tempEquations;
    // Rainfall Layers - (temporarily a very simple version with a single humidity number per tile)
    private SingleValueLayer humidity;
    
    // Constructor
    public World(int x, int z)
    {
        // Initialize the variables
        WorldX = x;
        WorldZ = z;
        SingleValueLayer.WORLDX = WorldX;
        SingleValueLayer.WORLDZ = WorldZ;
        this.elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
        this.elevationVertices = new SingleValueLayer("ElevationVertices", "Semi-static", 1);
        this.highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
        this.lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
        this.tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
        this.variance = new SingleValueLayer("Variance", "Semi-static", 1);
        this.tempEquations = new EquationLayer("TemperatureEquations", "Semi-static");
        this.humidity = new SingleValueLayer("HumidityLayer", "Semi-static", 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        
        // Elevation info
        elevation.readCSVFile(filePathPrefix + "ElevationNiceMapA.csv");
        ConvertElevationToVertices();
        // Temperature info
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        tempEquations.createEquations(highTemp, lowTemp, tempMidpt, variance);
        // Rainfall info
        humidity.readCSVFile(filePathPrefix + "HumidityNiceMapA.csv");
    }
    
    // Converts the model's elevation number to a map of vertices which can be used by the view
    public void ConvertElevationToVertices()
    {
        for (int x = 0; x < WorldX + 1; x++)
        {
            for (int z = 0; z < WorldZ + 1; z++)
            {
                elevationVertices.worldArray[x, z] = VertexAverage(CellsAroundVertex(x, z));
            }
        }
    }

    // Private methods!
    private float[] CellsAroundVertex(int x, int z)
    {
        float[] cells;
        List<float> cellList = new List<float>();
        // Add the four possible values if legal
        if (x < WorldX && z < WorldZ)
        {
            cellList.Add(this.elevation.worldArray[x, z]);
        }   
        if (x > 0 && z > 0)
        {
            cellList.Add(this.elevation.worldArray[x - 1, z - 1]);
        }
        if (x > 0 && z < WorldZ)
        {
            cellList.Add(this.elevation.worldArray[x - 1, z]);
        }
        if (x < WorldX && z > 0)
        {
            cellList.Add(this.elevation.worldArray[x, z - 1]);
        }
        // Convert to an array and return
        cells = cellList.ToArray();
        return cells;
    }

    private float VertexAverage(float[] cellsAround)
    {
        int arrayLength = cellsAround.Length;
        float average = 0.0f;
        for (int i = 0; i < arrayLength; i++)
        {
            average += cellsAround[i];
        }
        average = average / arrayLength;
        return average;
    }

}
