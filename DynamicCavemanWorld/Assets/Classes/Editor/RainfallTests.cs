﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RainfallTests {

    [Test]
    public void YearOfRainTest()
    {
        // Test the Storm Generation method
        HumidityLayer testEquation = new HumidityLayer("HumidityTests", 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        testEquation.readCSVFile(filePathPrefix + "HumidityNiceMapA.csv");
        DailyLayer rainfall = testEquation.GenerateWorldsYearOfRain();
        int zerocount = 0;
        int positivecount = 0;

        // Make sure all numbers are legal
        for (int a = 0; a < 50; a++)
        {
            for (int b = 0; b < 50; b++)
            {
                for (int i = 0; i < 120; i++)
                {
                    if(rainfall.worldArray[i][a, b] == 0.0f)
                    {
                        zerocount++;
                    }
                    else if(rainfall.worldArray[i][a, b] > 0.0f)
                    {
                        positivecount++;
                    }
                }
            }
        }
        
        Debug.Log("0: " + zerocount + " / +: " + positivecount);
        Assert.AreEqual(120 * 50 * 50, zerocount + positivecount);
        Assert.AreNotSame(120 * 50 * 50, zerocount);

        // Print the first day of rain.
        string row;
        string output = "";
        for (int i = 0; i < 50; i++)
        {
            row = "";
            for (int j = 0; j < 50; j++)
            {
                row += rainfall.worldArray[0][i, j] + " ";
            }
            output += row + "\n";
        }
        Debug.Log(output);
    }

}
