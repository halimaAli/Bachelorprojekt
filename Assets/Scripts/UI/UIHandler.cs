using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text amountOfCoinsText;
    [SerializeField] private TMP_Text healthPointsText;
   

    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private GameObject mainMenu;

    [Header("Pause Menus")]
    [SerializeField] private bool cantPause;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip confirmSoundFx;
    [SerializeField] private AudioClip changeSoundFx;
    [SerializeField] private AudioClip saveSlotSoundFx;

    [SerializeField] private LoadingScene sceneLoader;

    internal bool isPaused;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            return;
        }

        if (cantPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void InitializeUI(string username, int coins, int health)
    {
        UpdateUsername(username);
        UpdateHealth(health);
        UpdateCoins(coins);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
       if (controlsMenu !=  null) controlsMenu.SetActive(false);
    }

    public void OnSaveSlotMenu()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnQuit()
    {
        Time.timeScale = 1;
        isPaused = false;
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.SaveGame();
        }
        SceneManager.LoadScene("Main Menu");
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateUsername(string username)
    {
        if (usernameText != null)
        {
            usernameText.text = username;
        }
    }

    public void UpdateCoins(int coins)
    {
        if (amountOfCoinsText != null)
        {
            amountOfCoinsText.text = coins.ToString();
        }
    }

    public void UpdateHealth(int health)
    {
        if (healthPointsText != null)
        {
            healthPointsText.text = health.ToString();
        }
    }

    public void PlayConfirmSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(confirmSoundFx, transform, 1, false);
    }

    public void PlayChangeSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(changeSoundFx, transform, 1, false);
    }

    public void PlaySaveSlotSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(saveSlotSoundFx, transform, 1, false);
    }

    public void ActivateMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        mainMenu.gameObject.SetActive(false);
    }

    public void RestartLevel(int level)
    {
        sceneLoader.LoadScene(level);
    }
}
