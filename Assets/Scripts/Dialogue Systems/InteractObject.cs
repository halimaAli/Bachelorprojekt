using UnityEngine;

public class InteractObject : Dialogue
{
    [SerializeField] private string text;

    protected override string GetCurrentDialogText()
    {
        return text;
    }

    protected override void NextLine()
    {
        
    }

    protected override int GetDialogLength()
    {
        return 1;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!Camera.main.orthographic)
        {
            base.EndDialog();
            return;
        }

        base.OnTriggerEnter(other);
    }
}
