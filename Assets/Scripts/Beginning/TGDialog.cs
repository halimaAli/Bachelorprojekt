using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TGDialog : MonoBehaviour
{
    [Header("Dialog Components")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private string[] firstDialog;
    [SerializeField] private string[] secondDialog;
    [SerializeField] private float wordSpeed;
    
    [SerializeField] private TowerGuardian tg;
    [SerializeField] private BoxCollider triggerDialogCollider;

    [Header("SoundFX")]
    [SerializeField] private AudioClip firstSound;
    [SerializeField] private AudioClip secondSound;

    private string[] dialog;
    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;
    private bool playerIsClose;

    private void Start()
    {
        dialog = firstDialog;
    }

    private void Update()
    {
        if (!playerIsClose) return;

        if (!dialogPanel.activeInHierarchy)
        {
            InitiateDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping) CompleteTextInstantly();
            else if (dialogText.text == dialog[index]) NextLine();
        }
    }

    private void InitiateDialogue()
    {
        PlayerController.instance.active = false;
        SoundFXManager.instance.PlaySoundFXClip(firstSound, transform, 1, false);
        dialogPanel.SetActive(true);
        typingCoroutine = StartCoroutine(Typing());
    }

    private void Reset()
    {
        index = 0;
        dialogPanel.SetActive(false);
        dialogText.text = string.Empty;
        PlayerController.instance.active = true;
        tg.EnableMovement(true);
        triggerDialogCollider.enabled = false;
        playerIsClose = false;
    }

    private IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialog[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    private void CompleteTextInstantly()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogText.text = dialog[index];
        isTyping = false;
    }

    public void NextLine()
    {
        if (index < dialog.Length - 1)
        {
            index++;
            dialogText.text = string.Empty;
            typingCoroutine = StartCoroutine(Typing());

            if (index == 2)
            {
                SoundFXManager.instance.PlaySoundFXClip(secondSound, transform, 1, false);
            }
        }
        else
        {
            Reset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerIsClose = false;
            Reset();
        }
    }

    internal void StartSecondDialogue()
    {
        dialog = secondDialog;
        triggerDialogCollider.enabled = true;
    }
}
