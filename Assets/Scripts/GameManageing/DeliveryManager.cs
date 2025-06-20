using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 3;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count <= waitingRecipesMax)
            {

                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

                waitingRecipeSOList.Add(waitingRecipeSO);

            }
        }
    }

    public void DeliverRecipe(WeaponStandSmithObject weaponStandSmithObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.smithObjectSOList.Count == weaponStandSmithObject.GetSmithObjectSOList().Count)
            {
                // Has the same number of parts
                bool weaponStandMatchesRecipe = true;
                foreach (SmithObjectSO recipeSmithObjectSO in waitingRecipeSO.smithObjectSOList)
                {
                    // Cycling through all parts in waiting recipes list
                    bool partFound = false;
                    foreach (SmithObjectSO weaponStandSmithObjectSO in weaponStandSmithObject.GetSmithObjectSOList())
                    {
                        // Cycling through all parts on weapon stand
                        if (weaponStandSmithObjectSO == recipeSmithObjectSO)
                        {
                            // Part matches!
                            partFound = true;
                            break;
                        }

                    }
                    if (!partFound)
                    {
                        // This recipe part was not found on the Plate
                        weaponStandMatchesRecipe = false;
                    }
                }
                if (weaponStandMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // No matches found!
        Debug.Log("Player did not delivered good recipe!");
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

}
