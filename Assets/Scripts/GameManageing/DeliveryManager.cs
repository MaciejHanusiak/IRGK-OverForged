using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler<RecipeCompletedEventArgs> OnRecipeCompleted;
    public event EventHandler OnRecipeExpired;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeTimerMax = 20f;
    [SerializeField] private int waitingRecipesMax = 3;


    private List<RecipeSO> waitingRecipeSOList;
    private List<float> singleRecipeEndTimeList; // Time measurement for every recipe in level
    private List<float> singleRecipeRewardMultiplier;
    private List<int> singleRecipeFinalReward;

    private float spawnRecipeTimer;
    
    public class RecipeCompletedEventArgs : EventArgs
    {
        public bool CompletedInTime { get; }
        public RecipeCompletedEventArgs(bool completedInTime)
        {
            CompletedInTime = completedInTime;
        }
    }

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        singleRecipeEndTimeList = new List<float>();
        singleRecipeRewardMultiplier = new List<float>();
        singleRecipeFinalReward = new List<int>();
    }
    private void Update()
    {
        SpawnRecipeTimer();
        UpdateRecipes();

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
                singleRecipeRewardMultiplier.Add(69f);
                singleRecipeFinalReward.Add(69);
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
    private void UpdateRecipes()
    {
        
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {

            float endTime = singleRecipeEndTimeList[i]; 
            
            float remaining = endTime - Time.time;
            float recipemaxTime = waitingRecipeSOList[i].recipeTime;

                //Debug.Log($"Remaining: {remaining}");
                //Debug.Log($"endTime: {endTime}");
                //Debug.Log($"TimeMax: { waitingRecipeSOList[i].recipeTime}");
            // multiplier
            if (remaining > recipemaxTime * 0.5f)
            {
                singleRecipeRewardMultiplier[i] = 2f;

            }
            else if ( remaining > recipemaxTime * 0.2)
            {

                singleRecipeRewardMultiplier[i] = 1.5f;
            }
            else if (remaining > recipemaxTime * 0.000001)
            {
                singleRecipeRewardMultiplier[i] = 1.2f;

            }
            else
            {
                singleRecipeRewardMultiplier[i] = 1f;
            }

            // pasek czasu
            if (remaining <= 0 && endTime > 0) // Receptura  raz po terminie
            {
                singleRecipeEndTimeList[i] = -1; // Oznacz jako expierd
                OnRecipeExpired?.Invoke(this, EventArgs.Empty);
                Debug.Log($"Zamównienie po terminie: {waitingRecipeSOList[i].recipeName}");
                Analytics.Instance.TimeForRecipeEnd(waitingRecipeSOList[i].recipeName, i, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            }

            // finalna nagroda
            decimal price = (decimal)waitingRecipeSOList[i].recipePrice;  // jeœli recipePrice jest float/int, castuj
            decimal multiplier = (decimal)singleRecipeRewardMultiplier[i];
            singleRecipeFinalReward[i] = Convert.ToInt32(price * multiplier);
            float dprice = waitingRecipeSOList[i].recipePrice;
            float dmultiplier = singleRecipeRewardMultiplier[i];
            float dproduct = dprice * dmultiplier;

            //Debug.Log($"Recipe index: {i} | Price: {dprice} | Multiplier: {dmultiplier} | Iloczyn: {dproduct} | Ceiling: {Math.Ceiling(dproduct)} | Final: {(int)Math.Ceiling(dproduct)}");
        }
    }
    public void DeliverRecipe(WeaponStandSmithObject weaponStandSmithObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            
#region MojeRozwiazanieA
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
            
#region MojeRozwiazanieB

            if (waitingRecipeSO.smithObjectSOList.Count != weaponStandSmithObject.GetSmithObjectSOList().Count)
                continue; // Has not same count of parts - quit the loop

            // has same count of parts
            bool matches = true;
            int finalReward = 0;

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
                
                bool wasInTime = singleRecipeEndTimeList[i] > Time.time;

                waitingRecipeSOList.RemoveAt(i);
                singleRecipeEndTimeList.RemoveAt(i);

                finalReward = singleRecipeFinalReward[i];
                LevelStats.Instance.AddGold(finalReward);
                OnRecipeCompleted?.Invoke(this, new RecipeCompletedEventArgs(wasInTime)
                    );

                Debug.Log($"Zamówienie ukoñczone! +{finalReward} z³ota!");
                return;
            }
            #endregion

        }

        // No matches found!
        Debug.Log("Player did not delivered good recipe!");
    }


    // GETTERY DO UI
    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRecipeSOList;
    public int GetWaitingRecipeSOPriceByIndex(int index) => waitingRecipeSOList[index].recipePrice;
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

    public float GetSingleRecipeRewardMultiplier(int index) => singleRecipeRewardMultiplier[index];
    

}
