using UnityEngine;
using System.Collections;

public class TempEquation {

    // Variables
    private double A;
    private double B;
    private double C;
    private double D;
    private double E;
    private double F = 1.0;
    private int avgHigh;
    private int avgLow;
    private double variance;

    // Constructor: takes in the input from the variable semi-static temperature references
    public TempEquation(int highTemp, int lowTemp, float Midpt, float variance)
    {

        // First store the variables that are used only when temperatures are read out.
        this.avgHigh = highTemp;
        this.avgLow = lowTemp;
        this.variance = variance;

        // Then create a matrix using the midpt input

        // Then solve matrix A's inverse

        // Then multiply matrix A-1 x B

        // Extract the answers for A-E

    }

    // Return an array of random temperatures for the year

    // Private methods



}
