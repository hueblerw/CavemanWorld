using System;

public class Trees {

    // Constants
    private const double SWAMPCONSTANT = .8;
    private const double OAKSEEDCONSTANT = 2.023809524 * 0.003020609;
    private const double PINESEEDCONSTANT = 2.777777778 * 0.003856682;
    private const double TROPFOILAGECONSTANT = 1.5 * 0.002314009;
    private const double TROPICALEAFGROWTH = 1.2;
    private const double ARTICLEAFGROWTH = 0.8;
    private const double MONSOONFORESTLEAFAGE = 1.5;
    private const double RAINFORESTLEAFAGE = 2.0;
    private const double SWAMPLEAFAGE = 1.25;

    // Variables
    public int oakTreesRemoved;
    public int tropicalTreesRemoved;
    public int pineTreesRemoved;

    // Constructor
    public Trees()
    {
        oakTreesRemoved = 0;
        tropicalTreesRemoved = 0;
        pineTreesRemoved = 0;
    }


    // Get the number of any tree type
    // =SUM($AH$2, $AJ$2*0.8)*((($AJ$4/100))*3000+500)
    public int getTrees(string type, double[] habitatPer, int quality)
    {
        double treePer = 0.0;
        switch (type)
        {
            case "oaks":
                treePer = habitatPer[7] + habitatPer[8] * SWAMPCONSTANT;
                break;
            case "tropical":
                treePer = habitatPer[11] + habitatPer[12] * SWAMPCONSTANT;
                break;
            case "pine":
                treePer = habitatPer[3] + habitatPer[4] * SWAMPCONSTANT;
                break;
        }
        int trees = (int) Math.Round(treePer * (quality * 30.0 + 500.0), 0);
        return trees;
    }


    // Return the seeds the forest produces
    public double getSeeds(double[] habitatPer, int quality)
    {
        double seeds = getTrees("tropical", habitatPer, quality) * Habitat.SEEDCONSTANT;
        seeds += getTrees("oaks", habitatPer, quality) * OAKSEEDCONSTANT;
        seeds += getTrees("pine", habitatPer, quality) * PINESEEDCONSTANT;
        return seeds * 120.0;
    }


    // Return the Foilage available in the forest
    public double getFoilageProduced(double[] habitatPer, int quality, IntDayList temps)
    {
        // Shrub Foilage - ALL
        double foilage = (habitatPer[11] + habitatPer[12]) * Habitat.SHRUBCONSTANT * TROPFOILAGECONSTANT;
        foilage += (habitatPer[7] + habitatPer[8]) * Habitat.SHRUBCONSTANT;
        foilage += (habitatPer[3] + habitatPer[4]) * Habitat.SHRUBCONSTANT * ARTICLEAFGROWTH;
        // Scrub Foilage - FORESTS ONLY, NO SWAMP
        foilage = (habitatPer[11] + habitatPer[12]) * Habitat.SCRUBCONSTANT * TROPFOILAGECONSTANT;
        foilage += (habitatPer[7] + habitatPer[8]) * Habitat.SCRUBCONSTANT;
        foilage += (habitatPer[3] + habitatPer[4]) * Habitat.SCRUBCONSTANT * ARTICLEAFGROWTH;
        // Desert Scrub Foilage - NOT APPLICABLE IN FORESTS!
        // Forest Leaves - ALL
        foilage = (habitatPer[11] * MONSOONFORESTLEAFAGE + habitatPer[12] * RAINFORESTLEAFAGE) * Habitat.FORESTLEAVESCONSTANT;
        foilage += (habitatPer[7] + habitatPer[8] * SWAMPLEAFAGE) * Habitat.FORESTLEAVESCONSTANT;
        // Pine Leaves - ARTIC FORESTS ONLY
        foilage += (habitatPer[3] + habitatPer[4] * SWAMPLEAFAGE) * Habitat.PINENEEDLECONSTANT;
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
        return sum;
    }

}
