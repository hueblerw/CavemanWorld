using UnityEngine;
using System.Collections;

public class ObjectLayer
{

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public object[,] worldArray;

    // Constructor
    public ObjectLayer(string name, string type)
    {
        this.layerName = name;
        this.layerType = type;
        this.worldArray = new object[WORLDX, WORLDZ];
}

    // World Array Initializer

    // Getters
    public string getType()
    {
        return layerType;
    }

}
