using UnityEngine;
using System.Collections;

public class EquationLayer {

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX;
    public static int WORLDZ;
    public TempEquation[,] worldArray;

    // Constructor
    public EquationLayer(string name, string type)
    {  
        this.layerName = name;
        this.layerType = type;
        WORLDX = SingleValueLayer.WORLDX;
        WORLDZ = SingleValueLayer.WORLDZ;
    }

    // World Array Initializer

    public void createEquations(SingleValueLayer highTemp, SingleValueLayer lowTemp, SingleValueLayer tempMidpt, SingleValueLayer variance)
    {

        // Basically, loop through the creation of each individual equation
        TempEquation temporary;
        TempEquation[,] tempArray = new TempEquation[EquationLayer.WORLDX, EquationLayer.WORLDZ];
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                temporary = new TempEquation((int)highTemp.worldArray[x, z], (int)lowTemp.worldArray[x, z], tempMidpt.worldArray[x, z], variance.worldArray[x, z]);
                tempArray[x, z] = temporary;
            }
        }
        this.worldArray = tempArray;

    }

    // Getters
    public string getType()
    {
        return layerType;
    }

}
