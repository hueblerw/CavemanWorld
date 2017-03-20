using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SingleValueLayer
{

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX;
    public static int WORLDZ;
    public float[,] worldArray = new float[WORLDX, WORLDZ];

    // Constructor
    public SingleValueLayer(string name, string type, int roundTo)
    {
        if (name == "ElevationVertices")
        {
            this.worldArray = new float[WORLDX + 1, WORLDZ + 1];
        }
        this.layerName = name;
        this.layerType = type;
        this.rounded = roundTo;
    }

    // Layer initialization Method
    public void readCSVFile(string filePath)
    {
        TextAsset CSVFile = Resources.Load(filePath) as TextAsset;
        string[] rows = CSVFile.text.Split('\n');
        // Debug.Log("*****************************************");
        float[,] data = new float[WORLDX, WORLDZ];
        for (int Row = 0; Row < rows.Length; Row++)
        {
            string[] Line = rows[Row].Split(',');
            for(int i = 0; i < Line.Length; i++)
            {
                data[i, Row] = (float)Convert.ToDouble(Line[i]);
            }
        }
        this.worldArray = data;

    }
    
    // Getter methods
    public string getType()
    {
        return layerType;
    }

    public int getRounding()
    {
        return rounded;
    }


    // Gets the lowest and highest values in the array.
    public float[] getMinMaxValues()
    {
        float[] minMax = new float[2];
        minMax[0] = getMinValue();
        minMax[1] = getMaxValue();
        return minMax;
    }

    private float getMinValue()
    {
        float extrema = 0f;
        for (int i = 0; i < WORLDX; i++)
        {
            for (int j = 0; j < WORLDZ; j++)
            {
                if (worldArray[i, j] < extrema)
                {
                    extrema = worldArray[i, j];
                }
            }
        }
        return extrema;
    }

    private float getMaxValue()
    {
        float extrema = 0f;
        for (int i = 0; i < WORLDX; i++)
        {
            for (int j = 0; j < WORLDZ; j++)
            {
                if (worldArray[i, j] > extrema)
                {
                    extrema = worldArray[i, j];
                }
            }
        }
        return extrema;
    }
}
