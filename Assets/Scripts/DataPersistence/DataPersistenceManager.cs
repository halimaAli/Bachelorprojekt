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

    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of DataPersistenceManager found.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, "SaveSlot");

        InitializeSelectedProfileId();
    }

    private void InitializeSelectedProfileId()
    {
        selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
        if (gameData == null)
        {
            Debug.LogWarning("No game data found. Cannot save.");
            return;
        }

        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
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

    public void SetPositionToDefault()
    {
        lastPosition = Vector3.zero;
    }

    public void SetUsername(string username)
    {
        if (!username.Equals("")) gameData.username = username;
    }
}
