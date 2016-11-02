using UnityEngine;
using System.Collections;

public class DataGenerator {

    // constructor
    public DataGenerator()
    {
    }

    // Static methods for creating layers based on inputs:
    // ***************************************************
    // Standard Layer - float [,]
    public static float[,] CreateStandardFloatLayer(int WORLDX, int WORLDZ, double minValue, double maxValue, double distMult)
    {
        float[,] layer = new float[WORLDX, WORLDZ];
        Random randy = new Random();

        // First do the upper left square
        layer[0, 0] = randy.NextDouble(minValue, maxValue);
    }

}
