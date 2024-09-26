using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private LoadingScene loadingScene;
    [SerializeField] private SoundMixerManager soundMixerManager;
   

    private Dictionary<Transform, AudioSource> audioSources = new Dictionary<Transform, AudioSource>();
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (UIHandler.instance.isPaused)
        {
            PauseAllAudioClips();
        }
        else
        {
            UnpauseAllAudioClips();
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool exit)
    {
        if (audioClip == null) { return; }
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
      
        if (exit)
        {
            StartCoroutine(HandleExit(clipLength));
        }
    }

    // Pause all active audio clips
    private void PauseAllAudioClips()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                pausedAudioSources.Add(audioSource);
            }
        }
    }

    private void UnpauseAllAudioClips()
    {
        foreach (AudioSource audioSource in pausedAudioSources)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }
        pausedAudioSources.Clear();
    }

    private IEnumerator HandleExit(float delay)
    {
        yield return new WaitForSeconds(delay);
        loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayLoopingSound(AudioClip audioClip, Transform transform, float volume)
    {
        if (!audioSources.ContainsKey(transform))
        {
            AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.transform.SetParent(transform);
            audioSource.spatialBlend = 1.0f;  // Set to 3D sound
            audioSource.maxDistance = 50.0f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.Play();

            audioSources[transform] = audioSource;
        }
        else
        {
            AudioSource audioSource = audioSources[transform];
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    public void StopLoopingSound(Transform transform)
    {
        if (audioSources.ContainsKey(transform))
        {
            AudioSource audioSource = audioSources[transform];
            if (audioSource.isPlaying)
            {
                Destroy(audioSource.gameObject);
            }
            audioSources.Remove(transform);
        }
    }
}
