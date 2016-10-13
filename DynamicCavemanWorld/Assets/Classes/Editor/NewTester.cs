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
    public void TempEquationTest()
    {
        // Test the TempEquation
        TempEquation testEquation = new TempEquation(80, 20, (float)25.0, (float)6.0);

        // Test the TempEquation Constructor
        Assert.AreEqual(0.0, testEquation.returnConstants()[0]);
        Assert.AreEqual(-26.5, testEquation.returnConstants()[1]);
        Assert.AreEqual(52.5, testEquation.returnConstants()[2]);
        Assert.AreEqual(-31.5, testEquation.returnConstants()[3]);
        Assert.AreEqual(5.25, testEquation.returnConstants()[4]);
        Assert.AreEqual(1.0, testEquation.returnConstants()[5]);
    }

    [Test]
    public void EquationCreationTest()
    {

        // Test initialize what is needed for the test
        EquationLayer testEquation = new EquationLayer("TemperatureEquations", "Semi-static");
        SingleValueLayer testhighTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
        SingleValueLayer testlowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
        SingleValueLayer testtempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
        SingleValueLayer testvariance = new SingleValueLayer("Variance", "Semi-static", 1);
        string filePathPrefix = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\";
        testhighTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA.csv");
        testlowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA.csv");
        testtempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA.csv");
        testvariance.readCSVFile(filePathPrefix + "VarianceNiceMapA.csv");
        testEquation.createEquations(testhighTemp, testlowTemp, testtempMidpt, testvariance);

        // Test conditions
        // Basic correct info
        Assert.AreEqual(50 * 50, testEquation.worldArray.Length);
        Assert.AreEqual(100, testhighTemp.worldArray[0, 0]);
        Assert.AreEqual(1, testlowTemp.worldArray[0, 0]);
        Assert.AreEqual((float)20.4, testtempMidpt.worldArray[0, 0]);
        Assert.AreEqual((float)8.0, testvariance.worldArray[0, 0]);
        // Check for correct X-Z axes
        Assert.AreEqual(98, testhighTemp.worldArray[2, 1]);
        Assert.AreEqual(-5, testlowTemp.worldArray[2, 1]);
        Assert.AreEqual((float)26.6, testtempMidpt.worldArray[2, 1]);
        Assert.AreEqual((float)9.8, testvariance.worldArray[2, 1]);

    }

}
