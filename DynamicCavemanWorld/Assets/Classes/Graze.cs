using System;

public class Graze {

    // Variables
    public double availableGrazingPercent;
    // Constants
    private const double GRASSCALORIECONTENT = .067311605;
    private const double DESERTGROWTHFACTOR = .3;

    // Constructor
    public Graze()
    {
        availableGrazingPercent = 1.0;
    }

    // NOTE: NOT IMPLEMENTED
    // OVERGRAZING
    // Balance

    // Return the grass number for today
    public double getGrass(int quality, double oceanPer, double grassPercent, double desertPercent, double last5Rain, float temp)
    {
        // =(1.2-ABS(70-AA7)/70)*400*SUM($AF$1:$AF$3)*(($AJ$4-50)/200+1)+(1.2-ABS(70-AA7)/70)*400*(($AJ$4-50)/200+1)*SUM($AD$1:$AD$3)*0.3*(0.5+SUM(AB7:AB11)/10)
        double tempfactor = Math.Max((1.2 - Math.Abs(70.0 - temp) / 70.0), 0.0);
        double qualityfactor = ((quality - 50.0) / 200.0 + 1.0);
        // Calculate the grass
        double grass = tempfactor * 400 * grassPercent * qualityfactor;
        grass += tempfactor * 400 * desertPercent * qualityfactor * DESERTGROWTHFACTOR * (.5 + (last5Rain / 10.0));
        // Feed in the oceanPercentage and overgrazing factor
        grass = grass * (1 - oceanPer) * availableGrazingPercent;

        return grass;
    }


    // Return the grazing available for this square today.
    public double getGrazing(int day, int x, int z, int quality, double oceanPer, double[] habitatPercents, DailyLayer rainfall, IntDayList temps, DailyLayer surfaceWater)
    {
        // Get the % of terrain that has grass
        double grassPercent = 0.0;
        double desertPercent = 0.0;
        for (int i = 0; i < 9; i += 4)
        {
            grassPercent += habitatPercents[2 + i];
            desertPercent += habitatPercents[1 + i];
        }
        double last5Rain = Last5DaysOfRain(day, x, z, rainfall, surfaceWater);
        // Calculate the grass mass.
        double grass = getGrass(quality, oceanPer, grassPercent, desertPercent, last5Rain, temps.getDaysTemp(day));
        // Calculate the grazing available
        double grazing = grass * GRASSCALORIECONTENT;

        return grazing;
    }


    // Return the year's total foragable grazing.
    // Used by the game and herds to determine food availability
    public double YearsGrazingForage(int x, int z, int quality, double oceanPer, double[] habitatPercents, DailyLayer rainfall, IntDayList temps, DailyLayer surfaceWater)
    {
        double sum = 0.0;
        for (int d = 0; d < 120; d++)
        {
            sum += getGrazing(d, x, z, quality, oceanPer, habitatPercents, rainfall, temps, surfaceWater);    
        }
        return (sum * Habitat.FORAGECONSTANT);
    }


    // Returns the seeds produced by the grazing
    public double getSeeds(int x, int z, int quality, double oceanPer, double[] habitatPercents, DailyLayer rainfall, IntDayList temps, DailyLayer surfaceWater)
    {
        double seeds = (YearsGrazingForage(x, z, quality, oceanPer, habitatPercents, rainfall, temps, surfaceWater) / Habitat.FORAGECONSTANT) / GRASSCALORIECONTENT;
        return (seeds * Habitat.SEEDCONSTANT);
    }


    // Return the Foilage available in the forest
    public double getFoilageProduced(double[] habitatPer, int quality, IntDayList temps)
    {
        // Shrub Foilage - PLAINS ONLY, NO DESERTS
        double foilage = habitatPer[10] * Habitat.SHRUBCONSTANT * Habitat.TROPICALEAFGROWTH;
        foilage += habitatPer[6] * Habitat.SHRUBCONSTANT;
        foilage += habitatPer[2] * Habitat.SHRUBCONSTANT * Habitat.ARTICLEAFGROWTH;
        // Scrub Foilage - ALL
        foilage = (habitatPer[9] + habitatPer[10]) * Habitat.SCRUBCONSTANT * Habitat.TROPICALEAFGROWTH;
        foilage += (habitatPer[5] + habitatPer[6]) * Habitat.SCRUBCONSTANT;
        foilage += (habitatPer[1] + habitatPer[2]) * Habitat.SCRUBCONSTANT * Habitat.ARTICLEAFGROWTH;
        // Desert Scrub Foilage - DESERT ONLY
        foilage += (habitatPer[1] + habitatPer[5] + habitatPer[9]) * Habitat.DESERTSCRUBCONSTANT;
        // Forest Leaves - NONE
        // Temperature Effect
        double sum = 0.0;
        int todayTemp;
        for (int d = 0; d < 120; d++)
        {
            todayTemp = temps.getDaysTemp(d);
            if (todayTemp > 50)
                sum += foilage;
            else
            {
                if (todayTemp > 30)
                {
                    sum += ((todayTemp - 30.0) / 20.0) * foilage;
                }
            }
        }
        // Pine Leaves - NONE - NOTE THESE ARE EVERGREEN AND BLOOM EVENLY ALL YEAR!
        return sum;
    }


    // Get last 5 Days of Rain
    private double Last5DaysOfRain(int day, int x, int z, DailyLayer rainfall, DailyLayer surfaceWater)
    {
        double last5Rain = 0.0;
        for (int d = day; d > 0 && d > day - 5; d--)
        {
            last5Rain += rainfall.worldArray[d][x, z] + surfaceWater.worldArray[day][x, z] * (Habitat.RIVERWATERINGCONSTANT / 2.0);
        }

        return last5Rain;
    }


}
