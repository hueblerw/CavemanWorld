using NUnit.Framework;
using System.Diagnostics;

public class TimingTests {

	[Test]
	public void WorldCreationTime()
	{
        World testWorld = new World(50, 50, false);
	}

    [Test]
    public void NewYearTime()
    {
        World testWorld = new World(50, 50, false);
        testWorld.NewYear();
    }

    [Test]
    public void RandomWorldCreationTime()
    {
        World testWorld = new World(50, 50, true);
    }

    [Test]
    public void RandomDecentSizeWorld()
    {
        World testWorld = new World(100, 100, true);
    }

    [Test]
    public void DecentSizeNewYearTime()
    {
        World testWorld = new World(100, 100, false);
        testWorld.NewYear();
    }

    [Test]
    public void RandomLargeWorldCreationTime()
    {
        World testWorld = new World(200, 200, true);
    }

}
