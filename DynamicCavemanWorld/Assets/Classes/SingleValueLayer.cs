using UnityEngine;
using System.Collections;

public class SingleValueLayer
{

    public string layerName;
    private string layerType;
    public static int WORLDX = 50;
    public static int WORLDZ = 50;
    public float[][] worldArray;


    public SingleValueLayer(string name, string type)
    {
        this.layerName = name;
        this.layerType = type;
    }


    public string getType()
    {
        return layerType;
    }

}
