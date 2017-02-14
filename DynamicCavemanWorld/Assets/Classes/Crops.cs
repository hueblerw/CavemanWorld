using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops {

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
    
    // Constructor
    public Crops()
    {
        usedCrops = new float[15];
        missingCrops = false;
    }


    // Update the usedCrops array
    public void UseSomeCrops(float[] consumed)
    {
        // Consume any crops consumed passed in.

        // Regenerate any crops that can regenerate.

    }


    // Calculate how much of a crop is present upon request
    public float[] ReturnCurrentCropArray(int day, DailyLayer rainfall, IntDayList temps)
    {

    }


    // Determine if starting on today the previous days have a suitable temperature range.
    public bool DayTempAllowCrop(int day, IntDayList temps)
    {
        if (day - growthPeriod > 0)
        {
            int startGrowthDay = day - growthPeriod;
            for (int d = day; d > startGrowthDay; d--)
            {
                if (temps.getDaysTemp(d) < minTemp || temps.getDaysTemp(d) > maxTemp)
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

}
