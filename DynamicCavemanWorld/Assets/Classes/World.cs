using UnityEngine;
using System.Collections;
using System;

public class World {

    // Global Layers
    public int WorldX;
    public int WorldZ;
    public float maxElevationDifference;
    // Elevation Layers
    public SingleValueLayer elevation;
    public SingleValueLayer elevationVertices;
    public SingleValueLayer hillPer;
    public SingleValueLayer oceanPer;
    // Temperature Layers
    private SingleValueLayer highTemp;
    private SingleValueLayer lowTemp;
    private SingleValueLayer tempMidpt;
    private SingleValueLayer variance;
    public EquationLayer tempEquations;
    public IntDayList[,] temps;
    // Rainfall Layers - (temporarily a very simple version with a single humidity number per tile)
    private HumidityLayer humidity;
    public DailyLayer rainfall;
    public DailyLayer snow;
    public SingleValueLayer rainfallTotal;
    // River Layers
    public ObjectLayer riverStats;
    // Habitat Layer
    public HabitatLayer habitats;

    // Constructor
    public World(int x, int z, bool random)
    {
        // Initialize the variables
        WorldX = x;
        WorldZ = z;
        SingleValueLayer.WORLDX = WorldX;
        SingleValueLayer.WORLDZ = WorldZ;
        this.elevation = new SingleValueLayer("Elevation", "Semi-static", 1);
        this.elevationVertices = new SingleValueLayer("ElevationVertices", "Semi-static", 1);
        this.highTemp = new SingleValueLayer("HighTemp", "Semi-static", 0);
        this.lowTemp = new SingleValueLayer("LowTemp", "Semi-static", 0);
        this.tempMidpt = new SingleValueLayer("TempMidpoint", "Semi-static", 1);
        this.variance = new SingleValueLayer("Variance", "Semi-static", 1);
        this.tempEquations = new EquationLayer("TemperatureEquations", "Semi-static");
        this.temps = new IntDayList[WorldX, WorldZ];
        this.humidity = new HumidityLayer("HumidityLayer", 6, 1);

        if (!random)
        {
            string filePathPrefix = @"CSV\";
            elevation.readCSVFile(filePathPrefix + "ElevationNiceMapA");
            // Debug.Log("**************************************");
            highTemp.readCSVFile(filePathPrefix + "HighTempNiceMapA");
            lowTemp.readCSVFile(filePathPrefix + "LowTempNiceMapA");
            tempMidpt.readCSVFile(filePathPrefix + "MidptNiceMapA");
            variance.readCSVFile(filePathPrefix + "VarianceNiceMapA");
            humidity.readCSVFiles(filePathPrefix);
        }
        else
        {
            DataGenerator generator = new DataGenerator(WorldX, WorldZ);
            Debug.Log(x + ", " + z);
            // Generate elevation layer
            elevation.worldArray = generator.CreateElevationLayer();
            // Generate temperature info
            float[][,] temporaryTemps = generator.CreateTemperatureLayers(4);
            highTemp.worldArray = temporaryTemps[0];
            lowTemp.worldArray = temporaryTemps[1];
            tempMidpt.worldArray = generator.CreateStandardFloatLayer(20.2, 40.6, 4.0);
            variance.worldArray = generator.CreateStandardFloatLayer(1.0, 16.0, 2.0);
            // Generate rain info
            for (int i = 0; i < 6; i++){
                humidity.worldArray[i] = generator.CreateStandardFloatLayer(0.0, 10.0, 1.0);
            }
        }

        // Elevation info
        ConvertElevationToVertices();
        hillPer = CalculateHillPercentage();
        oceanPer = CalculateOceanPercentage();
        Debug.Log("Elevation Models Complete!");
        // Temperature info
        tempEquations.createEquations(highTemp, lowTemp, tempMidpt, variance);
        // Calculate Years worth of temperature data
        // CreateYearsTemps();
        Debug.Log("Temperature Models Complete!");
        // Rainfall info
        rainfall = humidity.GenerateWorldsYearOfRain();
        rainfallTotal = new SingleValueLayer("Yearly Rain Total", "Yearly", 1);
        // rainfallTotal.worldArray = rainfall.findYearTotalArray();
        Debug.Log("Rainfall Models Complete!");
        // Rivers info
        // Initialize Water Stats
        riverStats = new ObjectLayer("River Stats", "Semi-static");
        PopulateRivers();
        Debug.Log("Rivers Populated!");
        ResetStaticRiverLayers();
        ResetLastDayLayer();
        // Calculate Habitat Layer - ** for that we need 20 years of time run forward at initialization **
        HabitatInitialization();
        // When done initializing the habitats calculate a new year
        TempAndRainNewYear();
        Debug.Log("Habitats Created!");
    }

