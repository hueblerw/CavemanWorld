using UnityEngine;
using System.Collections;

public class World {

    // Layers
    public SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
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
        tempEquations.createEquations();
        // Rainfall info

    }
    
    // Other methods!
}
