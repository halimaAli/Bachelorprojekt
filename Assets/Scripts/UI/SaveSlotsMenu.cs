using System;
using System.Collections.Generic;
using UnityEngine;


public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private UIHandler mainMenu;
    [SerializeField] private GameObject nameEntryMenuObj;
    [SerializeField] private NameEntryMenu nameEntryMenu;

    private SaveSlot[] saveSlots;
    [SerializeField] private LoadingScene sceneLoader;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (saveSlot.newGame)
        {
            nameEntryMenuObj.SetActive(true);
            DeactivateMenu();
        } 
        else
        {
            sceneLoader.LoadScene(1);
        }
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        // loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}