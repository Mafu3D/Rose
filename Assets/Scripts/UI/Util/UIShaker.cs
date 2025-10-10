using System.Collections;
using UnityEngine;

public class UIShaker : MonoBehaviour
{
    // This was written with Chat GPT... mostly (T_T)

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void StartShake(float duration, float magnitude, float frequency)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude, frequency));
    }

    IEnumerator ShakeCoroutine(float duration, float magnitude, float frequency)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float xOffset = Mathf.PerlinNoise(Time.time * frequency, 0f) * 2f - 1f;
            float yOffset = Mathf.PerlinNoise(0f, Time.time * frequency) * 2f - 1f;

            rectTransform.anchoredPosition = originalPosition + new Vector2(xOffset, yOffset) * magnitude;

            timer += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition; // Reset to original position
    }
}
