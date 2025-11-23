using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeliveryManagerSingleIconUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Slider recipeTimeSlider;
    [SerializeField] private Image sliderFillImage;
    [SerializeField] private Image sliderBackgroundImage; // Background (opcjonalnie)
    [SerializeField] private float warningThreshold = 0.4f;

    // przechowuje indeks zlecenia w kolejce
    private int recipeIndex;
    private Coroutine blinkCoroutine;
    bool isExpired;


    // Ustawienia
    private const float BLINK_START_PERCENT = 0.8f;   // Miganie od 40%
    private const float COLOR_CHANGE_START = 0.7f;    // Kolor zaczyna siê zmieniaæ od 50%
    private const float MIN_FREQUENCY = 4f;
    private const float MAX_FREQUENCY = 20f;

    // Kolory
    private readonly Color COLOR_GOOD = new Color(0.1f, 0.8f, 0.1f); // zielony
    private readonly Color COLOR_WARNING = new Color(1f, 0.8f, 0f);   // ¿ó³ty
    private readonly Color COLOR_DANGER = new Color(1f, 0.2f, 0.2f);  // czerwony

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    // dodany parametr index
    public void SetRecipeSO(RecipeSO recipeSO, int index)
    {
        this.recipeIndex = index;

        // ustaw tylko nazwê i ikony, bez dotykania timera
        recipeNameText.text = $"{index + 1}. {recipeSO.recipeName}";
        
        // ustaw maksymalny czas - wartoœc bêdzie aktualizowana w Update()
        recipeTimeSlider.maxValue = recipeSO.recipeTime;
        
        Debug.Log("RecipeTimeSlider value: " + recipeTimeSlider.value);
        Debug.Log("Time DeltaTime: " + Time.deltaTime);

        // Ustaw ikony czêœci (jednorazowo)
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (SmithObjectSO smithObjectSO in recipeSO.smithObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = smithObjectSO.sprite;
        }

        StopBlinking();
    }

    private void Update()
    {
        // SprawdŸ, czy ten indeks nadal istnieje
        if (recipeIndex >= DeliveryManager.Instance.GetWaitingRecipeSOList().Count)
        {
            // UI nieaktualne - ukryj
            recipeTimeSlider.value = 0;
            recipeNameText.color = Color.gray;
            if (sliderFillImage) sliderFillImage.color = Color.gray;
            StopBlinking();
            return;
        }

        float remaining = DeliveryManager.Instance.GetRecipeRemainingTime(recipeIndex);
        isExpired = DeliveryManager.Instance.IsRecipeExpired(recipeIndex);
        float totalTime = recipeTimeSlider.maxValue;
        float precentLeft = isExpired ? 0f : remaining / totalTime;
        RecipeSO currentRecipe = DeliveryManager.Instance.GetWaitingRecipeSOList()[recipeIndex];


        
        if (isExpired)
        {
            recipeTimeSlider.value = 0;
            recipeNameText.color = Color.red;
            SetSliderColor(COLOR_DANGER);
        }
        else if (precentLeft < COLOR_CHANGE_START)
        {
            recipeTimeSlider.value = remaining;
            recipeNameText.color = Color.white;

            // slider kolor
            float t = precentLeft / COLOR_CHANGE_START;
            Color currentColor = Color.Lerp(COLOR_DANGER, COLOR_WARNING, t);
            if (precentLeft > 0.25f) currentColor = Color.Lerp(COLOR_WARNING, COLOR_GOOD, (precentLeft - 0.25f) / 0.25f);
            SetSliderColor(currentColor);
            //StartBlinking(8f);
        }
        else
        {
            recipeTimeSlider.value = remaining;
            recipeNameText.color = Color.white;
            SetSliderColor(COLOR_GOOD);
            //StopBlinking();
        }
    }
    private void SetSliderColor(Color color)
    {
        if (sliderFillImage != null) sliderFillImage.color = color;
        if (sliderBackgroundImage != null) sliderBackgroundImage.color = color * 0.3f;
    }
    private void ResetSliderColor()
    {
        SetSliderColor(COLOR_GOOD);
    }

    private void StartBlinking(float frequency = 8f)
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkText(frequency));
        }
    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            recipeNameText.color = Color.white;
        }
    }

    private IEnumerator BlinkText(float frequency)
    {
        while (true)
        {
            float alpha = Mathf.Sin(Time.time * frequency) * 0.5f + 0.5f;
            recipeNameText.color = new Color(1, 0, 0, alpha);
            sliderFillImage.color = new Color(1, 0, 0, alpha);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        StopBlinking();
    }
}