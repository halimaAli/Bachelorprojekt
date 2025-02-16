using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private int currentLevel = 1;
    private Vector3 lastPosition;
    private string selectedProfileId = "";
    private string username = "Nexus";
    private bool resetSaveOnNewLevel;

    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool disableDataPersistenceInEditor = false;

    private void Awake()
    {
        if (disableDataPersistenceInEditor)
        {
            Debug.Log("Data Persistence Manager is disabled in the editor.");
            return;
        }

        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of DataPersistenceManager found.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, "SaveSlot");

    }

    private void OnEnable()
    {
        if (disableDataPersistenceInEditor)
        {
            Debug.Log("Data Persistence Manager is disabled in the editor.");
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (disableDataPersistenceInEditor)
        {
            Debug.Log("Data Persistence Manager is disabled in the editor.");
            return;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resetSaveOnNewLevel = false;
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>().ToList();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load(selectedProfileId);

        if (gameData == null)
        {
            Debug.Log("No data found. Starting a new game.");
            NewGame(); 
        }

        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        lastPosition = gameData.spawnPoint;
    }

    public void SaveGame()
    {
        PlayerPrefs.Save();
        if (gameData == null)
        {
            Debug.LogWarning("No game data found. Cannot save.");
            return;
        }

        if (!resetSaveOnNewLevel)
        {
            foreach (var dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(gameData);
            }
        }


        gameData.currentSceneIndex = currentLevel;
        gameData.lastUpdated = DateTime.Now.ToBinary();
        gameData.spawnPoint = lastPosition;

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
        LoadGame();
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void ResetPlayerToDefault()
    {
        if (gameData != null)
        {
            gameData.ResetGameData();
        }
        
        lastPosition = gameData.spawnPoint;
        resetSaveOnNewLevel = true;
    }

    public void SetLastPosition(Vector3 position)
    {
        lastPosition = position;
    }

    public void SetUsername(string username)
    {
        if (!username.Equals("")) gameData.username = username;
    }
}
