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
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public decimal[,] worldArray = new decimal[WORLDX, WORLDZ];

    // Constructor
    public SingleValueLayer(string name, string type, int roundTo)
    {
        this.layerName = name;
        this.layerType = type;
        this.rounded = roundTo;
    }

    // Layer initialization Method
    public void readCSVFile(string filePath)
    {

        StreamReader sr = new StreamReader(filePath);
        decimal[,] data = new decimal[WORLDX, WORLDZ];
        int Row = 0;
        while (!sr.EndOfStream)
        {
            string[] Line = sr.ReadLine().Split(',');
            for(int i = 0; i < Line.Length; i++)
            {
                data[i, Row] = Math.Round(Convert.ToDecimal(Line[i]), this.rounded);
            }
            Row++;
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

}
