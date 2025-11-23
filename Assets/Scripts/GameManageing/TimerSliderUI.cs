using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public abstract class TimerSliderUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] protected Slider timerSlider;
    [SerializeField] protected TextMeshProUGUI labelText;
    [SerializeField] protected Image sliderFillImage;
    [SerializeField] protected Image sliderBackgroundImage;

    [Header("Visual Settings")]
    [SerializeField] private float blinkStartPrecent = 0.40f;
    [SerializeField] private float colorChangeStartPrecent = 0.50f;
    [SerializeField] private float minBlinkFrequency = 4f;
    [SerializeField] private float maxBlinkFrequecny = 20f;

    [Header("Colors")]
    [SerializeField] private Color colorGood = new Color(0.1f, 0.8f, 0.1f); // gren
    [SerializeField] private Color colorWarning = new Color(1f, 0.8f, 0f); // yellow
    [SerializeField] private Color colorDanger = new Color(1f, 0.2f, 0.2f);

    private Coroutine blinkCoroutine;

    protected abstract float GetRemainingTime();
    protected abstract float GetTotalTime();
    protected abstract bool IsExpired();

    protected virtual string GetLabelText() => "";

  

    protected virtual void Update()
    {
        float remaining = GetRemainingTime();
        float total = GetTotalTime();
        bool expired = IsExpired();

        float precentLeft = expired ? 0f : remaining / total;

        // Timer update
        timerSlider.value = expired ? 0f : remaining;

        // Color
        UpdateSliderColor(precentLeft, expired);

        // Blinking
        UpdateBlinking(precentLeft, expired);


    }

    private void UpdateSliderColor(float precentLeft, bool expired)
    {
        Color targetColor;

        if (expired)
            targetColor = colorDanger;
        else if (precentLeft <= colorChangeStartPrecent)
        {
            float t = precentLeft / colorChangeStartPrecent;
            targetColor = Color.Lerp(colorDanger, colorWarning, t);
            if (precentLeft > 0.25f)
            {
                targetColor = Color.Lerp(colorWarning, colorGood, (precentLeft - 0.25f) / 0.25f);
            }
        }
        else
        {
            targetColor = colorGood;
        }

        if (sliderFillImage) sliderFillImage.color = targetColor;
        if (sliderBackgroundImage) sliderBackgroundImage.color = targetColor * 0.4f;
    }

    // Update Blinking
    private void UpdateBlinking(float precentLeft, bool expired)
    {
        if (precentLeft <= blinkStartPrecent || expired)
        {
            float t = expired ? 0f : (blinkStartPrecent - precentLeft) / blinkStartPrecent;
            float frequency = Mathf.Lerp(minBlinkFrequency, maxBlinkFrequecny, t);
            StartBlinking(frequency);
        }
        else
        {
            StopBlinking();
            if (labelText) labelText.color = Color.white;
        }

        // Text (optional)
        if (labelText) labelText.text = GetLabelText();
    }
    protected void StartBlinking(float frequency)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCoroutine(frequency));
    }
    protected void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            if (labelText) labelText.color = Color.white;
        }
    }

    private IEnumerator BlinkCoroutine(float frequency)
    {
        while (true)
        {
            float alpha = Mathf.Sin(Time.time * frequency * Mathf.PI) * 0.5f + 0.5f;
            if (labelText) labelText.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }
    }
    private void OnDestroy() => StopBlinking();
}
