using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using System;

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
        float fallMidpt = 120 - Midpt;
        double[,] systemArray = {{ 1.0, 1.0, 1.0, 1.0, 1.0 },
                        { 0.5, 1/3, 0.25, 0.2, 1/6 },
                        { Math.Pow(.5, 6) / 6, Math.Pow(.5, 5) / 5, Math.Pow(.5, 4) / 4, Math.Pow(.5, 3) / 3, Math.Pow(.5, 2) / 2 },
                        { Math.Pow(Midpt / 120, 6) / 6, Math.Pow(Midpt / 120, 5) / 5, Math.Pow(Midpt / 120, 4) / 4, Math.Pow(Midpt / 120, 3) / 3, Math.Pow(Midpt / 120, 2) / 2 },
                        { Math.Pow(fallMidpt / 120, 6) / 6, Math.Pow(fallMidpt / 120, 5) / 5, Math.Pow(fallMidpt / 120, 4) / 4, Math.Pow(fallMidpt / 120, 3) / 3, Math.Pow(fallMidpt / 120, 2) / 2 }};
        Matrix<double> A = Matrix<double>.Build.DenseOfArray(systemArray);
        // Then solve matrix A's inverse

        // Then multiply matrix A-1 x B

        // Extract the answers for A-E

    }

    // Return an array of random temperatures for the year

    // Private methods



}
