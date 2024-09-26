using System.Collections;
using UnityEngine;
using TMPro;
using System;

public abstract class Dialogue: MonoBehaviour
{
    [SerializeField] protected GameObject dialogPanel;
    [SerializeField] protected TMP_Text dialogText;
     protected float wordSpeed = 0.06f;

    protected bool isTyping;
    public bool isPlayerClose;
    protected Coroutine typingCoroutine;
    [SerializeField] protected BoxCollider boxCollider;
    protected int dialogIndex;

    protected virtual void Update()
    {
        if (!isPlayerClose) return;

        if (!dialogPanel.activeInHierarchy)
        {
            StartDialog();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping) CompleteTextInstantly();
            else if (CheckIfDialogComplete()) EndDialog();
            else NextLine();
        }
    }

    protected abstract int GetDialogLength();

    protected abstract string GetCurrentDialogText();

    protected virtual void StartDialog()
    {
        PlayerController.instance.active = false;
        dialogPanel.SetActive(true);
        typingCoroutine = StartCoroutine(Typing(GetCurrentDialogText()));
    }

    protected virtual void EndDialog()
    {
        dialogIndex = 0;
        dialogPanel.SetActive(false);
        dialogText.text = string.Empty;
        PlayerController.instance.active = true;
        isPlayerClose = false;

        if(boxCollider != null) boxCollider.enabled = false;
    }

    protected IEnumerator Typing(string text)
    {
        isTyping = true;
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    protected void CompleteTextInstantly()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogText.text = GetCurrentDialogText();
        isTyping = false;
    }

    protected virtual bool CheckIfDialogComplete()
    {
        return dialogIndex >= GetDialogLength() - 1;
    }

    protected abstract void NextLine();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = false;
            EndDialog();
        }
    }
}
