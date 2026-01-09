using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeliveryManagerSingleIconUI : TimerSliderUI
{
    [Header("Delivery Specific")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    [SerializeField] private TextMeshProUGUI recipeReward;
    [SerializeField] private TextMeshProUGUI timeBonusMultiplier;
    [SerializeField] private TextMeshProUGUI recipeFinalReward;

    // T³o migania
    [Header("Flash Effect")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color successFlashColor = new Color(0.2f, 1f, 0.2f, 0.6f);
    [SerializeField] private Color lateFlashColor = new Color(1f, 0.2f, 0.2f, 0.6f);
    [SerializeField] private float flashDuration = 0.4f;
    [SerializeField] private int flashCount = 3;

    private Color originalBackgroundColor;
    private Coroutine flashCoroutine;

    public float FlashTotalTime => flashCount * flashDuration * 2f;

    [Header("Right Scroll Panel")]
    [SerializeField] private RectTransform rightPanelRect;
    [SerializeField] private GameObject rightPanelGO;
    [SerializeField] private TextMeshProUGUI rightPanelText;
    [SerializeField] private float rightPanelExpandTime = 0.5f;

    private Coroutine completionFxCoroutine;
    public float CopletionFxTotalTime => rightPanelExpandTime + FlashTotalTime;



    private int recipeIndex;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
        if (backgroundImage != null ) 
            originalBackgroundColor = backgroundImage.color;
        if (rightPanelGO != null)
            rightPanelGO.SetActive(false);
    }

    public void Flash(bool completedInTime)
    {
        if (backgroundImage == null)
            return;
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        Color c = completedInTime ? successFlashColor : lateFlashColor;
        flashCoroutine = StartCoroutine(FlashRoutine(c));
    }
    private System.Collections.IEnumerator FlashRoutine(Color c)
    {
        for (int i = 0; i < flashCount; i++)
        {
            backgroundImage.color = c;
            yield return new WaitForSeconds(flashDuration);

            backgroundImage.color = originalBackgroundColor;
            yield return new WaitForSeconds(flashDuration);
        }
        backgroundImage.color = originalBackgroundColor;
        flashCoroutine = null;
    }

    public void PlayCompletionFx(bool completedInTime, int recipeRewardValue, float multiplierValue, int recipeFinalRewardValue)
    {
        if (completionFxCoroutine != null)
            StopCoroutine(completionFxCoroutine);
        completionFxCoroutine = StartCoroutine(
            CompletionFxRoutine(completedInTime, recipeRewardValue, multiplierValue, recipeFinalRewardValue));
    }

    private IEnumerator CompletionFxRoutine(bool completedInTime, int recipeRewardValue, float multiplierValue, int recipeFinalRewardValue)
    {
        // 1) Poka¿ zwój i ustaw tekst
        if (rightPanelGO != null) rightPanelGO.SetActive(true);
        if (rightPanelRect != null) rightPanelRect.localScale = new Vector3(0f, 1f, 1f);

        if (rightPanelText != null)
            rightPanelText.text = $"{recipeRewardValue} * {multiplierValue} = {recipeFinalRewardValue}";

        // 2) Rozwijanie od prawej do lewej
        float t = 0f;
        while (t < rightPanelExpandTime)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / rightPanelExpandTime);
            if (rightPanelRect != null) rightPanelRect.localScale = new Vector3(k, 1f, 1f);
            yield return null;
        }
        if (rightPanelRect != null) rightPanelRect.localScale = new Vector3(1f, 1f, 1f);


        // 3) Miganie t³a
        Flash(completedInTime);
        yield return new WaitForSeconds(FlashTotalTime);

        // 4) Schowaj zwój
        if (rightPanelGO != null)
            rightPanelGO.SetActive(false);
        completionFxCoroutine = null;



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

        // Panel Final Reward dla recipe
        recipeReward.text = "Recipe reward: " + DeliveryManager.Instance.GetWaitingRecipeSOPriceByIndex(recipeIndex).ToString();
        timeBonusMultiplier.text = "Time Bonus x" + DeliveryManager.Instance.GetSingleRecipeRewardMultiplier(recipeIndex).ToString();
        decimal price = (decimal)DeliveryManager.Instance.GetWaitingRecipeSOPriceByIndex(recipeIndex);
        decimal multiplier = (decimal)DeliveryManager.Instance.GetSingleRecipeRewardMultiplier(recipeIndex);
        recipeFinalReward.text = "Final Reward: " + Math.Ceiling(price * multiplier);


        
        base.Update();

        if (IsExpired())
        {
            labelText.color = Color.red;
        }
    }
}