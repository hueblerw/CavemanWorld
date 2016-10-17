﻿using UnityEngine;
using System.Collections;

public class DailyLayer {

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public float[][,] worldArray = new float[120][,];

    // Constructor
    public DailyLayer(string name, int roundTo)
    {
        this.layerName = name;
        this.layerType = "Daily";
        this.rounded = roundTo;
        for (int i = 0; i < 120; i++)
        {
            worldArray[i] = new float[WORLDX, WORLDZ];
        }
    }

    // World Array Initializer
    public void addWorldDay(int day, float[,] rainfallArray)
    {
        // Yay, fun loops or reorganized logic???
        worldArray[day] = rainfallArray;
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
