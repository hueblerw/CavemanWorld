using UnityEngine;
using System.Collections;

public class HabitatLayer {

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX;
    public static int WORLDZ;
    public float[,] worldArray;

    // Constructor
    public HabitatLayer()
    {
        layerName = "Habitat Layer";
        layerType = "Yearly";
    }

    // Getter methods
    public string getType()
    {
        return layerType;
    }

}
