using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class River {

    // Static Variables
    public static DailyLayer upstreamToday;
    public static DailyLayer surfacewater;
    public static SingleValueLayer lastUpstreamDay = new SingleValueLayer("First Day of Next Year Upstream", "Yearly", 2);

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
    // PRESENTLY WITHOUT SNOW OR MELT OFF!!!!!!!!!!!!
    public void CalculateSurfaceWater(int day, float rainfall, int temp, float humidity, System.Random randy)
    {
        // Inputs: Previous, rainfall, EACH upstream ***SNOW MELT***
        // Determine if sunny, cloudy, or rainy
        string weather = DetermineWeather(rainfall, humidity, randy);
        // Calculate the positive incoming water flow
        float current = yesterdaySurface + rainfall + PreviousUpstream(day);
        // Losses: Downstream, Evaporation, SoilAbsorption, Other
        float absorption = current * soilAbsorption;
        float evaportation = FindEvaporation(current, temp, humidity, weather);
        // Calculate the flow downstream
        float downstream = current * flowrate;
        // Pass downstream flow to your target's upstream for tommorrow
        PassDownstreamToUpstream(day, downstream);
        // update today's levels
        current = current - downstream - evaportation - absorption;
        yesterdaySurface = current;
        // Set the downstream layer correctly
        surfacewater.worldArray[day][x, z] = current;
    }

    
    // Calculate the Evaporation
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
    private void PassDownstreamToUpstream(int day, float downstream)
    {
        int[] coor = this.downstream.getCoordinateArray(x, z);
        upstreamToday.worldArray[day][coor[0], coor[1]] += downstream;
        if (day == 119)
        {
            lastUpstreamDay.worldArray[x, z] += downstream;
        }
    }
    

}
