using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image fill;
    [SerializeField] private AudioClip loadingSoundClip;

    public void LoadScene(int sceneId)
    {
        if (sceneId == 1 || sceneId == 2 || sceneId == 3)
        {
            DataPersistenceManager.Instance.SetCurrentLevel(sceneId);
            //DataPersistenceManager.Instance.SetPositionToDefault();
        }
        DataPersistenceManager.Instance.SaveGame();

        StartCoroutine(LoadSceneAsync(sceneId));
        SoundFXManager.instance.PlaySoundFXClip(loadingSoundClip, transform, 1, false);
        SoundMixerManager.Instance.SetMusicVolume(0.001f);
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            fill.fillAmount = progressValue;
            print(operation.progress);
            yield return null;
           
        }

        if (operation.isDone)
        {
            SoundMixerManager.Instance.SetMusicVolume(1f);
        }
    }
}


