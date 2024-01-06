using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [SerializeField] private Text amountOfCoins;
    private int coins;

    [SerializeField] private GameObject _3DTutorialText;
    [SerializeField] private GameObject _2DTutorialText;

    private void Awake()
    {
        if (instance == null) instance = this;

        coins = 0;
        _3DTutorialText.SetActive(false);
        _2DTutorialText.SetActive(true);
    }

    public void OnExitClick()
    { 
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit(); // original code to quit Unity player
    #endif
        
    }

    public void onCoinCollected()
    {
        coins += 1;
        amountOfCoins.text = coins + " x";
    }

    public void EnableText(bool is2D)
    {
        if (!is2D)
        {
            _3DTutorialText.SetActive(true);
            _2DTutorialText.SetActive(false);
        } else
        {
            _3DTutorialText.SetActive(false);
            _2DTutorialText.SetActive(true);
        }
    }
   
}
