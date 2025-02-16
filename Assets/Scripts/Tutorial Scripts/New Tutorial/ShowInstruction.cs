using TMPro;
using UnityEngine;
using System.Collections;

public class ShowInstruction : MonoBehaviour
{
    public TMP_Text[] instructionTextObjs;
    public float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeTextIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeTextOut());
        }
    }

    private IEnumerator FadeTextIn()
    {
        foreach (var textObj in instructionTextObjs)
        {
            textObj.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            foreach (var textObj in instructionTextObjs)
            {
                Color textColor = textObj.color;
                textColor.a = alpha;
                textObj.color = textColor;
            }
            yield return null;
        }

        foreach (var textObj in instructionTextObjs)
        {
            Color textColor = textObj.color;
            textColor.a = 1f;
            textObj.color = textColor;
        }
    }

    private IEnumerator FadeTextOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < .5f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            foreach (var textObj in instructionTextObjs)
            {
                Color textColor = textObj.color;
                textColor.a = alpha;
                textObj.color = textColor;
            }
            yield return null;
        }

        foreach (var textObj in instructionTextObjs)
        {
            Color textColor = textObj.color;
            textColor.a = 0f;
            textObj.color = textColor;
            textObj.gameObject.SetActive(false);
        }
    }
}
