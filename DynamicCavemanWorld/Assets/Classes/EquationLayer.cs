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

    public void createEquations(SingleValueLayer highTemp, SingleValueLayer lowTemp, SingleValueLayer tempMidpt)
    {
        // Basically, loop through the creation of each individual equation
        TempEquation temporary;
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                temporary = new TempEquation();
                this.worldArray[x, z] = temporary;
            }
        }
    }

    // Getters
    public string getType()
    {
        return layerType;
    }

}
