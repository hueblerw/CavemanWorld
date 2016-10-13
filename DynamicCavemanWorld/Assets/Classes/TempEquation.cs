﻿using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

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

        // convert to more useful numbers
        float fallMidpt = 120.0f - Midpt;
        double midptRatio = Midpt / 120;
        double fallMidptRatio = fallMidpt / 120;

        // Create the matirces
        string solutionMatrix = "0.0\r\n0.0\r\n0.0\r\n" + (0.25 - midptRatio) + "\r\n" + (0.75 - fallMidptRatio);
        string systemMatrix = create5x5Matrix(midptRatio, fallMidptRatio);
        
            
        LightweightMatrixCSharp.Matrix MA = LightweightMatrixCSharp.Matrix.Parse(systemMatrix);
        LightweightMatrixCSharp.Matrix MB = LightweightMatrixCSharp.Matrix.Parse(solutionMatrix);
        
        // Solve the system
        LightweightMatrixCSharp.Matrix solutions = MA.SolveWith(MB);
            //if (highTemp == 100)
            //{
            //    Debug.Log(solutionMatrix);
            //    Debug.Log(systemMatrix);
            //    Debug.Log(solutions.ToString());
            //}
        // Extract the answers for A-E
        string[] solutionArray = Regex.Split(solutions.ToString(), "\r\n");
            // Debug.Log(solutions.ToString());
        extractSolutions(solutionArray);

    }

    // Return an array of random temperatures for the year
    public IntDayList generateYearsTemps()
    {
        int[] temps = new int[120];
        System.Random randy = new System.Random();
        for (int day = 1; day <= 120; day++)
        {
            temps[day - 1] = generateTodaysTemp(day, randy);
        }
        IntDayList dayList = new IntDayList(temps);
        return dayList;
    }

    // For Test purposes: return the coefficents as an array
    public double[] returnConstants()
    {
        double[] array = new double[6];
        array[0] = this.A;
        array[1] = this.B;
        array[2] = this.C;
        array[3] = this.D;
        array[4] = this.E;
        array[5] = this.F;
        return array;
    }

    // Private methods
    private int generateTodaysTemp(int day, System.Random rnd)
    {
        double dM = day / 120.0;
        double fakeDay = 120.0 * (this.A * (Math.Pow(dM, 6.0) / 6.0) + this.B * (Math.Pow(dM, 5.0) / 5.0) + this.C * (Math.Pow(dM, 4.0) / 4.0) + this.D * (Math.Pow(dM, 3.0) / 3.0) + this.E * (Math.Pow(dM, 2.0) / 2.0) + this.F * dM);
        fakeDay = -Math.Cos((fakeDay / 120.0) * (2.0 * Math.PI));
        double temp = ((this.avgHigh - this.avgLow)/ 2.0) * fakeDay;
        temp = temp + ((this.avgHigh - this.avgLow) / 2.0) + this.avgLow;
        double randy = rnd.Next((int) - this.variance * 10, (int) this.variance * 10 + 1) / 10;
        return (int) Math.Round(temp + randy, 0);
    }

    private string create5x5Matrix(double midptRatio, double fallMidptRatio)
    {
        string systemMatrix = "1.0 1.0 1.0 1.0 1.0\r\n";
        systemMatrix += (1.0/6.0) + " 0.2 0.25 " + (1.0/3.0) + " 0.5\r\n";
        systemMatrix += Math.Pow(.5, 6.0) / 6.0 + " " + Math.Pow(.5, 5.0) / 5.0 + " " + Math.Pow(.5, 4.0) / 4.0 + " " + Math.Pow(.5, 3.0) / 3.0 + " " + Math.Pow(.5, 2.0) / 2.0 + "\r\n";
        systemMatrix += Math.Pow(midptRatio, 6.0) / 6.0 + " " + Math.Pow(midptRatio, 5.0) / 5.0 + " " + Math.Pow(midptRatio, 4.0) / 4.0 + " " + Math.Pow(midptRatio, 3.0) / 3.0 + " " + Math.Pow(midptRatio, 2.0) / 2.0 + "\r\n";
        systemMatrix += Math.Pow(fallMidptRatio, 6.0) / 6.0 + " " + Math.Pow(fallMidptRatio, 5.0) / 5.0 + " " + Math.Pow(fallMidptRatio, 4.0) / 4.0 + " " + Math.Pow(fallMidptRatio, 3.0) / 3.0 + " " + Math.Pow(fallMidptRatio, 2.0) / 2.0;
        return systemMatrix;
    }

    private void extractSolutions(string[] solutionArray)
    {
        this.A = Convert.ToDouble(solutionArray[0]);
        this.B = Convert.ToDouble(solutionArray[1]);
        this.C = Convert.ToDouble(solutionArray[2]);
        this.D = Convert.ToDouble(solutionArray[3]);
        this.E = Convert.ToDouble(solutionArray[4]);
    }

}
