using UnityEngine;
using System.Collections;
using Boo.Lang;

public class World {

    // Layers
    public int WorldX;
    public int WorldZ;
    public SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
    public SingleValueLayer elevationVertices = new SingleValueLayer("ElevationVertices", "Semi-static", 1);
    private SingleValueLayer highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
    private SingleValueLayer lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
    private SingleValueLayer tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
    private SingleValueLayer variance = new SingleValueLayer("Variance", "Semi-static", 1);
    public EquationLayer tempEquations = new EquationLayer("TemperatureEquations", "Semi-static");
    
    // Constructor
    public World(int x, int z)
    {
        WorldX = x;
        WorldZ = z;
        SingleValueLayer.WORLDX = WorldX;
        SingleValueLayer.WORLDZ = WorldZ;
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
        float average = 0.0f;
        for (int i = 0; i < cellsAround.Length; i++)
        {
            average += cellsAround[i];
        }
        average = average / cellsAround.Length;
        return average;
    }

}
