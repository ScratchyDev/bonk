using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;
	public Dropdown resolutionDropdown;
	public SaveManager saves;

	[Header("Optional: lets the menu show the saved values")]
	public Slider volumeSlider;
	public Toggle fullscreenToggle;
	public Dropdown qualityDropdown;
	public Dropdown vsyncDropdown;

	Resolution[] resolutions;

	int targetFramerate;
	int resolutionWidth;
	int resolutionHeight;
	int quality;
	int vSync;
	float mainVolume;
	bool fullscreen;

	const string PrefsKey = "bonk.settings";

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

		// Start from what the game is really using. The fields used to start at 0 / false, so pressing
		// Apply without touching everything switched to a 0x0 window, windowed mode and quality 0.
		resolutionWidth = Screen.currentResolution.width;
		resolutionHeight = Screen.currentResolution.height;
		quality = QualitySettings.GetQualityLevel();
		vSync = QualitySettings.vSyncCount;
		fullscreen = Screen.fullScreen;
		targetFramerate = Application.targetFrameRate;
		if (audioMixer == null || !audioMixer.GetFloat("Main", out mainVolume))
		{
			mainVolume = 0f;
		}

		LoadSaved();
		RefreshControls();
	}

	public void SetTargetFramerate(string amount){
		int parsed;
		if (int.TryParse(amount, out parsed))
		{
			targetFramerate = parsed;
		}
	}

	public void SetResolution(int index){
		if (resolutions == null || index < 0 || index >= resolutions.Length)
		{
			return;
		}
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
		ApplyValues();
		Save();
	}

	void ApplyValues(){
		Application.targetFrameRate = targetFramerate > 0 ? targetFramerate : -1;
		if (resolutionWidth > 0 && resolutionHeight > 0)
		{
			Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen);
		}
		QualitySettings.SetQualityLevel(quality);
		QualitySettings.vSyncCount = vSync;
		if (audioMixer != null)
		{
			audioMixer.SetFloat("Main", mainVolume);
		}
		Screen.fullScreen = fullscreen;
	}

	void Save(){
		PlayerPrefs.SetInt(PrefsKey, 1);
		PlayerPrefs.SetInt(PrefsKey + ".fps", targetFramerate);
		PlayerPrefs.SetInt(PrefsKey + ".width", resolutionWidth);
		PlayerPrefs.SetInt(PrefsKey + ".height", resolutionHeight);
		PlayerPrefs.SetInt(PrefsKey + ".quality", quality);
		PlayerPrefs.SetInt(PrefsKey + ".vsync", vSync);
		PlayerPrefs.SetFloat(PrefsKey + ".volume", mainVolume);
		PlayerPrefs.SetInt(PrefsKey + ".fullscreen", fullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	void LoadSaved(){
		if (!PlayerPrefs.HasKey(PrefsKey))
		{
			return;
		}

		targetFramerate = PlayerPrefs.GetInt(PrefsKey + ".fps", targetFramerate);
		resolutionWidth = PlayerPrefs.GetInt(PrefsKey + ".width", resolutionWidth);
		resolutionHeight = PlayerPrefs.GetInt(PrefsKey + ".height", resolutionHeight);
		quality = PlayerPrefs.GetInt(PrefsKey + ".quality", quality);
		vSync = PlayerPrefs.GetInt(PrefsKey + ".vsync", vSync);
		mainVolume = PlayerPrefs.GetFloat(PrefsKey + ".volume", mainVolume);
		fullscreen = PlayerPrefs.GetInt(PrefsKey + ".fullscreen", fullscreen ? 1 : 0) == 1;

		ApplyValues();
	}

	void RefreshControls(){
		if (volumeSlider != null) volumeSlider.SetValueWithoutNotify(mainVolume);
		if (fullscreenToggle != null) fullscreenToggle.SetIsOnWithoutNotify(fullscreen);
		if (qualityDropdown != null) qualityDropdown.SetValueWithoutNotify(quality);
		if (vsyncDropdown != null) vsyncDropdown.SetValueWithoutNotify(vSync);

		if (resolutions != null)
		{
			for (int i = 0; i < resolutions.Length; i++)
			{
				if (resolutions[i].width == resolutionWidth && resolutions[i].height == resolutionHeight)
				{
					resolutionDropdown.SetValueWithoutNotify(i);
					resolutionDropdown.RefreshShownValue();
					break;
				}
			}
		}
	}
}
