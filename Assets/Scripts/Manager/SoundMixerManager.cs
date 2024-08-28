using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour, IDataPersistence
{
    public static SoundMixerManager Instance;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundFXVolumeSlider;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f); // translates logarithmic interpolation into a linear interpolation

    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void LoadData(GameData data)
    {
        float masterVolume = data.volumeSettings.GetValueOrDefault("masterVolume", 0.001f);
        float musicVolume = data.volumeSettings.GetValueOrDefault("musicVolume", 0.001f);
        float soundFXVolume = data.volumeSettings.GetValueOrDefault("soundFXVolume", 0.001f);

        audioMixer.SetFloat("masterVolume", masterVolume);
        audioMixer.SetFloat("musicVolume", musicVolume);
        audioMixer.SetFloat("soundFXVolume", soundFXVolume);

        // convertd the dB value back to linear value and set the sliders
        masterVolumeSlider.value = Mathf.Pow(10f, masterVolume / 20f);
        musicVolumeSlider.value = Mathf.Pow(10f, musicVolume / 20f);
        soundFXVolumeSlider.value = Mathf.Pow(10f, soundFXVolume / 20f);
    }

    public void SaveData(GameData data)
    {
        data.volumeSettings["masterVolume"] = GetVolume("masterVolume");
        data.volumeSettings["musicVolume"] = GetVolume("musicVolume");
        data.volumeSettings["soundFXVolume"] = GetVolume("soundFXVolume");
    }

    private float GetVolume(string volumeName)
    {
        float value;
        bool result = audioMixer.GetFloat(volumeName, out value);
        return result ? value : 0.001f;
    }
}
