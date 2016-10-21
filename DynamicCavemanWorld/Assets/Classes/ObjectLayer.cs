using UnityEngine;
using System.Collections;

public class ObjectLayer
{

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public River[,] worldArray;

    // Constructor
    public ObjectLayer(string name, string type)
    {
        this.layerName = name;
        this.layerType = type;
    }

    // World Array Initializer

    // Getters
    public string getType()
    {
        return layerType;
    }

}
