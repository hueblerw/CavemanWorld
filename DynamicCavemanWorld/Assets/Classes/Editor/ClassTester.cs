using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class ClassTester
{

    [Test]
    public void SVLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        SingleValueLayer.WORLDX = 50;
        SingleValueLayer.WORLDZ = 50;
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
        Assert.AreEqual(120, rainfall.worldArray.Length);
        Assert.AreEqual(50, DailyLayer.WORLDX);
        Assert.AreEqual(50, DailyLayer.WORLDZ);
    }

    [Test]
    public void ObjectLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        ObjectLayer downstream = new ObjectLayer("Downstream", "Semi-static");

        Assert.AreEqual("Downstream", downstream.layerName);
        Assert.AreEqual("Semi-static", downstream.getType());
        // Assert.AreEqual(50 * 50, downstream.worldArray.Length);
        Assert.AreEqual(50, ObjectLayer.WORLDX);
        Assert.AreEqual(50, ObjectLayer.WORLDZ);
    }

    [Test]
    public void EquationLayerTest()
    {
        // Test the TempEquation Constructor
        EquationLayer testEquation = new EquationLayer("TemperatureEquations", "Semi-static");

        Assert.AreEqual("TemperatureEquations", testEquation.layerName);
        Assert.AreEqual("Semi-static", testEquation.getType());
        Assert.AreEqual(50, EquationLayer.WORLDX);
        Assert.AreEqual(50, EquationLayer.WORLDZ);
    }

    [Test]
    public void TempEquationTest()
    {
        // Test the TempEquation
        TempEquation testEquation = new TempEquation(80, 20, (float)25.0, (float)6.0);

        // Test the TempEquation Constructor
        Assert.AreEqual(0.0, testEquation.returnConstants()[0], 0.01);
        Assert.AreEqual(-26.25, testEquation.returnConstants()[1], 0.1);
        Assert.AreEqual(52.51, testEquation.returnConstants()[2], 0.1);
        Assert.AreEqual(-31.51, testEquation.returnConstants()[3], 0.1);
        Assert.AreEqual(5.25, testEquation.returnConstants()[4], 0.1);
        Assert.AreEqual(1.0, testEquation.returnConstants()[5], 0.1);

        // Test the yearly temperature generation
        Assert.AreEqual(20.0, testEquation.generateYearsTemps().days[0], 6.0);
        Assert.AreEqual(58.6, testEquation.generateYearsTemps().days[29], 6.0);
        Assert.AreEqual(80.0, testEquation.generateYearsTemps().days[59], 6.0);
        Assert.AreEqual(58.6, testEquation.generateYearsTemps().days[89], 6.0);
    }

    [Test]
    public void EquationCreationTest()
    {
        SingleValueLayer.WORLDX = 50;
        SingleValueLayer.WORLDZ = 50;
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

    [Test]
    public void HillAndOceanPerTest()
    {
        // Initialize a world
        World testWorld = new World(50, 50);
        System.Random randy = new System.Random();
        int x = randy.Next(0, 50);
        int z = randy.Next(0, 50);

        Debug.Log("MAXELE - " + testWorld.maxElevationDifference);
        Assert.Greater(testWorld.maxElevationDifference, 0f);
        Assert.GreaterOrEqual(testWorld.hillPer.worldArray[x, z], 0f);
        Assert.LessOrEqual(testWorld.hillPer.worldArray[x, z], 1f);
        Assert.GreaterOrEqual(testWorld.oceanPer.worldArray[x, z], 0f);
        Assert.LessOrEqual(testWorld.oceanPer.worldArray[x, z], 1f);
    }

    [Test]
    public void RiverTest()
    {
        // Initialize a river
        River river = new River(4, 7, .25f, 0f);
        River otherRiver = new River(12, 4, .25f, 1f);

        Assert.AreEqual(river.x, 4);
        Assert.AreEqual(river.z, 7);
        Assert.AreEqual(river.type, null);
        Assert.AreEqual(otherRiver.x, 12);
        Assert.AreEqual(otherRiver.z, 4);
        Assert.AreEqual(otherRiver.type, "ocean");
    }

    [Test]
    public void DirectionTest()
    {
        // Check the Direction methods work correctly
        Direction upwards = new Direction("up");
        Direction downwards = new Direction("down");
        Direction leftwards = new Direction("left");
        Direction rightwards = new Direction("right");
        // Direction none = null;
        int x = 1;
        int z = 2;
        int[] coor = new int[2];
        coor[0] = 2;
        coor[1] = 2;
        SingleValueLayer.WORLDX = 50;
        SingleValueLayer.WORLDZ = 50;
        SingleValueLayer ele = new SingleValueLayer("ele", "Testing", 1);
        string filePath = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\ElevationNiceMapA.csv";
        ele.readCSVFile(filePath);

        // Check it assigns properly
        Assert.AreEqual(upwards.direction, "up");
        Assert.AreEqual(downwards.direction, "down");
        Assert.AreEqual(leftwards.direction, "left");
        Assert.AreEqual(rightwards.direction, "right");
        // Check the get coordinate as array thingy
        Assert.AreEqual(rightwards.getCoordinateArray(x, z), coor);
        Assert.AreEqual(upwards.getCoordinateArray(x, z)[1], 1);
        Assert.AreEqual(leftwards.getCoordinateArray(x, z)[0], 0);
        Assert.AreEqual(downwards.getCoordinateArray(x, z)[1], 3);
        // Check the to string method works properly
        Assert.AreEqual(upwards.ToString(), "up");
        Assert.AreEqual(downwards.ToString(), "down");
        Assert.AreEqual(leftwards.ToString(), "left");
        Assert.AreEqual(rightwards.ToString(), "right");
        // Assert.AreEqual(none.ToString(), "none");
        // Check the get float from direction feature
        Assert.AreEqual(-3.5f, (float) Math.Round(upwards.getFloatAtCoordinates(x, z, ele.worldArray), 1));
        Assert.AreEqual(-0.5f, (float) Math.Round(downwards.getFloatAtCoordinates(x, z, ele.worldArray), 1));
        Assert.AreEqual(-0.6f, (float) Math.Round(leftwards.getFloatAtCoordinates(x, z, ele.worldArray), 1));
        Assert.AreEqual(-1.6f, (float) Math.Round(rightwards.getFloatAtCoordinates(x, z, ele.worldArray), 1));
    }

    [Test]
    public void HabitatTest()
    {
        // Initialize a habitat
        int[] habitatTypeCounters = new int[13];
        habitatTypeCounters[3] = 11;
        habitatTypeCounters[8] = 5;
        habitatTypeCounters[4] = 3;
        habitatTypeCounters[2] = 1;
        Habitat testHabitat = new Habitat(habitatTypeCounters);

        // Test the public variables and the initializer methods indirectly
        Assert.AreEqual(testHabitat.dominantType, "boreal");
        Assert.AreEqual(testHabitat.typePercents[2], .05);
        Assert.AreEqual(testHabitat.typePercents[3], .55);
        Assert.AreEqual(testHabitat.typePercents[4], .15);
        Assert.AreEqual(testHabitat.typePercents[8], .25);

        // Test the public static methods
        Assert.AreEqual(Habitat.DetermineTemp(5, 60), "artic");
        Assert.AreEqual(Habitat.DetermineTemp(25, 60), "temperate");
        Assert.AreEqual(Habitat.DetermineTemp(5, 33), "temperate");
        Assert.AreEqual(Habitat.DetermineTemp(50, 60), "temperate");
        Assert.AreEqual(Habitat.DetermineTemp(50, 8), "tropical");
        Assert.AreEqual(Habitat.DetermineWetness(15), "dry");
        Assert.AreEqual(Habitat.DetermineWetness(28), "moderate");
        Assert.AreEqual(Habitat.DetermineWetness(46), "wet");
        Assert.AreEqual(Habitat.DetermineWetness(70), "very wet");
        Assert.AreEqual(Habitat.DetermineHabitatFavored("moderate", "artic"), 2);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("moderate", "tropical"), 10);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("dry", "artic"), 1);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("wet", "artic"), 3);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("wet", "temperate"), 7);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("very wet", "temperate"), 8);
        Assert.AreEqual(Habitat.DetermineHabitatFavored("very wet", "tropical"), 12);

        // Test the toString method
        string expected = "Habitats: " + "\n" + "5% tundra" + "\n" + "55% boreal" + "\n" + "15% artic marsh" + "\n" + "25% swamp";
        Assert.AreEqual(expected, testHabitat.ToString());

        // Test the Update Habitat Method
        System.Random randy = new System.Random();
        testHabitat.UpdateHabitatYear(50, 9, 32.4f, 2.4f, 1, randy);
        Debug.Log(testHabitat);
        Assert.AreEqual(.01, testHabitat.typePercents[10]);
    }

    [Test]
    public void HabitatLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        World testWorld = new World(50, 50);
        int[,][] habitatTypeCounters = new int[50, 50][];
        int index;
        string wetness;
        string temperateness;

        // Do this for twenty years
        for (int year = 0; year < 20; year++)
        {
            // Generate a new year
            testWorld.NewYear();
            // Create arrays with the relevant data for calcluating the habitats
            for (int x = 0; x < 50; x++)
            {
                for (int z = 0; z < 50; z++)
                {
                    // Get the index of the expected habitat for each tile that year
                    wetness = Habitat.DetermineWetness(testWorld.rainfallTotal.worldArray[x, z] + River.AverageRiverLevel(x, z));
                    temperateness = Habitat.DetermineTemp(testWorld.temps[x, z].Count70DegreeDays(), testWorld.temps[x, z].Count32DegreeDays());
                    index = Habitat.DetermineHabitatFavored(wetness, temperateness);
                    // Add a counter for that habitat to that tiles counter array
                    habitatTypeCounters[x, z][index] += 1;
                }
            }
        }

        HabitatLayer tester = new HabitatLayer(habitatTypeCounters);

        Assert.AreEqual("Habitat", tester.layerName);
        Assert.AreEqual("Yearly", tester.getType());
        Assert.AreEqual(50 * 50, tester.worldArray.Length);
        Assert.AreEqual(50, HabitatLayer.WORLDX);
        Assert.AreEqual(50, HabitatLayer.WORLDZ);
    }

}