    // Converts the model's elevation number to a map of vertices which can be used by the view
    public void ConvertElevationToVertices()
    {
        for (int x = 0; x < WorldX + 1; x++)
        {
            for (int z = 0; z < WorldZ + 1; z++)
            {
                elevationVertices.worldArray[x, z] = VertexAverage(Support.CellsAroundVertex(x, z, WorldX, WorldZ, this.elevation.worldArray));
            }
        }
    }

    public void TempAndRainNewYear()
    {
        // Create Temperatures
        CreateYearsTemps();
        // Create Rainfall
        rainfall = humidity.GenerateWorldsYearOfRain();
        rainfallTotal.worldArray = rainfall.findYearTotalArray();
        // Sort Into Snowfall
        snow = SortSnowDayByTemps(temps, rainfall);
        // Calculate River Flow
        CalculateRiverYear();
    }


    // Sorts the days it snows from the days it rains
    private DailyLayer SortSnowDayByTemps(IntDayList[,] temps, DailyLayer rainfall)
    {
        DailyLayer snowfall = new DailyLayer("snow", 1);

        for (int day = 0; day < 120; day++)
        {
            for (int x = 0; x < WorldX; x++)
            {
                for (int z = 0; z < WorldZ; z++)
                {
                    if(temps[x, z].getDaysTemp(day) <= 32)
                    {
                        snowfall.worldArray[day][x, z] = rainfall.worldArray[day][x, z];
                        rainfall.worldArray[day][x, z] = 0f;
                    }
                }
            }
        }

        return snowfall;
    }


