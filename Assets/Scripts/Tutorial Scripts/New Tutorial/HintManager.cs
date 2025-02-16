using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Image hintPanelImage; 
    public TMP_Text messageText; 
    public TMP_Text keyText; 
    public float fadeDuration = 0.5f; 
    public float hintDuration = 3f; 

    private Coroutine currentHintCoroutine;

    public void ShowHint(string message, string key)
    {
        if (currentHintCoroutine != null)
        {
            StopCoroutine(currentHintCoroutine);
        }
        currentHintCoroutine = StartCoroutine(HintRoutine(message, key));
    }

    private IEnumerator HintRoutine(string message, string key)
    {
        messageText.text = message;
        keyText.text = key;
        yield return StartCoroutine(FadeIn());


       // yield return StartCoroutine(FadeOut());
    }

    public void HideHint()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        hintPanelImage.gameObject.SetActive(true);
        float elapsedTime = 0f;

        Color panelColor = hintPanelImage.color;
        Color messageColor = messageText.color;
        Color keyColor = keyText.color;

        panelColor.a = 0f;
        messageColor.a = 0f;
        keyColor.a = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            panelColor.a = alpha;
            messageColor.a = alpha;
            keyColor.a = alpha;

            hintPanelImage.color = panelColor;
            messageText.color = messageColor;
            keyText.color = keyColor;

            yield return null;
        }

        panelColor.a = 1f;
        messageColor.a = 1f;
        keyColor.a = 1f;

        hintPanelImage.color = panelColor;
        messageText.color = messageColor;
        keyText.color = keyColor;
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        Color panelColor = hintPanelImage.color;
        Color messageColor = messageText.color;
        Color keyColor = keyText.color;

        panelColor.a = 1f;
        messageColor.a = 1f;
        keyColor.a = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

            panelColor.a = alpha;
            messageColor.a = alpha;
            keyColor.a = alpha;

            hintPanelImage.color = panelColor;
            messageText.color = messageColor;
            keyText.color = keyColor;

            yield return null;
        }

        panelColor.a = 0f;
        messageColor.a = 0f;
        keyColor.a = 0f;

        hintPanelImage.color = panelColor;
        messageText.color = messageColor;
        keyText.color = keyColor;

        hintPanelImage.gameObject.SetActive(false);
    }
}
