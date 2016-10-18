using UnityEngine;
using UnityEditor;
using NUnit.Framework;

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

        for (int day = 0; day < 5; day++)
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
                if(rainfallTotal.worldArray[a,b] >= 0)
                {
                    positivecount++;
                }
                if(rainfallTotal.worldArray[a, b] > 150)
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
        string row;
        string output = "";
        for (int i = 0; i < 50; i++)
        {
            row = "";
            for (int j = 0; j < 50; j++)
            {
                row += array[i, j] + " ";
            }
            output += row + "\n";
        }
        return output;
    }

}