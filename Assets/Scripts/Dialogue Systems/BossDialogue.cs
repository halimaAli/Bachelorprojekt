using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogue : Dialogue
{
    [SerializeField] private string dialogue;

    [SerializeField] private AudioClip sound;
    [SerializeField] private LoadingScene sceneLoader;

    protected override void StartDialog()
    {
        base.StartDialog();
        SoundFXManager.instance.PlaySoundFXClip(sound, transform, 1, false);
    }

    protected override void EndDialog()
    {
        base.EndDialog();
        sceneLoader.LoadScene(5);
    }

    protected override int GetDialogLength()
    {
        return 1;
    }

    protected override string GetCurrentDialogText()
    {
        return dialogue;
    }

    protected override void NextLine()
    {
       
    }
}
