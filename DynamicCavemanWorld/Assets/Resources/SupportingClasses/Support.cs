using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Support {

    public static float[] CellsAroundVertex(int x, int z, int WorldX, int WorldZ, float[,] array)
    {
        float[] cells;
        List<float> cellList = Support.getAround(x, z, WorldX, WorldZ, array);
        cells = cellList.ToArray();
        return cells;
    }

    public static float[] CellsAllAroundC(int x, int z, int WorldX, int WorldZ, float[,] array)
    {
        float[] cells;
        List<float> cellList = Support.getAroundCenter(true, x, z, WorldX, WorldZ, array); ;
        cells = cellList.ToArray();
        return cells;
    }

    // Private methods
    private static List<float> getAround(int x, int z, int WorldX, int WorldZ, float[,] array)
    {
        List<float> cellList = new List<float>();
        // Add the four caridnal values if legal
        if (x < WorldX && z < WorldZ)
        {
            cellList.Add(array[x, z]);
        }
        if (x > 0 && z > 0)
        {
            cellList.Add(array[x - 1, z - 1]);
        }
        if (x > 0 && z < WorldZ)
        {
            cellList.Add(array[x - 1, z]);
        }
        if (x < WorldX && z > 0)
        {
            cellList.Add(array[x, z - 1]);
        }

        return cellList;
    }

    private static List<float> getAroundCenter(bool diagonals, int x, int z, int WorldX, int WorldZ, float[,] array)
    {
        List<float> cellList = new List<float>();
        // Add the four caridnal values if legal
        if (x > 0)
        {
            cellList.Add(array[x - 1, z]);
        }
        if (x < WorldX)
        {
            cellList.Add(array[x + 1, z]);
        }
        if (z > 0)
        {
            cellList.Add(array[x, z - 1]);
        }
        if (z < WorldZ)
        {
            cellList.Add(array[x, z + 1]);
        }
        // Add the diagonals if required and legal
        if (diagonals)
        {
            if (x < WorldX && z < WorldZ)
            {
                cellList.Add(array[x + 1, z + 1]);
            }
            if (x > 0 && z > 0)
            {
                cellList.Add(array[x - 1, z - 1]);
            }
            if (x > 0 && z < WorldZ)
            {
                cellList.Add(array[x - 1, z + 1]);
            }
            if (x < WorldX && z > 0)
            {
                cellList.Add(array[x + 1, z - 1]);
            }
        }

        return cellList;
    }
}
