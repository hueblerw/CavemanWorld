using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NewTester {

	[Test]
	public void SVLayerTest()
	{
        // Test all the SingleLayer Values can be initialized
        SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static", 1);

        Assert.AreEqual("Elevation", elevation.layerName);
        Assert.AreEqual("Semi-static", elevation.getType());
        Assert.AreEqual(1, elevation.getRounding());
        Assert.AreEqual(50, SingleValueLayer.WORLDX);
        Assert.AreEqual(50, SingleValueLayer.WORLDZ);
    }

    [Test]
    public void DailyLayerTest()
    {
        // Test all the SingleLayer Values can be initialized
        DailyLayer rainfall = new DailyLayer("Rainfall", "Daily", 1);

        Assert.AreEqual("Elevation", rainfall.layerName);
        Assert.AreEqual("Daily", rainfall.getType());
        Assert.AreEqual(1, rainfall.getRounding());
        Assert.AreEqual(50, DailyLayer.WORLDX);
        Assert.AreEqual(50, DailyLayer.WORLDZ);
    }
}
