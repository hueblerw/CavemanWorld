using UnityEngine;
using System.Collections;

public class EquationLayer {

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public TempEquation[,] worldArray;

    // Constructor
    public EquationLayer(string name, string type)
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
