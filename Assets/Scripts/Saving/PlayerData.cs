using System;
using System.Runtime.Serialization;

[Serializable]
public class PlayerData
{
	public int completionLevel;

	// [OptionalField] keeps save files from older versions of the game loadable
	[OptionalField] public float[] bestTimes;
	[OptionalField] public int bestWave;

	public PlayerData()
	{
	}

	public PlayerData(SaveManager saves)
	{
		completionLevel = saves.completionLevel;
		bestTimes = saves.bestTimes;
		bestWave = saves.bestWave;
	}
}
