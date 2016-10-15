using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Support {

    public static float[] CellsAroundVertex(int x, int z, int WorldX, int WorldZ, float[,] array)
    {
        float[] cells;
        List<float> cellList = new List<float>();
        // Add the four possible values if legal
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
        // Convert to an array and return
        cells = cellList.ToArray();
        return cells;
    }

}