    // Updates the Habitat from year to year based on new data
    private void HabitatUpdate()
    {
        System.Random randy = new System.Random();
        for(int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                habitats.worldArray[x, z].UpdateHabitatYear(temps[x, z].Count70DegreeDays(), temps[x, z].Count32DegreeDays(), rainfallTotal.worldArray[x, z], River.AverageRiverLevel(x, z), River.surfaceSnow.AreAllZero(x, z), randy);
            }
        }
    }

    public void NewYear()
    {
        TempAndRainNewYear();
        // Update the Habitats this year
        HabitatUpdate();
        Debug.Log("Generated a new year!");
    }

    // Makes the string for the tile info display
    // Should probably be part of a view
    public string getTileInfo(int day, int x, int z)
    {
        string info = "Elevation: " + elevation.worldArray[x, z];
        info += "\n" + "Ocean %: " + oceanPer.worldArray[x, z] * 100f + "%";
        info += "\n" + "Hill %: " + hillPer.worldArray[x, z] * 100f + "%";
        info += "\n" + "Temp: " + temps[x, z].getDaysTemp(day);
        info += "\n" + "Rain: " + rainfall.worldArray[day][x, z];
        info += "\n" + "Snow: " + snow.worldArray[day][x, z];
        if (oceanPer.worldArray[x, z] != 1.0)
        {
            info += "\n" + "River Direction: " + riverStats.worldArray[x, z].downstream;
            info += "\n" + "Snow Cover: " + River.surfaceSnow.worldArray[day][x, z];
            info += "\n" + "River Level: " + River.surfacewater.worldArray[day][x, z];
            info += "\n" + "flowrate - absorbtion: " + riverStats.worldArray[x, z].getPrivateVariables();
            info += "\n" + "Upstream Directions: " + riverStats.worldArray[x, z].printUpstream();
            info += "\n" + "Upstream River Amount: " + River.upstreamToday.worldArray[day][x, z];
            info += "\n" + habitats.worldArray[x, z];
            info += "\n" + "Crops Today:";
            info += "\n" + habitats.worldArray[x, z].crops.PrintCurrentCropArray(day, x, z, oceanPer.worldArray[x, z], rainfall, temps[x, z], River.surfacewater);
            info += "\n" + "Crops Year Total:";
            info += "\n" + habitats.worldArray[x, z].crops.PrintYearsCropArray(x, z, oceanPer.worldArray[x, z], rainfall, temps[x, z], River.surfacewater);
                // info += "\n" + "Grazing Today:";
                // info += habitats.worldArray[x, z].grazing.getGrazing(day, x, z, habitats.worldArray[x, z].quality, oceanPer.worldArray[x, z], habitats.worldArray[x, z].typePercents, rainfall, temps[x, z], River.surfacewater);
                // info += "\n" + "Foragable Grazing For the Year:";
                // info += habitats.worldArray[x, z].grazing.YearsGrazingForage(x, z, habitats.worldArray[x, z].quality, oceanPer.worldArray[x, z], habitats.worldArray[x, z].typePercents, rainfall, temps[x, z], River.surfacewater);
            info += "\n" + "Grazing / Seeds / Foiliage Year Totals:";
            info += "\n" + habitats.worldArray[x, z].grazing.YearsGrazingForage(x, z, habitats.worldArray[x, z].quality, oceanPer.worldArray[x, z], habitats.worldArray[x, z].typePercents, rainfall, temps[x, z], River.surfacewater);
            info += " / " + habitats.worldArray[x, z].getSeeds(x, z, rainfall, temps[x, z], River.surfacewater);
            info += " / " + habitats.worldArray[x, z].getFoilage(temps[x, z]);
        }
        return info;
    }

    // Private methods!
    // Calculate a Years Temperatures
    private void CreateYearsTemps()
    {
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                temps[x, z] = tempEquations.worldArray[x, z].generateYearsTemps();
            }
        }
    }

    // Find the value of the vertex at the grid crossings
    private float VertexAverage(float[] cellsAround)
    {
        int arrayLength = cellsAround.Length;
        float average = 0.0f;
        for (int i = 0; i < arrayLength; i++)
        {
            average += cellsAround[i];
        }
        average = average / arrayLength;
        return average;
    }

    // Create the HillPercentage Layer
    private SingleValueLayer CalculateHillPercentage()
    {
        // Initialize a new layer
        float diff;
        SingleValueLayer hillPer = new SingleValueLayer("Hill Percentage", "Semi-static", 4);
        // Stash the max elevation difference
        maxNetDiff();
        // Calculate the hill %'s for the whole world
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                diff = netDiff(x, z);
                hillPer.worldArray[x, z] = (float)Math.Round(diff / maxElevationDifference, 4);
            }
        }
        return hillPer;
    }

    // Create the Ocean Percentage Layer
    private SingleValueLayer CalculateOceanPercentage()
    {
        // Initialize a new layer
        SingleValueLayer oceanPer = new SingleValueLayer("Ocean Percentage", "Semi-static", 4);
        // Find the vertexes, sum th abs of all negative values and divide by sum of abs of all values.
        float[,] vertexArray = elevationVertices.worldArray;
        float[] corners;
        float sumOfNegatives;
        float sumOfAll;

        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                corners = Support.CellsAroundVertex(x + 1, z + 1, WorldX, WorldZ, vertexArray);
                sumOfNegatives = FindAbsOfNegatives(corners);
                sumOfAll = FindAbsSum(corners);
                oceanPer.worldArray[x, z] = sumOfNegatives / sumOfAll;
            }
        }

        return oceanPer;
    }

    // Calculate the NetDifference around a cell
    private float netDiff(int x, int z)
    {
        float diff = 0f;
        float[,] array = elevation.worldArray;
        float[] cellsAround = Support.CellsAllAround(x, z, WorldX, WorldZ, array);
        foreach (float element in cellsAround)
        {
            diff += Mathf.Abs(array[x, z] - element);
        }
        return diff / cellsAround.Length;
    }

    // Calculate the maximum netDifference and store it as a private variable.
    private void maxNetDiff()
    {
        float maxDiff = 0f;
        float diff;
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                diff = netDiff(x, z);
                if (diff > maxDiff)
                {
                    maxDiff = diff;
                }
            }
        }
        maxElevationDifference = maxDiff;
    }

    // Calculate the abs of the sum of all negative numbers in the array
    private float FindAbsOfNegatives(float[] corners)
    {
        float sum = 0f;
        foreach (float ele in corners)
        {
            if (ele < 0)
            {
                sum += ele;
            }
        }
        return Math.Abs(sum);
    }

    // Calculate the sum of the Absolute Value of all numbers
    private float FindAbsSum(float[] corners)
    {
        float sum = 0f;
        foreach (float ele in corners)
        {
            sum += Math.Abs(ele);
        }
        return sum;
    }

    // Populate the rivers object
    private void PopulateRivers()
    {
        // Initialize basic river objects
        riverStats.worldArray = new River[WorldX, WorldZ];
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                riverStats.worldArray[x, z] = new River(x, z, hillPer.worldArray[x, z], oceanPer.worldArray[x, z]);
            }
        }
        // Create the down and upstream node information
        System.Random randy = new System.Random();
        for (int x = 0; x < WorldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                if (elevation.worldArray[x, z] >= 0f)
                {
                    riverStats.worldArray[x, z].ChooseDownstream(elevation, randy);
                    UpdateUpstream(x, z, riverStats.worldArray[x, z].downstream);
                }
            }
        }

    }

    // Update the upstream direction nodes based on the downstream one that was just created
    private void UpdateUpstream(int x, int z, Direction direction)
    {
        if (direction != null)
        {
            switch (direction.direction)
            {
                case "up":
                    riverStats.worldArray[x, z - 1].upstream.Add(new Direction("down"));
                    break;
                case "left":
                    riverStats.worldArray[x - 1, z].upstream.Add(new Direction("right"));
                    break;
                case "down":
                    riverStats.worldArray[x, z + 1].upstream.Add(new Direction("up"));
                    break;
                case "right":
                    riverStats.worldArray[x + 1, z].upstream.Add(new Direction("left"));
                    break;
            }
        }
    }


    // Resets the Static River layers
    private void ResetStaticRiverLayers()
    {
        River.upstreamToday = new DailyLayer("Upstream Waterflow", 2);
        River.surfacewater = new DailyLayer("Surface Water", 2);
        River.surfaceSnow = new DailyLayer("Surface Snow", 2);
    }


    // Resets the Static River layers
    private void ResetLastDayLayer()
    {
        River.lastUpstreamDay = new SingleValueLayer("First Day of Next Year Upstream", "Yearly", 2);
    }

    // Calcuate a year's worth of river data
    private void CalculateRiverYear()
    {
        // Initialize a random number generator
        System.Random randy = new System.Random();
        // Reset the 2 main layers
        ResetStaticRiverLayers();
        // Iterate
        for (int day = 0; day < 120; day++)
        {
            for (int x = 0; x < WorldX; x++)
            {
                for (int z = 0; z < WorldZ; z++)
                {
                    if(oceanPer.worldArray[x, z] != 1f)
                    {
                        // Account for snow fall ***LATER***
                        // Calculate the river flow
                        riverStats.worldArray[x, z].CalculateSurfaceWater(day, rainfall.worldArray[day][x, z], snow.worldArray[day][x, z], temps[x, z].getDaysTemp(day), humidity.CalculateHumidityFromBase(day, x, z), randy);
                    } 
                }
            }
            // if day == 1 Reset the lastRiverUpstream layer
            if (day == 1)
            {
                ResetLastDayLayer();
            }
        }
                
    }


    // HABITAT INITIALIZATION COUNTERS
    private void HabitatInitialization()
    {
        // Create the habitat type counters by running twenty years of habitats and seeing the results
        int[,][] habitatTypeCounters = CreateInitHabCounters();
        // Initialize the HabitatLayer
        habitats = new HabitatLayer(habitatTypeCounters, oceanPer.worldArray);
    }

    // Create the habitat initialization counters
    private int[,][] CreateInitHabCounters()
    {
        int[,][] habitatTypeCounters = new int[WorldX, WorldZ][];
        int index;
        string wetness;
        string temperateness;

        // Do this for twenty years
        for (int year = 0; year < 20; year++)
        {
            // Generate a new year
            TempAndRainNewYear();
            // Create arrays with the relevant data for calcluating the habitats
            for (int x = 0; x < WorldX; x++)
            {
                for (int z = 0; z < WorldZ; z++)
                {
                    // If the tile is 100% ocean initialize habitat counters to an empty array
                    if(oceanPer.worldArray[x, z] != 1f)
                    {
                        // initialize the habitattype counter with an empty array the first time round
                        if(year == 0)
                        {
                            habitatTypeCounters[x, z] = new int[13];
                        }
                        // Get the index of the expected habitat for each tile that year
                        wetness = Habitat.DetermineWetness(rainfallTotal.worldArray[x, z] + River.AverageRiverLevel(x, z));
                        temperateness = Habitat.DetermineTemp(temps[x, z].Count70DegreeDays(), temps[x, z].Count32DegreeDays());
                        index = Habitat.DetermineHabitatFavored(wetness, temperateness);
                        // Add a counter for that habitat to that tiles counter array
                        habitatTypeCounters[x, z][index] += 1;
                    }
                    else
                    {
                        if (year == 0)
                        {
                            habitatTypeCounters[x, z] = new int[13];
                        }
                    }    
                }
            }
        }

        return habitatTypeCounters;
    }

}
