using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeliveryManagerSingleIconUI : TimerSliderUI
{
    [Header("Delivery Specific")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private int recipeIndex;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO, int index)
    {
        this.recipeIndex = index;

        // Nazwa z numeracj¹
        if (labelText) labelText.text = $"{index + 1}. {recipeSO.recipeName}";

        // Maksymalny czas slidera
        timerSlider.maxValue = recipeSO.recipeTime;

        // Ikony czêœci
        foreach (Transform child in iconContainer)
            if (child != iconTemplate) Destroy(child.gameObject);

        foreach (SmithObjectSO part in recipeSO.smithObjectSOList)
        {
            var icon = Instantiate(iconTemplate, iconContainer);
            icon.gameObject.SetActive(true);
            icon.GetComponent<Image>().sprite = part.sprite;
        }
    }

    // KLUCZOWE: te 3 metody dziedziczone z TimerSliderUI
    protected override float GetRemainingTime()
        => DeliveryManager.Instance.GetRecipeRemainingTime(recipeIndex);

    protected override float GetTotalTime()
        => timerSlider.maxValue;

    protected override bool IsExpired()
        => DeliveryManager.Instance.IsRecipeExpired(recipeIndex);

    // Opcjonalnie: mo¿esz nadpisaæ tekst (np. dodaæ czas)
    protected override string GetLabelText()
    {
        var list = DeliveryManager.Instance.GetWaitingRecipeSOList();
        if (recipeIndex >= list.Count) return "???";

        float remaining = GetRemainingTime();
        int seconds = Mathf.CeilToInt(remaining);
        string timeText = IsExpired() ? "PO TERMINIE!" : $"{seconds}s";

        return $"{recipeIndex + 1}. {list[recipeIndex].recipeName} [{timeText}]";
    }

    // Sprawdzenie, czy slot nadal istnieje (wa¿ne przy usuwaniu zlecenia)
    override protected void Update()
    {
        if (recipeIndex >= DeliveryManager.Instance.GetWaitingRecipeSOList().Count)
        {
            timerSlider.value = 0;
            if (labelText) labelText.color = Color.gray;
            if (sliderFillImage) sliderFillImage.color = Color.gray;
            return;
        }

        // Resztê robi klasa bazowa!
        base.Update();

        if (IsExpired())
        {
            labelText.color = Color.red;
        }
    }
}