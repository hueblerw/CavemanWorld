using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops {

    // FOUR HUGE ISSUES REMAIN:
    // CONSUMPTION REDUCING THE AVAILABLE ???
    // OVERLAP FROM THE PREVIOUS YEAR NEEDS TO BE ALLOWED

    // Constants
    private const int NUM_OF_CROPS = 12;

    // Variables
    public float[] usedCrops;
    public bool missingCrops;
    private int maxTemp;
    private int minTemp;
    private double minWater;
    private double maxWater;
    private int growthPeriod;
    private double humanFoodUnits;
    private string cropName;
    private double rainSum;
    private double percentGrowable;
    
    // Constructor
    public Crops()
    {
        usedCrops = new float[NUM_OF_CROPS];
        missingCrops = false;
    }


    // Update the usedCrops array
    public void UseSomeCrops(float[] consumed)
    {
        // Consume any crops consumed passed in.

        // Regenerate any crops that can regenerate.

    }


    // Return an array containing totals of each crop the last X Days.
    public double[] SumCropsForLastX(int day, int lastXDays, int x, int z, DailyLayer rainfall, IntDayList temps, DailyLayer rivers)
    {
        double[] cropSum = new double[NUM_OF_CROPS];
        double[] cropDay = new double[NUM_OF_CROPS];
        for (int d = day - lastXDays + 1; d < day; d++)
        {
            cropDay = ReturnCurrentCropArray(d, x, z, rainfall, temps, rivers);
            for (int i = 0; i < NUM_OF_CROPS; i++)
            {
                cropSum[i] += cropDay[i];
            }
        }
        return cropSum;
    }


    // Return an array containing totals of each crop for the entire year.
    public double[] SumCropsForYear(int x, int z, DailyLayer rainfall, IntDayList temps, DailyLayer rivers)
    {
        double[] cropSum = new double[NUM_OF_CROPS];
        double[] cropDay = new double[NUM_OF_CROPS];
        for (int d = 0; d < 120; d++)
        {
            cropDay = ReturnCurrentCropArray(d, x, z, rainfall, temps, rivers);
            for (int i = 0; i < NUM_OF_CROPS; i++)
            {
                cropSum[i] += cropDay[i];
            }
        }
        return cropSum;
    }


    // Print the Current Crop Array
    public string PrintCurrentCropArray(int day, int x, int z, DailyLayer rainfall, IntDayList temps, DailyLayer rivers)
    {
        // get today's crop array
        double[] cropArray = ReturnCurrentCropArray(day, x, z, rainfall, temps, rivers);
        string printString = "";
        // convert all non-zero values to the appropriate display strings
        for (int i = 0; i < NUM_OF_CROPS; i++)
        {
            if (cropArray[i] != 0.0)
            {
                printString += SwitchName(i) + ": " + cropArray[i] + "\n";
            }
        }

        return printString;
    }


    // Print the Year's Crop Array
    public string PrintYearsCropArray(int x, int z, DailyLayer rainfall, IntDayList temps, DailyLayer rivers)
    {
        // get year's crop array
        double[] cropArray = SumCropsForYear(x, z, rainfall, temps, rivers);
        string printString = "";
        // convert all non-zero values to the appropriate display strings
        for (int i = 0; i < NUM_OF_CROPS; i++)
        {
            if (cropArray[i] != 0.0)
            {
                printString += SwitchName(i) + ": " + cropArray[i] + "\n";
            }
        }

        return printString;
    }


    // NOTE ***************
    // So far the crops can't grow early in the year because for that they need access to information from the previous year.
    // Implementation of that will be a bit tricky so I am saving it for later.
    // Also, these represent the number of new crops that grew today.  A scavenger would have access to the last x days worth of crops.
    // Calculate how much of a crop is present upon request
    public double[] ReturnCurrentCropArray(int day, int x, int z, DailyLayer rainfall, IntDayList temps, DailyLayer rivers)
    {
        double[] currentCrops = new double[NUM_OF_CROPS];
        percentGrowable = 1.0;
        // For each of the crops
            // If he crop can grow in the region return the crops store the crops returned value in the current crop array for today.
        for (int i = 0; i < NUM_OF_CROPS; i++)
        {
            SwitchVariables(i);
            if (DayTempAllowCrop(day, temps) && DayRainAllowCrop(day, x, z, rainfall, rivers))
            {
                // Debug.Log("Crops Allowed!");
                double cropMultiplier = (1.0 / ((120 - growthPeriod) * 100.0)) * 400.0;
                // Calculate the crop quality
                currentCrops[i] = cropQuality(day, temps) * cropMultiplier * humanFoodUnits * percentGrowable;
                // Debug.Log(x + ", " + z + " / " + cropQuality(day, temps) + " / " + percentGrowable + " / " + humanFoodUnits);
            }
        }

        return currentCrops;
    }


    // Determine if starting on today the previous days have a suitable temperature range.
    private bool DayTempAllowCrop(int day, IntDayList temps)
    {
        // Can grow if the temperature is within +/- 10 degrees of the ideal temperature range
        if (day - growthPeriod > 0)
        {
            int startGrowthDay = day - growthPeriod;
            for (int d = day; d > startGrowthDay; d--)
            {
                if (temps.getDaysTemp(d) < minTemp - 10 || temps.getDaysTemp(d) > maxTemp + 10)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    // Return the growing crops Quality
    private double cropQuality(int day, IntDayList temps)
    {
        int goodDays = 0;
        int startGrowthDay = day - growthPeriod;
        // temperature multiplier is % of days that are within the ideal temperature range.
        for (int d = day; d > startGrowthDay; d--)
        {
            if (temps.getDaysTemp(d) >= minTemp || temps.getDaysTemp(d) <= maxTemp)
            {
                goodDays++;
            }
        }
        // rain multiplier is between 50% and 125%  based on how close to ideal the rainfall level was.
        double maxDist = (maxWater - minWater) / 2.0;
        double idealRain = (maxWater + minWater) / 2.0;
        double rainMultiplier = 1.25 - ((Math.Abs(rainSum - idealRain) / maxDist) * .75);
        // return the two modifiers used together.
        return (goodDays / growthPeriod) * rainMultiplier * 100.0;
    }


    // Determine if starting on today the previous days have a suitable rainfall sum.
    private bool DayRainAllowCrop(int day, int x, int z, DailyLayer rain, DailyLayer rivers)
    {
        // can grow ONLY if the rainfall is within the ideal rainfall range
        const double RIVERWATERINGCONSTANT = .2;
        double sum = 0;
        double surfaceSum = 0;
        if (day - growthPeriod > 0)
        {
            int startGrowthDay = day - growthPeriod;
            // Sum the rainfall in the crops growing period
            for (int d = day; d > startGrowthDay; d--)
            {
                sum += rain.worldArray[d][x, z];
                surfaceSum += rivers.worldArray[d][x, z] * RIVERWATERINGCONSTANT;
            }
            // If that sum is in the acceptable range set the rainSum variable and return true, else return false.
                // Ideally if any value in the range of values from sum to sum + surfacewaterSum is between minWater and maxWater
                // Return true and rainSum set and percentGrowable set.
                // Else return false
            if ((sum > minWater && sum < maxWater) || (sum < minWater && (sum + surfaceSum) > minWater))
                {
                // Set the rainSum with the midpoint of the trapezoid of possible return values.
                rainSum = Average(Math.Min(sum + surfaceSum, maxWater), Math.Max(sum, minWater));
                // Set the percent Growable as the percent of the tile that actually is in the acceptable water range.
                if (surfaceSum == 0)
                {
                    percentGrowable = 1.0;
                }
                else
                {
                    percentGrowable = (Math.Min(sum + surfaceSum, maxWater) - Math.Max(sum, minWater)) / surfaceSum;
                }
                // return true because stuff grows here
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    // Crop Variable switch statement
    private void SwitchVariables(int cropNum)
    {
        switch (cropNum)
        {
            // Wheat
            case 0:
                cropName = "wheat";
                minTemp = 75;
                maxTemp = 95;
                minWater = 4.9;
                maxWater = 7.9;
                growthPeriod = 78;
                humanFoodUnits = 2.52;
                break;
            // Corn
            case 1:
                cropName = "corn";
                minTemp = 68;
                maxTemp = 88;
                minWater = 14.8;
                maxWater = 22.2;
                growthPeriod = 28;
                humanFoodUnits = 1.08;
                break;
            // Potato
            case 2:
                cropName = "potato";
                minTemp = 58;
                maxTemp = 80;
                minWater = 12.3;
                maxWater = 19.7;
                growthPeriod = 39;
                humanFoodUnits = 1.34;
                break;
            // Apple Tree
            case 3:
                cropName = "apple";
                minTemp = 48;
                maxTemp = 70;
                minWater = 12.3;
                maxWater = 19.7;
                growthPeriod = 39;
                humanFoodUnits = 1.29;
                break;
            // Grapes
            case 4:
                cropName = "grape";
                minTemp = 71;
                maxTemp = 91;
                minWater = 7.4;
                maxWater = 12.3;
                growthPeriod = 51;
                humanFoodUnits = 1.03;
                break;
            // Beans
            case 5:
                cropName = "beans";
                minTemp = 66;
                maxTemp = 86;
                minWater = 4.9;
                maxWater = 9.9;
                growthPeriod = 39;
                humanFoodUnits = .32;
                break;
            // Rice
            case 6:
                cropName = "rice";
                minTemp = 67;
                maxTemp = 90;
                minWater = 24.7;
                maxWater = 32.1;
                growthPeriod = 58;
                humanFoodUnits = 2.30;
                break;
            // Onions
            case 7:
                cropName = "onions";
                minTemp = 70;
                maxTemp = 90;
                minWater = 7.4;
                maxWater = 9.9;
                growthPeriod = 51;
                humanFoodUnits = .84;
                break;
            // Carrots
            case 8:
                cropName = "carrots";
                minTemp = 65;
                maxTemp = 85;
                minWater = 7.4;
                maxWater = 9.9;
                growthPeriod = 39;
                humanFoodUnits = .77;
                break;
            // Roots
            case 9:
                cropName = "roots";
                minTemp = 60;
                maxTemp = 90;
                minWater = 12.3;
                maxWater = 24.7;
                growthPeriod = 35;
                humanFoodUnits = .43;
                break;
            // Berries
            case 10:
                cropName = "berries";
                minTemp = 43;
                maxTemp = 79;
                minWater = 12.3;
                maxWater = 24.7;
                growthPeriod = 39;
                humanFoodUnits = .37;
                break;
            // Nuts
            case 11:
                cropName = "nuts";
                minTemp = 52;
                maxTemp = 73;
                minWater = 19.7;
                maxWater = 34.5;
                growthPeriod = 69;
                humanFoodUnits = 1.0; // ?????????????????
                break;
        }
    }


    // Crop get name only statement
    private string SwitchName(int cropNum)
    {
        string name = "";
        switch (cropNum)
        {
            // Wheat
            case 0:
                name = "wheat";
                break;
            // Corn
            case 1:
                name = "corn";
                break;
            // Potato
            case 2:
                name = "potato";
                break;
            // Apple Tree
            case 3:
                name = "apple";
                break;
            // Grapes
            case 4:
                name = "grape";
                break;
            // Beans
            case 5:
                name = "beans";
                break;
            // Rice
            case 6:
                name = "rice";
                break;
            // Onions
            case 7:
                name = "onions";
                break;
            // Carrots
            case 8:
                name = "carrots";
                break;
            // Roots
            case 9:
                name = "roots";
                break;
            // Berries
            case 10:
                name = "berries";
                break;
            // Nuts
            case 11:
                name = "nuts";
                break;
        }

        return name;
    }


    // Averages two doubles
    private double Average(double a, double b)
    {
        return (a + b) / 2.0;
    }


}
