using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;
	public Dropdown resolutionDropdown;
	public SaveManager saves;
	Resolution[] resolutions;

	int targetFramerate;
	int resolutionWidth;
	int resolutionHeight;
	int quality;
	int vSync;
	float mainVolume;
	bool fullscreen;

	void Start(){
		//Making resolutions array
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width + " x " + resolutions[i].height;
			list.Add(item);
			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		resolutionDropdown.AddOptions(list);
		resolutionDropdown.value = value;
		resolutionDropdown.RefreshShownValue();
	}

	public void SetTargetFramerate(string amount){
		targetFramerate = int.Parse(amount);
	}

	public void SetResolution(int index){
		Resolution resolution = resolutions[index];
		resolutionWidth = resolution.width;
		resolutionHeight = resolution.height;
	}

	public void SetQuality(int level){
		quality = level;
	}

	public void Vsync(int level){
		vSync = level;
	}

	public void SetVolume(float amount){
		mainVolume = amount;
	}

	public void SetFullscreen(bool toggle){
		fullscreen = toggle;
	}

	public void Apply(){
		Application.targetFrameRate = targetFramerate;
		Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
		QualitySettings.SetQualityLevel(quality);
		QualitySettings.vSyncCount = vSync;
		audioMixer.SetFloat("Main", mainVolume);
		Screen.fullScreen = fullscreen;
	}
}
