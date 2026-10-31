using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public int completionLevel;

	// Best clear time in seconds per level number (0 = not cleared yet) and the best survival wave.
	[NonSerialized] public float[] bestTimes = new float[0];
	[NonSerialized] public int bestWave;

	public void LoadPlayer()
	{
		PlayerData playerData = SaveSystem.LoadPlayer();

		// There is no save file on the very first launch. This used to throw a NullReferenceException.
		if (playerData == null)
		{
			return;
		}

		completionLevel = playerData.completionLevel;
		bestTimes = playerData.bestTimes != null ? playerData.bestTimes : new float[0];
		bestWave = playerData.bestWave;
	}

	private void Start()
	{
		LoadPlayer();
	}

	public void SavePlayer()
	{
		SaveSystem.SavePlayer(this);
	}

	public float GetBestTime(int level)
	{
		if (bestTimes == null || level < 0 || level >= bestTimes.Length)
		{
			return 0f;
		}
		return bestTimes[level];
	}

	/// Stores the time if it is the first clear or faster than the old record. Returns true when it was stored.
	public bool RecordLevelTime(int level, float seconds)
	{
		if (level < 0 || seconds <= 0f)
		{
			return false;
		}

		if (bestTimes == null)
		{
			bestTimes = new float[0];
		}
		if (level >= bestTimes.Length)
		{
			Array.Resize(ref bestTimes, level + 1);
		}

		float old = bestTimes[level];
		if (old > 0f && old <= seconds)
		{
			return false;
		}

		bestTimes[level] = seconds;
		return true;
	}

	public void RecordWave(int wavesCleared)
	{
		if (wavesCleared > bestWave)
		{
			bestWave = wavesCleared;
		}
	}
}
