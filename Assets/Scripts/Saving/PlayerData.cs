using System;

[Serializable]
public class PlayerData
{
	public int completionLevel;

	public PlayerData(SaveManager saves)
	{
		completionLevel = saves.completionLevel;
	}
}
