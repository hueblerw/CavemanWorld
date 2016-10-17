using UnityEngine;
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
        int x = 0;
        int z = 0;

        // Make sure all numbers are legal
        for (int i = 0; i < 120; i++)
        {
            if(rainfall.worldArray[i][x, z] == 0.0f)
            {
                zerocount++;
            }
            else if(rainfall.worldArray[i][x, z] > 0.0f)
            {
                positivecount++;
            }
        }

        Debug.Log("0: " + zerocount + " / +: " + positivecount);
        Assert.AreEqual(120, zerocount + positivecount);
        // Assert.AreNotSame(120, zerocount);

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
