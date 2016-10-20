using UnityEngine;
using System.Collections;
using System;

public class River {

    // Static Variables
    public static DailyLayer upstreamToday;
    public static DailyLayer surfacewater;

    // Variables
    public int x;
    public int z;
    public string type;
    public Direction downstream = null;
    public Direction[] upstream = null;
    private float flowrate;
    private float soilAbsorption;

    // Constructor 
    public River(int x, int z, float hillPer, float oceanPer)
    {
        this.x = x;
        this.z = z;
        // Generate the Directions
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

}
