using UnityEngine;
using System.Collections;
using System;

public class DataGenerator {

    public static int WORLDX;
    public static int WORLDZ;
    // constructor
    public DataGenerator()
    {
    }

    // Static methods for creating layers based on inputs:
    // ***************************************************
    // Standard Layer - float [,] - note this works for TempMidpt, Variance, and AvgHumidity layer generation
    public static float[,] CreateStandardFloatLayer(double minValue, double maxValue, double distMult)
    {
        float[,] layer = new float[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        int sign;
        double average;

        // First do the upper left square
        layer[0, 0] = (float) Math.Round((randy.NextDouble() * (maxValue - minValue) + minValue), 1);
        // Then do the top row based on the square to its left
        for (int x = 1; x < WORLDX; x++)
        {
            sign = GenerateRandomSign(randy);
            layer[x, 0] = (float) Math.Round(Math.Max(Math.Min((layer[x - 1, 0] + sign * randy.NextDouble() * distMult), maxValue), minValue), 1);
        }
        // Then do the left column based on the square above it
        for (int z = 1; z < WORLDZ; z++)
        {
            sign = GenerateRandomSign(randy);
            layer[0, z] = (float) Math.Round(Math.Max(Math.Min((layer[z - 1, 0] + sign * randy.NextDouble() * distMult), maxValue), minValue), 1);
        }
        // Then do every other cell in order
        for (int z = 1; z < WORLDX; z++)
        {
            for (int x = 1; x < WORLDX; x++)
            {
                sign = GenerateRandomSign(randy);
                average = (layer[z - 1, x] + layer[z - 1, x - 1] + layer[z, x - 1]) / 3.0;
                layer[0, z] = (float) Math.Round(Math.Max(Math.Min((average + sign * randy.NextDouble() * distMult), maxValue), minValue), 1);
            }
        }

        return layer;
    }

    // Elevation Layer
    public static float[,] CreateElevationLayer()
    {
        float[,] layer = new float[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        int sign;
        int index;
        double randomRoot;
        int[] randomNeighbor;

        // First do the upper left square
        layer[0, 0] = (float)(randy.NextDouble() * 6f - 3f);
        // Then do the top row based on the square to its left
        for (int x = 1; x < WORLDX; x++)
        {
            sign = GenerateRandomSign(randy);
            randomRoot = RandomRoot(randy);
            layer[x, 0] = (float) Math.Round((layer[x - 1, 0] + sign * randomRoot), 1);
        }
        // Then do the left column based on the square above it
        for (int z = 1; z < WORLDZ; z++)
        {
            sign = GenerateRandomSign(randy);
            randomRoot = RandomRoot(randy);
            layer[0, z] = (float) Math.Round((layer[0, z - 1] + sign * randomRoot), 1);
        }
        // Then do every other cell in order
        for (int z = 1; z < WORLDX; z++)
        {
            for (int x = 1; x < WORLDX; x++)
            {
                index = randy.Next(0, 4);
                sign = GenerateRandomSign(randy);
                randomRoot = RandomRoot(randy);
                randomNeighbor = RandomNeighborCoor(x, z, randy);
                layer[x, z] = (float) Math.Round((layer[randomNeighbor[0], randomNeighbor[1]] + sign * randomRoot), 1);
            }
        }

        return layer;
    }

    // Temperature Layers - linked int [,] - note this works for AvgHigh and LowTemp
    public static int[][,] CreateTemperatureLayers(int distMult)
    {
        int[][,] layers = new int[2][,];
        layers[0] = new int[WORLDX, WORLDZ];
        layers[1] = new int[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        int sign;
        int average;
        int coldmaxValue;
        int coldminValue;

        // Create the hightemperature layer
        layers[0] = CreateStandardIntLayer(30, 110, 4);

        // Then create the linked LowTemperature layer
        coldminValue = -20;
        // First do the upper left square
        layers[1][0, 0] = randy.Next(coldminValue, layers[0][0, 0] - 15);
        // Then do the top row based on the square to its left
        for (int x = 1; x < WORLDX; x++)
        {
            sign = GenerateRandomSign(randy);
            coldmaxValue = layers[0][x, 0] - 15;
            layers[1][x, 0] = Math.Max(Math.Min((layers[1][x - 1, 0] + sign * randy.Next(0, distMult + 1)), coldmaxValue), coldminValue);
        }
        // Then do the left column based on the square above it
        for (int z = 1; z < WORLDZ; z++)
        {
            sign = GenerateRandomSign(randy);
            coldmaxValue = layers[0][0, z] - 15;
            layers[1][0, z] = Math.Max(Math.Min((layers[1][z - 1, 0] + sign * randy.Next(0, distMult + 1)), coldmaxValue), coldminValue);
        }
        // Then do every other cell in order
        for (int z = 1; z < WORLDX; z++)
        {
            for (int x = 1; x < WORLDX; x++)
            {
                sign = GenerateRandomSign(randy);
                coldmaxValue = layers[0][x, z] - 15;
                average = (int)Math.Round((layers[1][z - 1, x] + layers[1][z - 1, x - 1] + layers[1][z, x - 1]) / 3.0, 0);
                layers[1][0, z] = Math.Max(Math.Min((average + sign * randy.Next(0, distMult + 1)), coldmaxValue), coldminValue);
            }
        }

        return layers;
    }


    // Private supporting methods
    private static int[,] CreateStandardIntLayer(int minValue, int maxValue, int distMult)
    {
        int[,] layer = new int[WORLDX, WORLDZ];
        System.Random randy = new System.Random();
        int sign;
        int average;

        // First do the upper left square
        layer[0, 0] = randy.Next(minValue, maxValue);
        // Then do the top row based on the square to its left
        for (int x = 1; x < WORLDX; x++)
        {
            sign = GenerateRandomSign(randy);
            layer[x, 0] = Math.Max(Math.Min((layer[x - 1, 0] + sign * randy.Next(0, distMult + 1)), maxValue), minValue);
        }
        // Then do the left column based on the square above it
        for (int z = 1; z < WORLDZ; z++)
        {
            sign = GenerateRandomSign(randy);
            layer[0, z] = Math.Max(Math.Min((layer[z - 1, 0] + sign * randy.Next(0, distMult + 1)), maxValue), minValue);
        }
        // Then do every other cell in order
        for (int z = 1; z < WORLDX; z++)
        {
            for (int x = 1; x < WORLDX; x++)
            {
                sign = GenerateRandomSign(randy);
                average = (int) Math.Round((layer[z - 1, x] + layer[z - 1, x - 1] + layer[z, x - 1]) / 3.0, 0);
                layer[0, z] = Math.Max(Math.Min((average + sign * randy.Next(0, distMult + 1)), maxValue), minValue);
            }
        }

        return layer;
    }
    
    // Generate a random negative sign
    private static int GenerateRandomSign(System.Random randy)
    {
        int num = randy.Next(0, 2);
        if (num == 0)
        {
            num = -1;
        }
        return num;
    }

    // Generate a number from 0-2 with a square root distribution
    private static double RandomRoot(System.Random randy)
    {
        return Math.Pow((Math.Sqrt(2) * (randy.NextDouble())), 2);
    }

    // return the coordinates of a random neighbor
    private static int[] RandomNeighborCoor(int x, int z, System.Random randy)
    {
        int[] coor = new int[2];
        int indexMax = 4;
        int index;

        if(x == WORLDX)
        {
            indexMax--;
        }
        index = randy.Next(0, indexMax);

        switch (index)
        {
            case 0:
                coor[0] = x - 1;
                coor[1] = z;
                break;
            case 1:
                coor[0] = x - 1;
                coor[1] = z - 1;
                break;
            case 2:
                coor[0] = x;
                coor[1] = z - 1;
                break;
            case 3:
                coor[0] = x + 1;
                coor[1] = z - 1;
                break;
        }

        return coor;
    }

}
