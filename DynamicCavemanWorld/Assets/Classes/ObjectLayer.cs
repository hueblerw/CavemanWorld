using UnityEngine;
using System.Collections;

public class ObjectLayer
{

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX;
    public static int WORLDZ;
    public River[,] worldArray;

    // Constructor
    public ObjectLayer(string name, string type)
    {
        this.layerName = name;
        this.layerType = type;
        WORLDX = SingleValueLayer.WORLDX;
        WORLDZ = SingleValueLayer.WORLDZ;
    }

    // World Array Initializer

    // Getters
    public string getType()
    {
        return layerType;
    }

}
