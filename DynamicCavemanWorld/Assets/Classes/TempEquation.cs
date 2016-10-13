using UnityEngine;
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
        float fallMidpt = 120 - Midpt;
        double midptRatio = Midpt / 120;
        double fallMidptRatio = fallMidpt / 120;

        // Create the matirces
        string solutionMatrix = "0.0\r\n0.0\r\n0.0\r\n" + (0.25 - midptRatio) + "\r\n" + (0.75 - fallMidptRatio);
        string systemMatrix = create5x5Matrix(midptRatio, fallMidptRatio);
            // Debug.Log("5x5:" + systemMatrix);
            // Debug.Log("5x1:" + solutionMatrix);
        LightweightMatrixCSharp.Matrix MA = LightweightMatrixCSharp.Matrix.Parse(systemMatrix);
        LightweightMatrixCSharp.Matrix MB = LightweightMatrixCSharp.Matrix.Parse(solutionMatrix);

        // Solve the system
        LightweightMatrixCSharp.Matrix solutions = MA.SolveWith(MB);

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
        double dM = day / 120;
        double fakeDay = 120 * (this.A * (Math.Pow(dM, 6) / 6) + this.B * (Math.Pow(dM, 5) / 5) + this.C * (Math.Pow(dM, 4) / 4) + this.D * (Math.Pow(dM, 3) / 3) + this.E * (Math.Pow(dM, 2) / 2) + this.F * dM);
        fakeDay = -Math.Cos((fakeDay / 120) * (2 * Math.PI));
        double temp = ((this.avgHigh - this.avgLow)/ 2) * fakeDay;
        temp = temp + ((this.avgHigh - this.avgLow) / 2) + this.avgLow;
        double randy = rnd.Next((int) -this.variance * 10, (int) this.variance * 10 + 1) / 10;
        return (int) Math.Round(temp + randy, 0);
    }

    private string create5x5Matrix(double midptRatio, double fallMidptRatio)
    {
        string systemMatrix = "1.0 1.0 1.0 1.0 1.0\r\n";
        systemMatrix += "0.5 " + (1/3) + " 0.25 0.2 " + (1/6) + "\r\n";
        systemMatrix += Math.Pow(.5, 6) / 6 + " " + Math.Pow(.5, 5) / 5 + " " + Math.Pow(.5, 4) / 4 + " " + Math.Pow(.5, 3) / 3 + " " + Math.Pow(.5, 2) / 2 + "\r\n";
        systemMatrix += Math.Pow(midptRatio, 6) / 6 + " " + Math.Pow(midptRatio, 5) / 5 + " " + Math.Pow(midptRatio, 4) / 4 + " " + Math.Pow(midptRatio, 3) / 3 + " " + Math.Pow(midptRatio, 2) / 2 + "\r\n";
        systemMatrix += Math.Pow(fallMidptRatio, 6) / 6 + " " + Math.Pow(fallMidptRatio, 5) / 5 + " " + Math.Pow(fallMidptRatio, 4) / 4 + " " + Math.Pow(fallMidptRatio, 3) / 3 + " " + Math.Pow(fallMidptRatio, 2) / 2;
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
