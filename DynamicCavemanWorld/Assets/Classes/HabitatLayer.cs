﻿using UnityEngine;
using System.Collections;

public class HabitatLayer {

    // Variables
    public string layerName;
    private string layerType;
    public static int WORLDX;
    public static int WORLDZ;
    public Habitat[,] worldArray;

    // Constructor
    public HabitatLayer(int[,][] habitatCounters)
    {
        layerName = "Habitat";
        layerType = "Yearly";
        WORLDX = SingleValueLayer.WORLDX;
        WORLDZ = SingleValueLayer.WORLDZ;
        for (int x = 0; x < WORLDX; x++)
        {
            for (int z = 0; z < WORLDZ; z++)
            {
                worldArray[x, z] = new Habitat(habitatCounters[x, z]);
            }
        }
    }

    // Getter methods
    public string getType()
    {
        return layerType;
    }

}
