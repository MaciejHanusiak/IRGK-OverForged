using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeExpired += DeliveryManager_OnRecipeExpired; // dodany nowy event przeterminowania
    }
    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e) => UpdateVisual();
    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e) => UpdateVisual();
    // Dodane: odœwie¿a UI gdy zlecenie siê przeterminuje
    private void DeliveryManager_OnRecipeExpired(object sender, System.EventArgs e) => UpdateVisual(); 
    


    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        var recipes = DeliveryManager.Instance.GetWaitingRecipeSOList();
        for (int i = 0; i < recipes.Count; i++)
        //foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) // mój stary kod
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            // recipeTransform.GetComponent<DeliveryManagerSingleIconUI>().SetRecipeSO(recipeSO);

            var singleUI = recipeTransform.GetComponent<DeliveryManagerSingleIconUI>();

            // przekazujemy przepis i pozycjê w kolejce
            singleUI.SetRecipeSO(recipes[i], i);

        }
    }
}
