using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NewTester {

	[Test]
	public void SVLayerTest()
	{
        // Test all the SingleLayer Values can be initialized
        SingleValueLayer elevation = new SingleValueLayer("Elevation", "Semi-static");

        Assert.AreEqual("Elevation", elevation.layerName);
        Assert.AreEqual("Semi-static", elevation.getType());
        Assert.AreEqual(50, SingleValueLayer.WORLDX);
        Assert.AreEqual(50, SingleValueLayer.WORLDZ);
    }
}
