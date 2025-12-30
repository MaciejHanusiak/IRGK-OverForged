using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeExpired;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeTimerMax = 20f;
    [SerializeField] private int waitingRecipesMax = 3;


    private List<RecipeSO> waitingRecipeSOList;
    private List<float> singleRecipeEndTimeList; // Time measurement for every recipe in level

    private float spawnRecipeTimer;
    
    

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        singleRecipeEndTimeList = new List<float>();
       
    }
    private void Update()
    {
        SpawnRecipeTimer();
        UpdateRecipeTimers();

    }
    private void SpawnRecipeTimer()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {

                RecipeSO newRecipe = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)]; // random new recipe form list in recipeList
                waitingRecipeSOList.Add(newRecipe);
                singleRecipeEndTimeList.Add(Time.time + newRecipe.recipeTime); // get recipeTime from RecipeSO and add to EndTimeList

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                Debug.Log($"Nowe zamównienie: {newRecipe.recipeName}");
                Analytics.Instance.RecipeGenerated(newRecipe.recipeName, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);

            }
            else
            {
                Debug.Log("Kolejka pe³na (3/3) - pomijam nowe zamównienie");
                Analytics.Instance.RecipeQuerryFull(LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);

            }
        }
    }
    private void UpdateRecipeTimers()
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            float endTime = singleRecipeEndTimeList[i];
            float remaining = endTime - Time.time;

                Debug.Log($"Remaining: {remaining}");
                Debug.Log($"endTime: {endTime}");
            if (remaining <= 0 && endTime > 0) // Receptura  raz po terminie
            {
                singleRecipeEndTimeList[i] = -1; // Oznacz jako expierd
                OnRecipeExpired?.Invoke(this, EventArgs.Empty);
                Debug.Log($"Zamównienie po terminie: {waitingRecipeSOList[i].recipeName}");
                Analytics.Instance.TimeForRecipeEnd(waitingRecipeSOList[i].recipeName, i, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            }
        }
    }
    public void DeliverRecipe(WeaponStandSmithObject weaponStandSmithObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            
#region MojeRozwiazanie
            // ############################## Robi to samo co groka ##############################
            /*
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
                            int moneyForOrder = 0;
                            switch (weaponStandSmithObjectSO.name)
                            {
                                case "CopperBlade":
                                    moneyForOrder = weaponStandSmithObjectSO.price;
                                    Debug.Log($"Odda³eœ MiedŸ, zarobi³eœ {moneyForOrder}!");
                                    break;
                                case "IronBladeNotSharpened":
                                    moneyForOrder = weaponStandSmithObjectSO.price;
                                    Debug.Log($"Odda³eœ ¯elazo, zarobi³eœ {moneyForOrder}!");

                                    break;

                            }
                            LevelStats.Instance.AddGold(moneyForOrder);
                            


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
                    // Tutaj trzeba dodaæ remove at dla TimeEndList?


                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
            */
#endregion
            
#region WersjaGroka

            if (waitingRecipeSO.smithObjectSOList.Count != weaponStandSmithObject.GetSmithObjectSOList().Count)
                continue; // Has not same count of parts - quit the loop

            // has same count of parts
            bool matches = true;
            int totalReward = 0;

            // Cycling through parts of weapon in recipe
            foreach (SmithObjectSO recipePart in waitingRecipeSO.smithObjectSOList)
            {
                // starting status of parts matching
                bool found = false; 

                // Cycling through parts of weapon on stand
                foreach (SmithObjectSO standPart in weaponStandSmithObject.GetSmithObjectSOList())
                {
                    if (standPart == recipePart)
                    {
                        
                        found = true;
                        break; // jeœli nie znajdzie to przechodzi do porównywania innych czêœci broni
                    }
                }
                if (!found)
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                // Sukces!
                Analytics.Instance.GoodRecipeDelivered(waitingRecipeSOList[i].recipeName, i, LevelStats.Instance.gold, GetRecipeRemainingTime(i), LevelTime.Instance.timeRemaining);
                waitingRecipeSOList.RemoveAt(i);
                singleRecipeEndTimeList.RemoveAt(i);

                totalReward = waitingRecipeSO.recipePrice;
                if (!IsRecipeExpired(i)) totalReward = waitingRecipeSO.recipePrice * 2;
                LevelStats.Instance.AddGold(totalReward);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty );

                Debug.Log($"Zamówienie ukoñczone! +{totalReward} z³ota!");
                return;
            }
            #endregion

        }

        // No matches found!
        Debug.Log("Player did not delivered good recipe!");
    }


    // GETTERY DO UI
    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRecipeSOList;
    public float GetRecipeRemainingTime(int index)
    {
        if (index >= singleRecipeEndTimeList.Count) return 0;
        float endTime = singleRecipeEndTimeList[index];
        return endTime > 0 ? endTime - Time.time : -1;
    }
    public bool IsRecipeExpired(int index)
    {
        // sprawdzamy indeks jest mniejszy ni¿ rozmiar tablicy index < count, aby uniknac out of range exception
        // sprawdzamy czy dany czas wygas³ - pozosta³y czas = -1
        return index < singleRecipeEndTimeList.Count && singleRecipeEndTimeList[index] == -1f;
    }
    public int GetWaitingRecipeSOListCount() => waitingRecipeSOList.Count;
    

}
