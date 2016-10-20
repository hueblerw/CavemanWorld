using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class River {

    // Static Variables
    public static DailyLayer upstreamToday;
    public static DailyLayer surfacewater;

    // Variables
    public int x;
    public int z;
    public string type;
    public Direction downstream;
    public List<Direction> upstream;
    private float flowrate;
    private float soilAbsorption;

    // Constructor 
    public River(int x, int z, float hillPer, float oceanPer, SingleValueLayer elevation)
    {
        this.x = x;
        this.z = z;
        // Generate the Directions
        System.Random randy = new System.Random();
        ChooseDownstream(elevation, randy);
            // Find Downstream location - (Upstream locations must be generated later)
        // Then Determine the type
        setRates(hillPer, oceanPer);
    }

    // Private methods
    //  Use hillPer to determine the flow and soilAbsorption rates
    private void setRates(float hillPer, float oceanPer)
    {
        System.Random randy = new System.Random();
        flowrate = (float) Math.Round((1f - hillPer) + (float) randy.NextDouble() + .1f, 2);
        if (oceanPer == 1f)
        {
            soilAbsorption = 0f;
        }
        else
        {
            soilAbsorption = (float)((randy.NextDouble() * 1.5 + .25) * (1.0 - oceanPer));
        }
    }

    // Choose a downstream flow
    private void ChooseDownstream(SingleValueLayer elevation, System.Random randy)
    {
        float myElevation = elevation.worldArray[x, z];
        List<string> options = Support.getDirectionAsStringBelow(false, x, z, SingleValueLayer.WORLDX, SingleValueLayer.WORLDZ, myElevation, elevation.worldArray);
        if(options.Count == 0)
        {
            downstream = null;
        }
        else
        {
            string choice = options[randy.Next(0, options.Count)];
            downstream = new Direction(choice);
            // Update upstream
        }
    }

}
