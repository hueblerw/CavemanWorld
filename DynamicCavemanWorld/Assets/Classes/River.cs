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
    public River(int x, int z, float hillPer, float oceanPer)
    {
        // Initialize the essentials
        this.x = x;
        this.z = z;
        this.upstream = new List<Direction>();
        // Generate the Directions
        if(oceanPer != 1f)
        {
            setRates(hillPer, oceanPer);
        }
        else
        {
            type = "ocean";
        }      
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
    public void ChooseDownstream(SingleValueLayer elevation, System.Random randy)
    {
        float myElevation = elevation.worldArray[x, z];
        List<string> options = Support.getDirectionAsStringBelow(false, x, z, SingleValueLayer.WORLDX, SingleValueLayer.WORLDZ, myElevation, elevation.worldArray);
        if(options.Count == 0)
        {
            downstream = null;
            type = "lake";
        }
        else
        {
            string choice = options[randy.Next(0, options.Count)];
            downstream = new Direction(choice);
            type = "river";
            // Update upstream
        }
    }

    // printUpstream array as a string
    public string printUpstream()
    {
        string output = "";
        foreach (Direction direction in upstream)
        {
            output += direction.direction + ", ";
        }
        return output;
    }

    // Calculate the surface water

    // Calculate the flow downstream

    // Pass downstream flow to your target's upstream for tommorrow

}
