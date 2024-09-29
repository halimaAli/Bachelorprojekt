using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDialogue : Dialogue
{

    [SerializeField] private LoadingScene sceneLoader;
    [SerializeField] private string text;


    protected override string GetCurrentDialogText()
    {
        return text;
    }

    protected override int GetDialogLength()
    {
        return 1;
    }

    protected override void NextLine()
    {
        
    }

    protected override void EndDialog()
    {
        base.EndDialog();
        sceneLoader.LoadScene(0);
    }
}
