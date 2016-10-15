using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RainfallTests {

    [Test]
    public void YearOfRainTest()
    {
        // Test the Storm Generation method
        HumidityLayer testEquation = new HumidityLayer("HumidityTests", 1);

        testEquation.GenerateWorldsYearOfRain();

        // Assert.AreEqual("TemperatureEquations", testEquation.layerName);
    }
}
