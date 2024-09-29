using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataSlot;
    [SerializeField] private GameObject hasDataSlot;
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text healthPoints;

    [Header("Prefabs")]
    [SerializeField] private GameObject introSlotPrefab;
    [SerializeField] private GameObject tutorialSlotPrefab;
    [SerializeField] private GameObject level1SlotPrefab;
    [SerializeField] private GameObject boss1SlotPrefab;

    public int currentLevel;
    private Button saveSlotButton;
    public bool newGame; 

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            noDataSlot.SetActive(true);
            hasDataSlot.SetActive(false);
            newGame = true;
        }
        // there is data for this profileId
        else
        {
            noDataSlot.SetActive(false);
            hasDataSlot.SetActive(true);
            newGame = false;

            LoadSaveSlotUI(data);
        }
    }

    private void LoadSaveSlotUI(GameData data)
    {
        currentLevel = data.currentSceneIndex;
        introSlotPrefab.SetActive(currentLevel == 1);
        tutorialSlotPrefab.SetActive(currentLevel == 2);
        level1SlotPrefab.SetActive(currentLevel == 3);
        boss1SlotPrefab.SetActive(currentLevel == 4 || currentLevel == 5);

        // load stats
        username.text = data.username;
        coins.text = data.coins.ToString();
        healthPoints.text = data.healthPoints.ToString();
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
