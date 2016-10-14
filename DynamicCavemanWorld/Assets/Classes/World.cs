using UnityEngine;
using System.Collections;

public class World {

    // Layers
    public SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
    public SingleValueLayer elevationVertices = new SingleValueLayer("ElevationVertices", "Semi-static", 1);
    private SingleValueLayer highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
    private SingleValueLayer lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
    private SingleValueLayer tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
    private SingleValueLayer variance = new SingleValueLayer("Variance", "Semi-static", 1);
    public EquationLayer tempEquations = new EquationLayer("TemperatureEquations", "Semi-static");
    
    // Constructor
    public World()
    {
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        // Elevation info
        elevation.readCSVFile(filePathPrefix + "ElevationNiceMapA.csv");
        // Temperature info
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        tempEquations.createEquations(highTemp, lowTemp, tempMidpt, variance);
        // Rainfall info

    }
    
    // Other methods!
    public void ConvertElevationToVertices()
    {

    }

    // Private methods!
    private float[] CellsAroundVertex(int x, int z)
    {

    }

    private float VertexAverage(float[] cellsAround)
    {
        float average = 0.0f;
        for (int i = 0; i < cellsAround.Length; i++)
        {
            average += cellsAround[i];
        }
        average = average / cellsAround.Length;
        return average;
    }

}
