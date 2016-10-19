using UnityEngine;
using System.Collections;

public class World {

    // Global Layers
    public int WorldX;
    public int WorldZ;
    private float maxElevationDifference;
    // Elevation Layers
    public SingleValueLayer elevation;
    public SingleValueLayer elevationVertices;
    public SingleValueLayer hillPer;
    // Temperature Layers
    private SingleValueLayer highTemp;
    private SingleValueLayer lowTemp;
    private SingleValueLayer tempMidpt;
    private SingleValueLayer variance;
    public EquationLayer tempEquations;
    // Rainfall Layers - (temporarily a very simple version with a single humidity number per tile)
    private HumidityLayer humidity;
    public DailyLayer rainfall;
    public SingleValueLayer rainfallTotal;
    // River Layers
    
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
        this.humidity = new HumidityLayer("HumidityLayer", 6, 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        
        // Elevation info
        elevation.readCSVFile(filePathPrefix + "ElevationNiceMapA.csv");
        ConvertElevationToVertices();
        CalculateHillPercentage();
        // Temperature info
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        tempEquations.createEquations(highTemp, lowTemp, tempMidpt, variance);
        // Rainfall info
        humidity.readCSVFiles(filePathPrefix);
        rainfall = humidity.GenerateWorldsYearOfRain();
        rainfallTotal = new SingleValueLayer("Yearly Rain Total", "Yearly", 1);
        rainfallTotal.worldArray = rainfall.findYearTotalArray();
    }
    
    // Converts the model's elevation number to a map of vertices which can be used by the view
    public void ConvertElevationToVertices()
    {
        for (int x = 0; x < WorldX + 1; x++)
        {
            for (int z = 0; z < WorldZ + 1; z++)
            {
                elevationVertices.worldArray[x, z] = VertexAverage(Support.CellsAroundVertex(x, z, WorldX, WorldZ, this.elevation.worldArray));
            }
        }
    }

    // Private methods!
    // Find the value of the vertex at the grid crossings
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

    // Create the HillPercentage Layer
    private SingleValueLayer CalculateHillPercentage()
    {

    }

    // Calculate the NetDifference around a cell
    private float netDiff(int x, int z)
    {
        float diff = 0f;
        float[,] array = elevation.worldArray;

    }

    // Calculate the maximum netDifference and store it as a private variable.
    private void maxNetDiff()
    {

    }

}
