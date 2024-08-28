using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [Header("UI Elements")]
    [SerializeField] private Text amountOfCoinsText;
    [SerializeField] private Text healthPointsText;

    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private GameObject mainMenu;

   [Header("Pause Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip confirmSoundFx;
    [SerializeField] private AudioClip changeSoundFx;

    [SerializeField] private LoadingScene sceneLoader;

   private bool isPaused;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        isPaused = false;
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            // TO DO exchange the save slots with loaded save slots
        }
    }

    private void Update()
    {
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

    public void InitializeUI(int coins, int health)
    {
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
        DataPersistenceManager.Instance.SaveGame();
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

    public void UpdateCoins(int coins)
    {
        amountOfCoinsText.text = coins.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthPointsText.text = health.ToString();
    }

    public void PlayConfirmSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(confirmSoundFx, transform, 1, false);
    }

    public void PlayChangeSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(changeSoundFx, transform, 1, false);
    }

    public void ActivateMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        mainMenu.gameObject.SetActive(false);
    }
}
