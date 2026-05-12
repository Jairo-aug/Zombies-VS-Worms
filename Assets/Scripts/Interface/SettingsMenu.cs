using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        audioMixer.SetFloat("GeneralVolume", audioMixer.GetFloat("GeneralVolume"));
        audioMixer.SetFloat("MusicVolume", audioMixer.GetFloat("MusicVolume"));
        audioMixer.SetFloat("EffectsVolume", audioMixer.GetFloat("EffectsVolume"));
    }


    public void SetGeneralVolume (float volume) {
        audioMixer.SetFloat("GeneralVolume", ConvertToDecibels(volume));
    }

    public void SetMusicVolume (float volume) {
        audioMixer.SetFloat("MusicVolume", ConvertToDecibels(volume));
    }
    public void SetEffectsVolume (float volume) {
        audioMixer.SetFloat("EffectsVolume", ConvertToDecibels(volume));
    }

    private float ConvertToDecibels(float sliderValue) => Mathf.Log10(sliderValue) * 20 + 10f;

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
