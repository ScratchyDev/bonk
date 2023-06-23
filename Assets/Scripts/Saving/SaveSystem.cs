using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
	public static void SavePlayer(SaveManager saves)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/player.gamer", FileMode.Create);
		PlayerData graph = new PlayerData(saves);
		binaryFormatter.Serialize(fileStream, graph);
		fileStream.Close();
	}

	public static PlayerData LoadPlayer()
	{
		string text = Application.persistentDataPath + "/player.gamer";
		if (File.Exists(text))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(text, FileMode.Open);
			PlayerData result = binaryFormatter.Deserialize(fileStream) as PlayerData;
			fileStream.Close();
			return result;
		}
		Debug.LogError("Save file not found in" + text);
		return null;
	}
}
