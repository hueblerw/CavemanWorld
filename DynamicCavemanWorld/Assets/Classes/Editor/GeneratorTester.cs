using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class GeneratorTester {

	[Test]
	public void ElevationGeneratorTester()
	{
        // Set the world size
        DataGenerator.WORLDX = 80;
        DataGenerator.WORLDZ = 60;
        // Generate the tested world
        float[,] array = DataGenerator.CreateElevationLayer();
        // Generate random coordinates
        System.Random randy = new System.Random();
        int i = randy.Next(0, DataGenerator.WORLDX);
        int j = randy.Next(0, DataGenerator.WORLDZ);
        string map ="";

        Assert.GreaterOrEqual(array[0, 0], -3f);
        Assert.LessOrEqual(array[0, 0], 3f);
        Assert.AreNotEqual(0f, array[i, j]);

        for(int x = 0; x < DataGenerator.WORLDX; x++)
        {
            for (int z = 0; z < DataGenerator.WORLDX; z++)
            {
                map += array[x, z] + ", ";
            }
            map += "\n";
        }

        Debug.Log(map);
    }

    [Test]
    public void TempGeneratorTester()
    {
        // Set the world size
        DataGenerator.WORLDX = 40;
        DataGenerator.WORLDZ = 60;
        // Generate the tested world
        int[][,] array = DataGenerator.CreateTemperatureLayers(4);
        // Generate random coordinates
        System.Random randy = new System.Random();
        int i = randy.Next(0, DataGenerator.WORLDX);
        int j = randy.Next(0, DataGenerator.WORLDZ);
        string mapA = "";
        string mapB = "";

        Assert.GreaterOrEqual(array[0][0, 0], 30);
        Assert.LessOrEqual(array[0][0, 0], 110);
        Assert.GreaterOrEqual(array[0][i, j], 30);
        Assert.LessOrEqual(array[0][i, j], 110);

        Assert.GreaterOrEqual(array[1][0, 0], -20);
        Assert.LessOrEqual(array[1][0, 0], array[0][0, 0] - 15);
        Assert.GreaterOrEqual(array[1][i, j], -20);
        Assert.LessOrEqual(array[1][i, j], array[0][i, j] - 15);

        for (int x = 0; x < DataGenerator.WORLDX; x++)
        {
            for (int z = 0; z < DataGenerator.WORLDX; z++)
            {
                mapA += array[0][x, z] + ", ";
                mapB += array[1][x, z] + ", ";
            }
            mapA += "\n";
            mapB += "\n";
        }

        Debug.Log(mapA);
        Debug.Log(mapB);
    }

    [Test]
    public void StandardLayerTest()
    {
        // Set the world size
        DataGenerator.WORLDX = 40;
        DataGenerator.WORLDZ = 50;
        // Generate the tested world
        float[,] array = DataGenerator.CreateStandardFloatLayer(0.0, 10.0, 1.0);
        // Generate random coordinates
        System.Random randy = new System.Random();
        int i = randy.Next(0, DataGenerator.WORLDX);
        int j = randy.Next(0, DataGenerator.WORLDZ);
        string map = "";

        Assert.GreaterOrEqual(array[0, 0], 0f);
        Assert.LessOrEqual(array[0, 0], 10f);
        Assert.GreaterOrEqual(array[i, j], 0f);
        Assert.LessOrEqual(array[i, j], 10f);

        for (int x = 0; x < DataGenerator.WORLDX; x++)
        {
            for (int z = 0; z < DataGenerator.WORLDX; z++)
            {
                map += array[0][x, z] + ", ";
            }
            map += "\n";
        }

        Debug.Log(map);
    }
}
