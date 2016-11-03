using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class RainfallTests
{

    [Test]
    public void EarlyHumidityTest()
    {
        HumidityLayer.WORLDX = 50;
        HumidityLayer.WORLDZ = 50;
        // Test the single value Humidity Layer so I can get a sense for how the rainfall logic will work
        HumidityLayer testHumidityLayer = new HumidityLayer("Humidity", 6, 1);
        string filePath = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        testHumidityLayer.readCSVFiles(filePath);

        Assert.AreEqual("Humidity", testHumidityLayer.layerName);
        Assert.AreEqual("Semi-static", testHumidityLayer.getType());
        Assert.AreEqual(1, testHumidityLayer.getRounding());
        Assert.AreEqual(6, testHumidityLayer.getNumFiles());
        Assert.AreEqual(6, testHumidityLayer.worldArray.Length);
        Assert.AreEqual(50, HumidityLayer.WORLDX);
        Assert.AreEqual(50, HumidityLayer.WORLDZ);
    }

    [Test]
    public void YearOfRainTest()
    {
        // Test the Storm Generation method
        HumidityLayer testEquation = new HumidityLayer("HumidityTests", 6, 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        testEquation.readCSVFiles(filePathPrefix);
        DailyLayer rainfall = testEquation.GenerateWorldsYearOfRain();
        int zerocount = 0;
        int positivecount = 0;
        System.Random randy = new System.Random();
        int x = randy.Next(0, 50);
        int z = randy.Next(0, 50);
        int daye = randy.Next(0, 120);

        // Make sure all numbers are legal
        for (int a = 0; a < 50; a++)
        {
            for (int b = 0; b < 50; b++)
            {
                for (int i = 0; i < 120; i++)
                {
                    if (rainfall.worldArray[i][a, b] == 0.0f)
                    {
                        zerocount++;
                    }
                    else if (rainfall.worldArray[i][a, b] > 0.0f)
                    {
                        positivecount++;
                    }
                }
            }
        }

        // Debug.Log("0: " + zerocount + " / +: " + positivecount);
        Assert.AreEqual(120 * 50 * 50, zerocount + positivecount);
        Assert.AreNotSame(120 * 50 * 50, zerocount);
        Assert.GreaterOrEqual(testEquation.CalculateHumidityFromBase(daye, x, z), 0.0f);
        Assert.LessOrEqual(testEquation.CalculateHumidityFromBase(daye, x, z), 10.0f);

        // Print the first day of rain.

        for (int day = 10; day < 30; day++)
        {
            Debug.Log("Day " + day);
            Debug.Log(printArray(rainfall.worldArray[day]));
        }

    }

    [Test]
    public void TestYearlyRainfallLayer()
    {
        HumidityLayer testEquation = new HumidityLayer("HumidityTests", 6, 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        testEquation.readCSVFiles(filePathPrefix);
        DailyLayer rainfall = testEquation.GenerateWorldsYearOfRain();
        SingleValueLayer rainfallTotal = new SingleValueLayer("Yearly Rain Total", "Yearly", 1);
        rainfallTotal.worldArray = rainfall.findYearTotalArray();
        int positivecount = 0;
        int excesscount = 0;

        for (int a = 0; a < 50; a++)
        {
            for (int b = 0; b < 50; b++)
            {
                if (rainfallTotal.worldArray[a, b] >= 0)
                {
                    positivecount++;
                }
                if (rainfallTotal.worldArray[a, b] > 150)
                {
                    excesscount++;
                }
            }
        }

        Assert.AreEqual(50 * 50, rainfallTotal.worldArray.Length);
        Assert.AreEqual(50 * 50, positivecount);
        Assert.AreEqual(0, excesscount);

        Debug.Log(printArray(rainfallTotal.worldArray));

    }

    // method to display a 2 x 2 array of floats
    private string printArray(float[,] array)
    {
        float min = array[0, 0];
        float max = 0f;
        string row;
        string output = "";
        for (int i = 0; i < 50; i++)
        {
            row = "";
            for (int j = 0; j < 50; j++)
            {
                row += array[i, j] + " ";
                if (array[i, j] > max)
                {
                    max = array[i, j];
                }
                if (array[i, j] < min)
                {
                    min = array[i, j];
                }
            }
            output += row + "\n";
        }

        Debug.Log("Yearly Rain -- Min: " + min + " / Max: " + max);
        return output;
    }

    [Test]
    public void RiverLayerCreationTest()
    {
        // Initialize a world
        World testWorld = new World(50, 50, false);
        System.Random randy = new System.Random();
        int x = randy.Next(0, 50);
        int z = randy.Next(0, 50);
        River testRiver = testWorld.riverStats.worldArray[x, z];

        // Downstream shit if applicable
        if (testWorld.oceanPer.worldArray[x, z] != 1f && testRiver.downstream != null)
        {
            Debug.Log("Downstream: " + testRiver.downstream.direction);
            Assert.GreaterOrEqual(testRiver.downstream.getCoordinateArray(x, z)[0], x - 1);
            Assert.LessOrEqual(testRiver.downstream.getCoordinateArray(x, z)[0], x + 1);
            Assert.GreaterOrEqual(testRiver.downstream.getCoordinateArray(x, z)[1], z - 1);
            Assert.LessOrEqual(testRiver.downstream.getCoordinateArray(x, z)[1], z + 1);
            // Does the river flow uphill?
            Debug.Log("My Elevation: " + testWorld.elevation.worldArray[x, z]);
            Debug.Log("Elevation Above: " + testWorld.elevation.worldArray[x, z - 1]);
            Debug.Log("Elevation Below: " + testWorld.elevation.worldArray[x, z + 1]);
            Debug.Log("Elevation Right: " + testWorld.elevation.worldArray[x + 1, z]);
            Debug.Log("Elevation Left: " + testWorld.elevation.worldArray[x - 1, z]);
            List<string> possible = Support.getDirectionAsStringBelow(false, x, z, 50, 50, testWorld.elevation.worldArray[x, z], testWorld.elevation.worldArray);
            foreach (string dir in possible)
            {
                Debug.Log("Possible: " + dir);
            }
        }
        // Upstream shit
        Debug.Log("Upstream: " + testRiver.printUpstream());
        foreach (Direction cell in testRiver.upstream)
        {
            // Do the coordinates make sense?
            Assert.GreaterOrEqual(cell.getCoordinateArray(x, z)[0], x - 1);
            Assert.LessOrEqual(cell.getCoordinateArray(x, z)[0], x + 1);
            Assert.GreaterOrEqual(cell.getCoordinateArray(x, z)[1], z - 1);
            Assert.LessOrEqual(cell.getCoordinateArray(x, z)[1], z + 1);
        }

        Debug.Log("Square: (" + x + ", " + z + ")");
        if(testRiver.downstream != null)
        {
            Debug.Log("Downstream: " + testRiver.downstream.direction);
        }
        
        // Ok do the static variables come out as plausible?
        for (int day = 0; day < 120; day++)
        {
            Debug.Log("day: " + day + " - " + River.surfacewater.worldArray[day][x, z]);
            Assert.GreaterOrEqual(River.surfacewater.worldArray[day][x, z], 0f);
            Assert.GreaterOrEqual(River.upstreamToday.worldArray[day][x, z], 0f);
            Assert.GreaterOrEqual(River.lastUpstreamDay.worldArray[x, z], 0f);
        }
        
    }

}