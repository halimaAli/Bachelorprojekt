using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [SerializeField] private Text amountOfCoins;
    private int coins;

    [SerializeField] private Text healthpoints;

    [SerializeField] private GameObject _3DTutorialText;
    [SerializeField] private GameObject _2DTutorialText;
    [SerializeField] private GameObject pausedScreen;

    private bool paused;

    private void Awake()
    {
        if (instance == null) instance = this;

        coins = 0;
        paused = false;
        /*  _3DTutorialText.SetActive(false);
          _2DTutorialText.SetActive(true);*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        pausedScreen.SetActive(true);

    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
        pausedScreen.SetActive(false);
    }

    public void OnExitClick()
    { 
        SceneManager.LoadScene("Main Menu");
    }

    public void OnCoinCollected()
    {
        coins += 1;
        amountOfCoins.text = coins.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthpoints.text = health.ToString();
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnLoadGame()
    {
        print("Load Game");
    }

    public void OnOptions()
    {
        print("Options");
    }
}
