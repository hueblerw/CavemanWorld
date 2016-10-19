using UnityEngine;
using System.Collections;

public class ObjectLayer
{

    // Variables
    public string layerName;
    private string layerType;
    private int rounded;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public object[,] worldArray;

    // Constructor
    public ObjectLayer(string name, string type, int roundTo)
    {
        this.layerName = name;
        this.layerType = type;
        this.rounded = roundTo;
        this.worldArray = new object[WORLDX, WORLDZ];
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
