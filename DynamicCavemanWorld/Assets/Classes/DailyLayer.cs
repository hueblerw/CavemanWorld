using UnityEngine;
using System.Collections;
using System;

public class DailyLayer {

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX;
    public static int WORLDZ;
    public float[][,] worldArray = new float[120][,];

    // Constructor
    public DailyLayer(string name, int roundTo)
    {
        this.layerName = name;
        this.layerType = "Daily";
        this.rounded = roundTo;
        WORLDX = SingleValueLayer.WORLDX;
        WORLDZ = SingleValueLayer.WORLDZ;
        for (int i = 0; i < 120; i++)
        {
            worldArray[i] = new float[WORLDX, WORLDZ];
        }
    }

    // World Array Initializer
    public void addWorldDay(int day, float[,] rainfallArray)
    {
        worldArray[day] = rainfallArray;
    }

    // Sums up all the values in all given squares of the daily layer for all days of the year
    public float[,] findYearTotalArray()
    {
        float[,] totals = new float[WORLDX, WORLDZ];

        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                totals[x, z] = findYearTotal(x, z);
            }
        }

        return totals;
    }

    // Finds the sum of all values of an individual square over the course of a year
    public float findYearTotal(int x, int z)
    {
        float sum = 0f;
        for (int day = 0; day < 120; day++)
        {
            sum += worldArray[day][x, z];
        }
        return (float) Math.Round(sum, 1);
    }

    // Getters
    public string getType()
    {
        return layerType;
    }

    public int getRounding()
    {
        return rounded;
    }

}
