using UnityEngine;
using System.Collections;
using System.IO;

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
    public void readCSVFile()
    {

        string filePath = @"C:\Users\William\Documents\World Generator Maps\CavemanWorld\DynamicCavemanWorld\Assets\Resources\CSV\ElevationNiceMapA.csv";
        StreamReader sr = new StreamReader(filePath);
        // decimal[][] data;
        // int Row = 0;
        while (!sr.EndOfStream)
        {
            string[] Line = sr.ReadLine().Split(',');
            Debug.Log(Line[0]);
        }

        // Debug.Log(data);

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
