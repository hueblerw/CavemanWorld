using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMemory {

    // variables
    public Vector2 location;
    public float foodRecall;
    public float predatorThreat;

    // constructor
    public LocationMemory(Vector2 location, float initFood, float initPredators)
    {
        this.location = location;
        foodRecall = initFood;
        predatorThreat = initPredators;
    }

}
