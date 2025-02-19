using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Onscreentext : MonoBehaviour
{
    public TextMeshProUGUI uiText; // Assign in the Inspector
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private Coroutine textRoutine;

    private void Awake()
    {
        if (uiText == null)
        {
            Debug.LogError("OnScreenText: No Text component assigned!");
        }
        else
        {
            Color color = uiText.color;
            color.a = 0;
            uiText.color = color;
        }
    }

    public void ShowText(string message, float duration)
    {
        if (textRoutine != null)
        {
            StopCoroutine(textRoutine);
        }
        textRoutine = StartCoroutine(ShowTextRoutine(message, duration));
    }

    private IEnumerator ShowTextRoutine(string message, float duration)
    {
        uiText.text = message;
        yield return StartCoroutine(FadeText(0, 1, fadeInDuration)); // Fade in
        yield return new WaitForSeconds(duration); // Wait
        yield return StartCoroutine(FadeText(1, 0, fadeOutDuration)); // Fade out
    }

    private IEnumerator FadeText(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = uiText.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            uiText.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        uiText.color = color;
    }
}
