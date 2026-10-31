using System;
using System.IO;
using UnityEngine;
#if !BONK_DROP_LEGACY_SAVE
using System.Runtime.Serialization.Formatters.Binary;
#endif

/// Saves progress as readable JSON (player.json).
/// BinaryFormatter is deprecated because it is unsafe to load, so it is only kept to import an old
/// player.gamer file once. Add the scripting define BONK_DROP_LEGACY_SAVE to remove that import.
public static class SaveSystem
{
	static string JsonPath
	{
		get { return Path.Combine(Application.persistentDataPath, "player.json"); }
	}

#if !BONK_DROP_LEGACY_SAVE
	static string LegacyPath
	{
		get { return Application.persistentDataPath + "/player.gamer"; }
	}
#endif

	public static void SavePlayer(SaveManager saves)
	{
		try
		{
			string json = JsonUtility.ToJson(new PlayerData(saves), true);
			string temp = JsonPath + ".tmp";

			// write to a temp file first so a crash mid-save cannot leave a half written save behind
			File.WriteAllText(temp, json);
			if (File.Exists(JsonPath))
			{
				File.Delete(JsonPath);
			}
			File.Move(temp, JsonPath);
		}
		catch (Exception e)
		{
			Debug.LogError("Could not save the game: " + e.Message);
		}
	}

	public static PlayerData LoadPlayer()
	{
		try
		{
			string path = null;
			if (File.Exists(JsonPath))
			{
				path = JsonPath;
			}
			else if (File.Exists(JsonPath + ".tmp"))
			{
				path = JsonPath + ".tmp";
			}

			if (path != null)
			{
				PlayerData data = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
				if (data != null)
				{
					return data;
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning("Save file could not be read: " + e.Message);
		}

#if !BONK_DROP_LEGACY_SAVE
		return LoadLegacy();
#else
		return null;
#endif
	}

#if !BONK_DROP_LEGACY_SAVE
	static PlayerData LoadLegacy()
	{
		if (!File.Exists(LegacyPath))
		{
			return null;
		}

		try
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (FileStream fileStream = new FileStream(LegacyPath, FileMode.Open))
			{
				return binaryFormatter.Deserialize(fileStream) as PlayerData;
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning("Old save file could not be imported: " + e.Message);
			return null;
		}
	}
#endif
}
