﻿using NUnit.Framework;
using System.Diagnostics;

public class TimingTests {

	[Test]
	public void WorldCreationTime()
	{
        World testWorld = new World(50, 50);
	}

    [Test]
    public void NewYearTime()
    {
        World testWorld = new World(50, 50);
        testWorld.NewYear();
    }

}
