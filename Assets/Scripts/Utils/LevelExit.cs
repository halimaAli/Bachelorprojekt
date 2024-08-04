using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool enterAllowed;
    public string nextLevel;
    [SerializeField] private GameObject toolTip;

    private void Start()
    {
        if (toolTip != null)
        {
            toolTip.SetActive(false);
        }
    }

    private void Update()
    {
        
        if (enterAllowed && Input.GetKeyDown(KeyCode.Return))
        {
            print("loading");
            if (string.IsNullOrEmpty(nextLevel))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else
            {
                SceneManager.LoadScene(nextLevel);
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
