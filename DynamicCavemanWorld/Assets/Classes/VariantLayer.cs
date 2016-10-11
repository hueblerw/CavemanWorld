using UnityEngine;
using System.Collections;

public class VariantLayer
{

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public decimal[,,] worldArray;

    // Constructor
    public VariantLayer(string name, string type, int c, int roundTo)
    {
        this.layerName = name;
        this.layerType = type;
        this.rounded = roundTo;
        this.worldArray = new decimal[c, WORLDX, WORLDZ];
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
