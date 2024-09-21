using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Rendering;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TMP_Text dialogText;
    private BoxCollider boxCollider;

    private Coroutine typingCoroutine;
    private bool isTyping;


    private bool isPlayerClose;

    void Start()
    {
        textBox.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
    }


    void Update()
    {
        if (!isPlayerClose) return;


        if (!textBox.activeInHierarchy)
        {
            PlayerController.instance.active = false;
            textBox.SetActive(true);
            typingCoroutine = StartCoroutine(Typing());
        } 
        else  if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping) CompleteTextInstantly();
            else if (dialogText.text == text) Reset();
        }
    }


    private void Reset()
    {
        textBox.SetActive(false);
        dialogText.text = string.Empty;
        PlayerController.instance.active = true;
        boxCollider.enabled = false;
        isPlayerClose = false;
    }


    private IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.06f);
        }
        isTyping = false;
    }

    private void CompleteTextInstantly()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogText.text = text;
        isTyping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerClose = false;
        }
    }
}
