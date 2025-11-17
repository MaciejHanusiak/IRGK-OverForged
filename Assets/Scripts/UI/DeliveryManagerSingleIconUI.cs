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

    // przechowuje indeks zlecenia w kolejce
    private int recipeIndex;

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
    }

    private void Update()
    {
        // SprawdŸ, czy ten indeks nadal istnieje
        if (recipeIndex >= DeliveryManager.Instance.GetWaitingRecipeSOList().Count)
        {
            // UI nieaktualne - ukryj
            recipeTimeSlider.value = 0;
            recipeNameText.color = Color.gray;
            return;
        }

        float remaining = DeliveryManager.Instance.GetRecipeRemainingTime(recipeIndex);
        bool isExpired = DeliveryManager.Instance.IsRecipeExpired(recipeIndex);
        RecipeSO currentRecipe = DeliveryManager.Instance.GetWaitingRecipeSOList()[recipeIndex]; // zapytaæ co to robi, dlaczego w kwadratowym nawiasie jest recipeIndex


        // if/ else do dodania na miganie z groka: https://grok.com/c/fc6f0e9d-e8ae-484a-8e9e-c78d484e2788
        if (isExpired)
        {
            recipeTimeSlider.value = 0;
            // mo¿na dodaæ kourtenê od mrugania
        }
        else
        {
            recipeTimeSlider.value = remaining;
        }
    }
}