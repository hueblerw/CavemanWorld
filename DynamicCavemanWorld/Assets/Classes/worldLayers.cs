using UnityEngine;
using System.Collections;

public class worldLayers {

    public string layerName;
    private string layerType;
    public const int WORLDX = 50;
    public const int WORLDZ = 50;

    // Constructor
    public worldLayers(string name, string type)
    {
        this.layerName = name;
        this.layerType = type;
    }

    public string getType()
    {
        return layerType;
    }

}
