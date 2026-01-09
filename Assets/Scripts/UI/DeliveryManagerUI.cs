using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private bool isAnimating;

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
    private void DeliveryManager_OnRecipeExpired(object sender, System.EventArgs e) => UpdateVisual(); 
    private void DeliveryManager_OnRecipeCompleted(
        object sender, DeliveryManager.RecipeCompletedEventArgs e)
    {
        if (isAnimating) return;
        StartCoroutine(FlashThenRefresh(e.CompletedIndex, e.CompletedInTime));
    }
       // => UpdateVisual();
    // Dodane: odœwie¿a UI gdy zlecenie siê przeterminuje
    private System.Collections.IEnumerator FlashThenRefresh(int completedIndex, bool completedInTime)
    {
        isAnimating = true;

        // Turn off Update() of all panels;
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            var ui = child.GetComponent<DeliveryManagerSingleIconUI>();
            if (ui != null) ui.enabled = false;
        }

        var completedUI = GetUIByIndex(completedIndex);
        if (completedUI != null)
        {
            completedUI.Flash(completedInTime);
            // wait until ui stop blinking
            yield return new WaitForSeconds(completedUI.FlashTotalTime);
        }
        else
        {
            yield return null;
        }
            UpdateVisual(); // przebuduj ca³¹ listê po flashu
        isAnimating = false;


    }
    private DeliveryManagerSingleIconUI GetUIByIndex(int index)
    {
        // childs in container: [template] + instatnions in order
        int visualIdx = 0;
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            if (visualIdx == index)
                return child.GetComponent<DeliveryManagerSingleIconUI>();
            visualIdx++;
        }
        return null;
    }


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
