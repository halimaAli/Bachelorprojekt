using TMPro;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private TMP_Text pressButtonText;
    public int phase;

    private void Awake()
    {
        if (instance == null) { instance = this; };
        phase = 0;
    }

    public void ChangePhase()
    {
        phase += 1;
        OnDisplayText();
    }
    

    // after player tried to jump over pikes three times, this new text is displayed
    private void OnDisplayText()
    { 
        if (phase == 3)
        {
            PlayerController.instance.canChangeView = true; //placeholder
            displayedText.text = "It is time to change your Perspective";
            pressButtonText.text = "Press Q";
        }
    }
}
