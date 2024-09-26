using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundFXVolumeSlider;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        LoadVolumeSettings();
    }

    public void SetMasterVolume(float level)
    {
        // translates logarithmic interpolation into a linear interpolation
        float volume = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("masterVolume", volume);
        
        // Save settings
        PlayerPrefs.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        float volume = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("soundFXVolume", volume);

        PlayerPrefs.SetFloat("soundFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        float volume = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("musicVolume", volume);

        PlayerPrefs.SetFloat("musicVolume", level);
    }

    public void LoadVolumeSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume", 1f);

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = masterVolume;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
        }

        if (soundFXVolumeSlider != null)
        {
            soundFXVolumeSlider.value = soundFXVolume;
        }

        // Convert linear to dB before setting the AudioMixer values
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20f);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(soundFXVolume) * 20f);
    }
}
