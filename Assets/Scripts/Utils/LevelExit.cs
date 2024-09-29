using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool enterAllowed;
    public string nextLevel;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private LoadingScene loadingScene;
    [SerializeField] private AudioClip exitSoundClip;
    private bool entered;

    private void Start()
    {
        if (toolTip != null)
        {
            toolTip.SetActive(false);
        }
        if (loadingScene != null) { 
        }
    }

    private void Update()
    {
        if (enterAllowed && Input.GetKeyDown(KeyCode.Return) && !entered)
        {
            PlayerController.instance.active = false;
            entered = true;
            DataPersistenceManager.Instance.ResetPlayerToDefault();
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                BossLevelManager.instance.ShowGameWonScreen();
            }
            else if (exitSoundClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(exitSoundClip, transform, 1, true);
            } 
            else
            {   
                loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (toolTip != null)
            {
                toolTip.SetActive(true);
            }
            enterAllowed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (toolTip != null)
            {
                toolTip.SetActive(false);
            }
            enterAllowed = true;
        }
    }
}
