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
        if (DataPersistenceManager.Instance != null)
        {
            if (sceneId > 0 && sceneId != 5)
            {
                if (sceneId == 4) //When Boss Level gets Loaded, always reset Game Data except of coins
                {
                    DataPersistenceManager.Instance.ResetPlayerToDefault();
                }
                DataPersistenceManager.Instance.SetCurrentLevel(sceneId);
            }
            DataPersistenceManager.Instance.SaveGame();
        }
       

        StartCoroutine(LoadSceneAsync(sceneId));

        SoundFXManager.instance.PlaySoundFXClip(loadingSoundClip, transform, 1, false);
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(5);
       
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

                fill.fillAmount = progressValue;
                yield return null;
           
            }
        } 
        else
        {
            SceneManager.LoadScene(sceneId);
        }
    }
}


