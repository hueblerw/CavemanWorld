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
    public int[] generateYearsTemps()
    {
        int[] temps = new int[120];
        for (int day = 1; day <= 120; day++)
        {
            temps[day - 1] = generateTodaysTemp(day);
        }
        return temps;
    }

    // Private methods
    private int generateTodaysTemp(int day)
    {
        double dM = day / 120;
        double fakeDay = 120 * (this.A * (Math.Pow(dM, 6) / 6) + this.B * (Math.Pow(dM, 5) / 5) + this.C * (Math.Pow(dM, 4) / 4) + this.D * (Math.Pow(dM, 3) / 3) + this.E * (Math.Pow(dM, 2) / 2) + this.F * dM);
        fakeDay = -Math.Cos((fakeDay / 120) * (2 * Math.PI));
        double temp = ((this.avgHigh - this.avgLow)/ 2) * fakeDay;
        temp = temp + ((this.avgHigh - this.avgLow) / 2) + this.avgLow;
        System.Random rnd = new System.Random();
        double randy = rnd.Next((int) -this.variance * 10, (int) this.variance * 10 + 1) / 10;
        return (int) Math.Round(temp + randy, 0);
    }

}
