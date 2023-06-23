using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public int completionLevel;

	public void LoadPlayer()
	{
		PlayerData playerData = SaveSystem.LoadPlayer();
	}

	private void Start()
	{
		LoadPlayer();
	}

	private void Update()
	{
		//Update variables internally here
	}

	public void SavePlayer()
	{
		SaveSystem.SavePlayer(this);
	}
}
