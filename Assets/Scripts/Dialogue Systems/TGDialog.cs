using UnityEngine;


public class TGDialog : Dialogue
{
    [SerializeField] private string[] firstDialog;
    [SerializeField] private string[] secondDialog;
    [SerializeField] private TowerGuardian tg;

    [SerializeField] private AudioClip firstSound;
    [SerializeField] private AudioClip secondSound;

    private string[] currentDialog;

    private void Start()
    {
        currentDialog = firstDialog;
    }

    protected override void StartDialog()
    {
        base.StartDialog();
        SoundFXManager.instance.PlaySoundFXClip(firstSound, transform, 1, false);
    }

    protected override void EndDialog()
    {
        base.EndDialog();
        if (tg != null) tg.EnableMovement(true);
    }

    protected override void NextLine()
    {
        if (dialogIndex < currentDialog.Length - 1)
        {
            dialogIndex++;
            dialogText.text = string.Empty;
            typingCoroutine = StartCoroutine(Typing(GetCurrentDialogText()));

            if (dialogIndex == 2)
            {
                SoundFXManager.instance.PlaySoundFXClip(secondSound, transform, 1, false);
            }
        }
        else
        {
            EndDialog();
        }
    }

    protected override int GetDialogLength()
    {
        return currentDialog.Length;
    }

    protected override string GetCurrentDialogText()
    {
        return currentDialog[dialogIndex];
    }

    public void StartSecondDialogue()
    {
        currentDialog = secondDialog;
        base.boxCollider.enabled = true;
    }
}
