﻿using UnityEngine;
using System.Collections;
using System;

public class World {

    // Global Layers
    public int WorldX;
    public int WorldZ;
    public float maxElevationDifference;
    // Elevation Layers
    public SingleValueLayer elevation;
    public SingleValueLayer elevationVertices;
    public SingleValueLayer hillPer;
    public SingleValueLayer oceanPer;   
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
    public ObjectLayer riverStats;

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
        hillPer = CalculateHillPercentage();
        oceanPer = CalculateOceanPercentage();
        Debug.Log("Elevation Models Complete!");
        // Temperature info
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        tempEquations.createEquations(highTemp, lowTemp, tempMidpt, variance);
        Debug.Log("Temperature Models Complete!");
        // Rainfall info
        humidity.readCSVFiles(filePathPrefix);
        rainfall = humidity.GenerateWorldsYearOfRain();
        rainfallTotal = new SingleValueLayer("Yearly Rain Total", "Yearly", 1);
        rainfallTotal.worldArray = rainfall.findYearTotalArray();
        Debug.Log("Rainfall Models Complete!");
        // Rivers info
        riverStats = new ObjectLayer("River Stats", "Semi-static");
            // Write a method that populates rivers with a full array of river objects
        River.upstreamToday = new DailyLayer("Upstream Waterflow", 2);
        River.surfacewater = new DailyLayer("Surface Water", 2);
        // Initialize Water Stats
            // Calculate Years worth of river data
        // Debug.Log("River Models Complete!");
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
        // Initialize a new layer
        float diff;
        SingleValueLayer hillPer = new SingleValueLayer("Hill Percentage", "Semi-static", 4);
        // Stash the max elevation difference
        maxNetDiff();
        // Calculate the hill %'s for the whole world
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                diff = netDiff(x, z);
                hillPer.worldArray[x, z] = (float) Math.Round(diff / maxElevationDifference, 4);
            }
        }
        return hillPer;
    }

    // Create the Ocean Percentage Layer
    private SingleValueLayer CalculateOceanPercentage()
    {
        // Initialize a new layer
        SingleValueLayer hillPer = new SingleValueLayer("Ocean Percentage", "Semi-static", 4);
        // Find the vertexes, sum th abs of all negative values and divide by sum of abs of all values.
        float[,] vertexArray = elevationVertices.worldArray;
        float[] corners;
        float sumOfNegatives;
        float sumOfAll;

        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                corners = Support.CellsAroundVertex(x + 1, z + 1, WorldX, WorldZ, vertexArray);
                sumOfNegatives = FindAbsOfNegatives(corners);
                sumOfAll = FindAbsSum(corners);
                hillPer.worldArray[x, z] = sumOfNegatives / sumOfAll;
            }
        }

        return hillPer;
    }

    // Calculate the NetDifference around a cell
    private float netDiff(int x, int z)
    {
        float diff = 0f;
        float[,] array = elevation.worldArray;
        float[] cellsAround = Support.CellsAllAround(x, z, WorldX, WorldZ, array);
        foreach (float element in cellsAround)
        {
            diff += Mathf.Abs(array[x, z] - element);
        }
        return diff / cellsAround.Length;
    }

    // Calculate the maximum netDifference and store it as a private variable.
    private void maxNetDiff()
    {
        float maxDiff = 0f;
        float diff;
        for (int x = 0; x < WorldX; x++)
        {
            for(int z = 0; z < WorldZ; z++)
            {
                diff = netDiff(x, z);
                if (diff > maxDiff)
                {
                    maxDiff = diff;
                }
            }
        }
        maxElevationDifference = maxDiff;
    }

    // Calculate the abs of the sum of all negative numbers in the array
    private float FindAbsOfNegatives(float[] corners)
    {
        float sum = 0f;
        foreach (float ele in corners)
        {
            if (ele > 0)
            {
                sum += ele;
            }
        }
        return Math.Abs(sum);
    }

    // Calculate the sum of the Absolute Value of all numbers
    private float FindAbsSum(float[] corners)
    {
        float sum = 0f;
        foreach (float ele in corners)
        {
            sum += Math.Abs(ele);
        }
        return sum;
    }
}
