using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NewTester {

	[Test]
	public void SVLayerTest()
	{
        // Test all the SingleLayer Values can be initialized
        SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
        string filePath = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\ElevationNiceMapA.csv";
        elevation.readCSVFile(filePath);

        Assert.AreEqual("Elevation", elevation.layerName);
        Assert.AreEqual("Semi-static", elevation.getType());
        Assert.AreEqual(1, elevation.getRounding());
        Assert.AreEqual(50, SingleValueLayer.WORLDX);
        Assert.AreEqual(50, SingleValueLayer.WORLDZ);
        Assert.AreEqual((float)-2.7, elevation.worldArray[1, 0]);
        Assert.AreEqual((float)-0.6, elevation.worldArray[0, 1]);

    }

    [Test]
    public void DailyLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        DailyLayer rainfall = new DailyLayer("Rainfall", 1);

        Assert.AreEqual("Rainfall", rainfall.layerName);
        Assert.AreEqual("Daily", rainfall.getType());
        Assert.AreEqual(1, rainfall.getRounding());
        Assert.AreEqual(120 * 50 * 50, rainfall.worldArray.Length);
        Assert.AreEqual(50, DailyLayer.WORLDX);
        Assert.AreEqual(50, DailyLayer.WORLDZ);
    }

    [Test]
    public void VariantLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        VariantLayer humidity = new VariantLayer("Humidity", "Semi-static", 6, 1);

        Assert.AreEqual("Humidity", humidity.layerName);
        Assert.AreEqual("Semi-static", humidity.getType());
        Assert.AreEqual(1, humidity.getRounding());
        Assert.AreEqual(6 * 50 * 50, humidity.worldArray.Length);
        Assert.AreEqual(50, VariantLayer.WORLDX);
        Assert.AreEqual(50, VariantLayer.WORLDZ);
    }

    [Test]
    public void EquationLayerTest()
    {
        // Test the TempEquation Constructor
        EquationLayer testEquation = new EquationLayer("TemperatureEquations", "Semi-static");

        Assert.AreEqual("TemperatureEquations", testEquation.layerName);
        Assert.AreEqual("Semi-static", testEquation.getType());
        Assert.AreEqual(50, VariantLayer.WORLDX);
        Assert.AreEqual(50, VariantLayer.WORLDZ);
    }

    [Test]
    public void EquationCreationTest()
    {
        // Test initialize what is needed for the test
        EquationLayer testEquation = new EquationLayer("TemperatureEquations", "Semi-static");
        SingleValueLayer highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
        SingleValueLayer lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
        SingleValueLayer tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
        SingleValueLayer variance = new SingleValueLayer("Variance", "Semi-static", 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        variance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        testEquation.createEquations(highTemp, lowTemp, tempMidpt, variance);

        // Test conditions
        Assert.AreEqual(50 * 50, testEquation.worldArray.Length);

    }

}
