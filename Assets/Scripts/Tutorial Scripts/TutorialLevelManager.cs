using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class TutorialLevelManager : MonoBehaviour
{
    public static TutorialLevelManager instance;
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private TMP_Text pressButtonText;
    [SerializeField] private TMP_Text killText;
    [SerializeField] private GameObject _2DText;
    [SerializeField] private GameObject _3DText;
    [SerializeField] private GameObject mushroom;
    [SerializeField] private GameObject hiddenBarrier;
    [SerializeField] private Dialogue triggerDialogue;

    public Section section = Section.None;
    public int attempts;

    public enum Section
    {
        None,
        Lava,
        Monster
    }

    private void Start()
    {
        if (instance == null) { instance = this; };
        CameraManager.instance.DisableViewSwitch();
    }

    private void Update()
    {
        if (section == Section.Lava)
        {
            MinusOneLife();
        } 
        else if (section == Section.Monster)
        {
            CheckIfMushroomWasKilled();
        }

        if (!Camera.main.orthographic)
        {
            _2DText.SetActive(false);
            _3DText.SetActive(true);
        } else
        {
            _2DText.SetActive(true);
            _3DText.SetActive(false);
        }
    }


    public void SetSection(Section section)
    {
        this.section = section;
    }


    private void MinusOneLife()
    {
        if (attempts == 0)
        {
            displayedText.text = "Try to Jump over the Lava!";
            pressButtonText.text = "First attempt.";
        }
        else if (attempts == 1)
        {
            displayedText.text = "Come on, just Jump!";
            pressButtonText.text = "Second attempt.";
        }
        else if (attempts == 2)
        {
            displayedText.text = "You do know that you have to Jump with SPACE, right?";
            pressButtonText.text = "Third attempt.";
        }
        else
        {
            displayedText.text = string.Empty;
            pressButtonText.text = "GAME OVER";
            StartCoroutine(OnDisplayText());
        }
    }



    // After player tried to jump over the lava three times, this new text is displayed
    private IEnumerator OnDisplayText()
    {
        PlayerController.instance.active = false;
        yield return new WaitForSeconds(1);
        PlayerController.instance.active = true;
        triggerDialogue.isPlayerClose = true;
        CameraManager.instance.EnableViewSwitch();

        displayedText.text = string.Empty;
        pressButtonText.text = "Press Q";
        pressButtonText.color = Color.white;
        section = Section.None;
    }

    public void CheckIfMushroomWasKilled()
    {
        if (mushroom == null)
        {
            hiddenBarrier.SetActive(false);
        }

    }

    internal void CheckIfAttempted()
    {
        if (section == Section.Lava)
        {
            attempts++;
        } 
    }
}
