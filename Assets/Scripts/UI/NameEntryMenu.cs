using UnityEngine;
using TMPro;

public class NameEntryMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;

    [SerializeField] private LoadingScene sceneLoader;

    private void Start()
    {
        usernameInputField.text = "";
    }

    public void OnStartGameClicked()
    {
        string enteredUsername = usernameInputField.text;
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.SetUsername(enteredUsername);
        }
        
        sceneLoader.LoadScene(1);
    }
}
