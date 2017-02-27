

using System;

public class Game
{
    // Constants
    private const int NUM_OF_GAME_ANIMALS = 10;
    private const int NUM_OF_HERBIVORES = 7;
    // Variables
    public int[] overconsumedArray;
    // Game Stats
    double foodAvail;
    double foodConsumed;
    double abundance;
    double habitatSum;
    double meatValue;

    // Constructor
    public Game()
    {
        overconsumedArray = new int[NUM_OF_GAME_ANIMALS];
    }


    // Print the Year's Game Array
    public string PrintYearsGameArray(double oceanPer, int quality, double[] habitatPer, double grazing, double seeds, double foilage)
    {
        // get year's crop array
        int[] gameArray = getGameAnimals(oceanPer, quality, habitatPer, grazing, seeds, foilage);
        return CreateGameArrayPrintString(gameArray);
    }


    // Create the string to print
    private string CreateGameArrayPrintString(int[] gameArray)
    {
        string gameString = "";
        for (int i = 0; i < NUM_OF_GAME_ANIMALS; i++)
        {
            if (i == NUM_OF_HERBIVORES)
            {
                gameString += "\n";
            }
            if (gameArray[i] != 0)
            {
                gameString += SwitchName(i) + ": " + gameArray[i] + " / ";
            }
        }

        return gameString;
    }


    // Return the game animals array
    public int[] getGameAnimals(double oceanPer, int quality, double[] habitatPer, double grazing, double seeds, double foilage)
    {
        // ROUNDDOWN(((foodAvailable/120)/foodRequired)*abundancePer*SumOfHabitatPer*(-(50-quality)/200+1), 0)
        int[] gameArray = new int[NUM_OF_GAME_ANIMALS];
        if (oceanPer != 1.0)
        {
            // Calculate the abundance of the herbivores
            double meatAvailable = 0.0;
            for (int i = 0; i < NUM_OF_HERBIVORES; i++)
            {
                SwitchVariables(i, habitatPer, grazing, seeds, foilage);
                double calculation = ((foodAvail / 120.0) / foodConsumed);
                calculation *= abundance * habitatSum;
                calculation *= (1 - (50 - quality) / 200.0);
                int animals = (int) calculation;
                gameArray[i] = animals;
                // Calculate the available game meat
                meatAvailable += animals * meatValue;
            }

            // Calculate the number of predators
            for (int i = NUM_OF_HERBIVORES; i < NUM_OF_GAME_ANIMALS; i++)
            {
                SwitchVariables(i, habitatPer, grazing, seeds, foilage);
                double calculation = ((meatAvailable / 120.0) / foodConsumed);
                calculation *= abundance * habitatSum;
                calculation *= (1 - (50 - quality) / 200.0);
                gameArray[i] = (int) Math.Truncate(calculation);
            }
        }

        return gameArray;
    }


    // Game Variable switch statement
    // Rabbit  Lizard Chicken Songbird Rat Frog Fox Cat Falcon
    private void SwitchVariables(int gameNum, double[] habitatPer, double grazing, double seeds, double foilage)
    {
        switch (gameNum)
        {
            // Squirrel
            case 0:
                // Squirrels live in all types of forests and swamps
                habitatSum = habitatPer[3] + habitatPer[4] + habitatPer[7] + habitatPer[8] + habitatPer[11] + habitatPer[12];
                foodAvail = seeds; // Squirrels eat only Seeds                
                foodConsumed = 0.004116667;
                abundance = .125;
                meatValue = 0.290635457;
                break;
            // Rabbit
            case 1:
                // Rabbits live in all types of deserts and plains
                habitatSum = habitatPer[1] + habitatPer[2] + habitatPer[5] + habitatPer[6] + habitatPer[9] + habitatPer[10];
                foodAvail = grazing + foilage; // Rabbits eat only Grazing and Foilage                
                foodConsumed = 0.009056667;
                abundance = .15;
                meatValue = 0.617222352;
                break;
            // Lizard
            case 2:
                // Lizards live in temperate deserts and all hot environments
                habitatSum = habitatPer[5] + habitatPer[9] + habitatPer[10] + habitatPer[11] + habitatPer[12];
                foodAvail = seeds; // Lizards eat only Seeds               
                foodConsumed = 0.000933333;
                abundance = .075;
                meatValue = 0.268795798;
                break;
            // Chicken
            case 3:
                // Chicken live in temperate forests and plains
                habitatSum = habitatPer[6] + habitatPer[7];
                foodAvail = seeds; // Chickens eat only Seeds               
                foodConsumed = 0.0131;
                abundance = .05;
                meatValue = 1.088622983;
                break;
            // Songbird
            case 4:
                // Songbirds live everywhere
                habitatSum = 1.0;
                foodAvail = seeds; // Songbirds eat only Seeds               
                foodConsumed = 0.000636667;
                abundance = .075;
                meatValue = 0.022175653;
                break;
            // Rats
            case 5:
                // Rats live everywhere
                habitatSum = 1.0;
                foodAvail = seeds + foilage; // Rats eat only Seeds and foilage             
                foodConsumed = 0.002058333;
                abundance = .2;
                meatValue = 0.12599803;
                break;
            // Frogs
            case 6:
                // Frogs live in temperate and hot forests and swamps, but heavily favor swamps
                habitatSum = (habitatPer[7] + habitatPer[11]) / 4.0 + habitatPer[8] + habitatPer[12];
                foodAvail = foilage; // Frogs eat only Foilage             
                foodConsumed = .0000466667;
                abundance = .075;
                meatValue = 0.009197856;
                break;
            // Foxes
            case 7:
                // Foxes live in cold and temperature non-swampy habitats
                habitatSum = habitatPer[1] + habitatPer[2] + habitatPer[3] + habitatPer[5] + habitatPer[6] + habitatPer[7];
                foodAvail = 0.0; // Foxes eat only meat            
                foodConsumed = 0.1235;
                abundance = .125;
                meatValue = 8.803062391;
                break;
            // Cats
            case 8:
                // Cats live in hot and temperature non-desert habitats
                habitatSum = habitatPer[6] + habitatPer[7] + habitatPer[8] + habitatPer[10] + habitatPer[11] + habitatPer[12];
                foodAvail = 0.0; // Cats eat only meat            
                foodConsumed = 0.055575;
                abundance = .15;
                meatValue = 2.223494654;
                break;
            // Falcons
            case 9:
                // Falcons live everywhere
                habitatSum = 1.0;
                foodAvail = 0.0; // Falcons eat only meat            
                foodConsumed = 0.023875;
                abundance = .175;
                meatValue = 0.797987526;
                break;
        }
    }


    // Crop get name only statement
    private string SwitchName(int gameNum)
    {
        string name = "";
        switch (gameNum)
        {
            // Squirrel
            case 0:
                name = "squirrel";
                break;
            // Rabbit
            case 1:
                name = "rabbit";
                break;
            // Lizard
            case 2:
                name = "lizard";
                break;
            // Chicken
            case 3:
                name = "chicken";
                break;
            // Songbird
            case 4:
                name = "songbird";
                break;
            // Rat
            case 5:
                name = "rat";
                break;
            // Frog
            case 6:
                name = "frog";
                break;
            // Fox
            case 7:
                name = "fox";
                break;
            // Cat
            case 8:
                name = "cat";
                break;
            // Falcon
            case 9:
                name = "falcon";
                break;
        }

        return name;
    }

}
