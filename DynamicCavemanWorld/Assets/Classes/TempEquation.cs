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
        double midptRatio = Midpt / 120;
        double fallMidptRatio = fallMidpt / 120;
        double[,] solutionArray = { { 0.0 },
                                    { 0.0 },
                                    { 0.0 },
                                    { 0.25 - midptRatio },
                                    { 0.75 - fallMidptRatio } };
        double[,] systemArray = {{ 1.0, 1.0, 1.0, 1.0, 1.0 },
                        { 0.5, 1/3, 0.25, 0.2, 1/6 },
                        { Math.Pow(.5, 6) / 6, Math.Pow(.5, 5) / 5, Math.Pow(.5, 4) / 4, Math.Pow(.5, 3) / 3, Math.Pow(.5, 2) / 2 },
                        { Math.Pow(midptRatio, 6) / 6, Math.Pow(midptRatio, 5) / 5, Math.Pow(midptRatio, 4) / 4, Math.Pow(midptRatio, 3) / 3, Math.Pow(midptRatio, 2) / 2 },
                        { Math.Pow(fallMidptRatio, 6) / 6, Math.Pow(fallMidptRatio, 5) / 5, Math.Pow(fallMidptRatio, 4) / 4, Math.Pow(fallMidptRatio, 3) / 3, Math.Pow(fallMidptRatio, 2) / 2 }};
        Matrix<double> A = Matrix<double>.Build.DenseOfArray(systemArray);
        Matrix<double> B = Matrix<double>.Build.DenseOfArray(solutionArray);
        // Then solve matrix A's inverse
        Matrix<double> inverse = A.Inverse();
        // Then multiply matrix A-1 x B
        Matrix<double> solutions = inverse * B;
        // Extract the answers for A-E
        Debug.Log(solutions);
        // solutions.ToColumnArrays();

    }

    // Return an array of random temperatures for the year

    // Private methods



}
