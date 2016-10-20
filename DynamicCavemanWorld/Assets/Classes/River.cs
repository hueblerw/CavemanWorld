using UnityEngine;
using System.Collections;

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
    public River(int x, int z)
    {
        this.x = x;
        this.z = z;
        // Generate the Directions
        // Then Determine the type
        //  Use hillPer to determine the flow and soilAbsorption rates
    }

}
