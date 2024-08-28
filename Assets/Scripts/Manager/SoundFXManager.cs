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

    private Dictionary<Transform, AudioSource> loopAudioSources = new Dictionary<Transform, AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool exit)
    {
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

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, bool exit)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

        if (exit)
        {
            StartCoroutine(HandleExit(clipLength));
        }
    }

    private IEnumerator HandleExit(float delay)
    {
        soundMixerManager.SetMusicVolume(0.001f);
        yield return new WaitForSeconds(delay);
        loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        soundMixerManager.SetMusicVolume(1f);
    }

    public void PlayLoopingSound(AudioClip audioClip, Transform transform, float volume)
    {
        if (!loopAudioSources.ContainsKey(transform))
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

            loopAudioSources[transform] = audioSource;
        }
        else
        {
            AudioSource audioSource = loopAudioSources[transform];
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    public void StopLoopingSound(Transform transform)
    {
        if (loopAudioSources.ContainsKey(transform))
        {
            AudioSource audioSource = loopAudioSources[transform];
            if (audioSource.isPlaying)
            {
                Destroy(audioSource.gameObject);
            }
            loopAudioSources.Remove(transform);
        }
    }
}
