using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class stamina : MonoBehaviour
{
    public Movement movement;
    public Image staminaBar; // The stamina bar image
    public float fadeDuration = 0.5f; // Time taken to fade in/out
    public float fadeDelay = 2f; // Time before the bar starts fading out
    private float lastStaminaValue;
    private Coroutine fadeCoroutine;

    void Start()
    {
        lastStaminaValue = staminaBar.fillAmount;
        SetImageAlpha(0); // Start hidden
    }

    void Update()
    {
        float newFillAmount = movement.stamina / movement.maxStamina;
        if (Mathf.Abs(newFillAmount - lastStaminaValue) > 0.01f) // Detect change
        {
            lastStaminaValue = newFillAmount;
            staminaBar.fillAmount = newFillAmount;
            ShowStaminaBar();
        }
    }

    void ShowStaminaBar()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        SetImageAlpha(1);
        fadeCoroutine = StartCoroutine(FadeOutStaminaBar());
    }

    IEnumerator FadeOutStaminaBar()
    {
        yield return new WaitForSeconds(fadeDelay); // Wait before fading out
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(0);
    }

    void SetImageAlpha(float alpha)
    {
        Color color = staminaBar.color;
        color.a = alpha;
        staminaBar.color = color;
    }
}
