using System.Collections;
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
        phase = 3;
    }

    public void MinusOneLife()
    {
        phase -= 1;
        if (phase == 1) 
        {
            displayedText.text = "Come on, just Jump!";
            pressButtonText.text ="Only 1 Life remaining!"; 
        } 
        else if (phase > 1) 
        {
            displayedText.text = "You got this!";
            pressButtonText.text = phase + " Lives remaining";
        }
        else
        {
            displayedText.text = string.Empty;
            pressButtonText.text = "GAME OVER";
            StartCoroutine(OnDisplayText());
        }
    }


    // after player tried to jump over pikes three times, this new text is displayed
    private IEnumerator OnDisplayText()
    { 
        yield return new WaitForSeconds(1);
        PlayerController.instance.canChangeView = true; //placeholder
        displayedText.text = "Just kidding \n"+"It is time to change your Perspective!";
        pressButtonText.text = "Press Q";
        pressButtonText.color = Color.white;
        
    }
}
