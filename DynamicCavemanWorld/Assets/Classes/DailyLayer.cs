using UnityEngine;
using System.Collections;

public class DailyLayer {

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public decimal[,,] worldArray = new decimal[120, WORLDX, WORLDZ];

    // Constructor
    public DailyLayer(string name, int roundTo)
    {
        this.layerName = name;
        this.layerType = "Daily";
        this.rounded = roundTo;
    }

    // World Array Initializer

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
