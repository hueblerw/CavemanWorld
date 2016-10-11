using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    // Layers
    SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
    SingleValueLayer highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
    SingleValueLayer lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
    SingleValueLayer tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
    SingleValueLayer variance = new SingleValueLayer("Variance", "Semi-static", 1);
    EquationLayer tempEquations = new EquationLayer("TemperatureEquations", "Semi-static");

    // Constructor
    public World()
    {
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        // Elevation info
        elevation.readCSVFile(filePathPrefix + "ElevationNiceMapA.csv");
        // Temperature info
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "TempMidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        tempEquations.createEquations();
        // Rainfall info

    }
    
    // Other methods!
}
