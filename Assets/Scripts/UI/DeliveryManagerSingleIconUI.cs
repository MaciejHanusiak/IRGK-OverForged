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

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;
        recipeTimeSlider.maxValue = recipeSO.recipeTime;
        recipeTimeSlider.value = recipeSO.recipeTime;
        Debug.Log("RecipeTimeSlider value: " + recipeTimeSlider.value);
        Debug.Log("Time DeltaTime: " + Time.deltaTime);

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

        StartCoroutine(UpdateSlider(recipeSO.recipeTime));
    }

    private IEnumerator UpdateSlider(float recipeTime)
    {
        float currentTime = recipeTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            recipeTimeSlider.value = currentTime;
            yield return null; // Czekaj do nastêpnej klatki
        }
    }
}
