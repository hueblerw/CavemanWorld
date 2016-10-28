using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class River {

    // Static Variables
    public static DailyLayer upstreamToday;
    public static DailyLayer surfacewater;
    public static SingleValueLayer lastUpstreamDay;

    // Variables
    public int x;
    public int z;
    public string type;
    public Direction downstream;
    public List<Direction> upstream;
    private float flowrate;
    private float soilAbsorption;
    private float yesterdaySurface;

    // Constructor 
    public River(int x, int z, float hillPer, float oceanPer)
    {
        // Initialize the essentials
        this.x = x;
        this.z = z;
        this.yesterdaySurface = 0f;
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
        if (oceanPer == 1f)
        {
            flowrate = 0f;
            soilAbsorption = 1f;
        }
        else
        {
            flowrate = (float) Math.Round((((hillPer * 2 + randy.NextDouble() * 2 + 4) / (1.0 - oceanPer)) / 100.0), 4);
            flowrate = (float) ChooseMinOf(flowrate, .8);
            soilAbsorption = (float) ((randy.NextDouble() + .2) * (1.0 - oceanPer) * (1.0 - hillPer));
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
        }
    }

    // return the average surface water for a tile
    public static float AverageRiverLevel(int x, int z)
    {
        float sum = 0;
        for (int day = 0; day < 120; day++)
        {
            sum += surfacewater.worldArray[day][x, z];
        }
        return (sum / 120f);
    }

    // return the number of days the river is empty in a year
    public static int DaysEmpty(int x, int z)
    {
        int count = 0;
        for (int day = 0; day < 120; day++)
        {
            if(surfacewater.worldArray[day][x, z] == 0)
            {
                count++;
            }
        }
        return count;
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
    // PRESENTLY WITHOUT SNOW OR MELT OFF!!!!!!!!!!!!
    // Assumes that surface water is river/lake water
    // Calculate is based on:
    // Assumption that soil absorbs some amount of water/day statically
        // NOTE: This is based off of a random number representing unknown soil factors and rockiness gathered from hillPercentage
    // What flows downhill at a certain rate (4-8% depending on random soil factors and steepness of hills - i.e hillPercentage
    // Evaporation each day - see evaporation note
    // rainfall and snow melt runoff
    // What is inputted in from upstream
        // NOTE: Tiles can have multiple upstream values but only one downstream value
    public void CalculateSurfaceWater(int day, float rainfall, int temp, float humidity, System.Random randy)
    {
        // Inputs: Previous, rainfall, EACH upstream ***SNOW MELT***
        // Determine if sunny, cloudy, or rainy
        string weather = DetermineWeather(rainfall, humidity, randy);
        // Calculate the positive incoming water flow
        float current = yesterdaySurface + rainfall + PreviousUpstream(day);
        // Losses: Downstream, Evaporation, SoilAbsorption, Other
        float absorption = (float) ChooseMinOf(current, soilAbsorption);
        float evaportation = FindEvaporation(current, temp, humidity, weather);
        // Calculate the flow downstream
        float downstreamFlow = 0f;
        if (type != "lake")
        {
            // NOTE: don't flow if there is no downstream
            downstreamFlow = (float) Math.Round(current * flowrate, 2);
        }
        // Pass downstream flow to your target's upstream for tommorrow
        PassDownstreamToUpstream(day, downstreamFlow);
        // update today's levels
        current = (float) ChooseMaxOf(Math.Round(current - downstreamFlow - evaportation - absorption, 2), 0.0);
        yesterdaySurface = current;
        // Set the downstream layer correctly
        surfacewater.worldArray[day][x, z] = current;
    }


    // Calculate the Evaporation
    // *** Based upon basic physics from this website, with some personalized fudge factors for humidity ***
    // http://www.engineeringtoolbox.com/evaporation-water-surface-d_690.html
    // NOTE: assumption is made for simplicity that a river is an inverted pyramid with a height a fixed constant in relation to its surface area
        // This allows volume to correspond directly with surface area.  This is worked directly into the equations
        // I also assumed that humidity on rainy days is 100%, and 3 times greater on cloudy days as sunny days
        // Occurance of sunny days corresponds indirectly with the expectation of rainfall on a given day (i.e. the humidity number generated for the tile).
    private float FindEvaporation(float current, int temp, float humidity, string weather)
    {
        double xs;
        double x;
        double pws;
        double pw;
        float evaporation;
        double multiplier = 1.0;

        switch (weather)
        {
            case "cloudy":
                multiplier = 100.0;
                break;
            case "sunny":
                multiplier = 300.0;
                break;
        }
        pws = Math.Exp(77.345 + 0.0057 * ((temp + 459.67) * (5.0 / 9.0)) - 7235.0 / ((temp + 459.67) * (5.0 / 9.0))) / (Math.Pow((temp + 459.67) * (5.0 / 9.0), 8.2));
        xs = 0.62198 * pws / (101325.0 - pws);
        if(weather != "rainy")
        {
            pw = (humidity / multiplier) * pws;
        }
        else
        {
            pw = pws;
        }
        x = 0.62198 * pw / (101325.0 - pw);
        evaporation = (float) (((1260.0 * 24.0 * Math.Sqrt(2.0 * current * 23.6) / Math.Pow(6.0, .25)) * (xs - x)) / 23600.0);

        return evaporation;
    }
    

    // Private Method for getting the previous day number
    private float PreviousUpstream(int day)
    {
        
        if (day == 0)
        {
            return lastUpstreamDay.worldArray[x, z];
        }
        else
        {
            return upstreamToday.worldArray[day - 1][x, z];
        }
    }

    // Determine the weather string
    private string DetermineWeather(float rainfall, float humidity, System.Random randy)
    {
        string weather;
        if (rainfall > 0)
        {
            weather = "rainy";
        }
        else
        {
            weather = DetermineCloudy(humidity, randy);
        }
        return weather;
    }

    // Determine if it is cloudy or not
    private string DetermineCloudy(float humidity, System.Random randy)
    {
        string weather;
        double prob = Math.Pow(0.5, humidity / 5.0);

        if (randy.NextDouble() < prob)
        {
            weather = "sunny";
        }
        else
        {
            weather = "cloudy";
        }
        return weather;
    }

    // Pass the downstream value onto the upstream value for the future
    private void PassDownstreamToUpstream(int day, float downstreamValue)
    {
        if (downstream != null)
        {
            int[] coor = downstream.getCoordinateArray(x, z);
            upstreamToday.worldArray[day][coor[0], coor[1]] += downstreamValue;
            if (day == 119)
            {
                lastUpstreamDay.worldArray[x, z] += downstreamValue;
            }
        }
        
    }

    // Choose the max of two doubles
    private double ChooseMaxOf(double a, double b)
    {
        if(a > b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    private double ChooseMinOf(double a, double b)
    {
        if (a < b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    // Testing method for returning private data as a string
    public string getPrivateVariables()
    {
        return flowrate * 100f + "% - " + soilAbsorption;
    }
    
}
