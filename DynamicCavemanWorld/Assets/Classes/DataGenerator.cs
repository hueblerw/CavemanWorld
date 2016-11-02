using UnityEngine;
using System.Collections;

public class DataGenerator {

    // constructor
    public DataGenerator()
    {
    }

    // Static methods for creating layers based on inputs:
    // ***************************************************
    // Standard Layer - float [,] - note this works for TempMidpt, Variance, and AvgHumidity layer generation
    public static float[,] CreateStandardFloatLayer(int WORLDX, int WORLDZ, double minValue, double maxValue, double distMult)
    {
        float[,] layer = new float[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        int sign = 1;
        double average;

        // First do the upper left square
        layer[0, 0] = (float) (randy.NextDouble() * (maxValue - minValue) + minValue);
        // Then do the top row based on the square to its left
        for (int x = 1; x < WORLDX; x++)
        {
            sign = GenerateRandomSign(randy);
            layer[x, 0] = (float) (layer[x - 1, 0] + sign * randy.NextDouble() * distMult);
        }
        // Then do the left column based on the square above it
        for (int z = 1; z < WORLDZ; z++)
        {
            sign = GenerateRandomSign(randy);
            layer[0, z] = (float)(layer[z - 1, 0] + sign * randy.NextDouble() * distMult);
        }
        // Then do every other cell in order
        for (int z = 1; z < WORLDX; z++)
        {
            for (int x = 1; x < WORLDX; x++)
            {
                sign = GenerateRandomSign(randy);
                average = (layer[z - 1, x] + layer[z - 1, x - 1] + layer[z, x - 1]) / 3.0;
                layer[0, z] = (float)(average + sign * randy.NextDouble() * distMult);
            }
        }

        return layer;
    }


    // Private supporting methods
    private static int GenerateRandomSign(System.Random randy)
    {
        int num = randy.Next(0, 2);
        if (num == 0)
        {
            num = -1;
        }
        return num;
    }

}
