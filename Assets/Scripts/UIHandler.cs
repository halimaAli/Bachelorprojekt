
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [SerializeField] private Text amountOfCoins;
    private int coins;

    private void Awake()
    {
        if (instance == null) instance = this;
        coins = 0;
      
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
   
}
