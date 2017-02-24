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
    // USING THE RIVER NUMBERS FOR TO BOOST RAINFALL

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
    public double getGrazing(int day, int x, int z, int quality, double oceanPer, double[] habitatPercents, DailyLayer rainfall, IntDayList temps)
    {
        // Get the % of terrain that has grass
        double grassPercent = 0.0;
        double desertPercent = 0.0;
        for (int i = 0; i < 9; i += 4)
        {
            grassPercent += habitatPercents[2 + i];
            desertPercent += habitatPercents[1 + i];
        }
        double last5Rain = Last5DaysOfRain(day, x, z, rainfall);
        // Calculate the grass mass.
        double grass = getGrass(quality, oceanPer, grassPercent, desertPercent, last5Rain, temps.getDaysTemp(day));
        // Calculate the grazing available
        double grazing = grass * GRASSCALORIECONTENT;

        return grazing;
    }


    // Get last 5 Days of Rain
    private double Last5DaysOfRain(int day, int x, int z, DailyLayer rainfall)
    {
        double last5Rain = 0.0;
        for (int d = day; d > 0 && d > day - 5; d--)
        {
            last5Rain += rainfall.worldArray[d][x, z];
        }

        return last5Rain;
    }
}
