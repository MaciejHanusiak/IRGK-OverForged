using System;
using UnityEngine;

public class AnvilCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private AnvilForgeingRecipeSO[] anvilForgeingRecipeSOArray;
    private int anvilForgeingProgress;
    public override void Interact(Player player)
    {
        if (!HasSmithObject())
        {
            // There is no smith object

            if (player.HasSmithObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetSmithObject().GetSmithObjectSO()))
                {
                    player.GetSmithObject().SetSmithObjectParent(this);
                    anvilForgeingProgress = 0;

                    AnvilForgeingRecipeSO anvilForgeingRecipeSO = GetAnvilForgeingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)anvilForgeingProgress / anvilForgeingRecipeSO.anvilForgeingProgressMax
                    });
                }
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            // There is smith object

            if (player.HasSmithObject())
            {
                // Player carrying an object
            }
            else
            {
                // Player is no carrying anything
                this.GetSmithObject().SetSmithObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasSmithObject() && HasRecipeWithInput(GetSmithObject().GetSmithObjectSO()))
        {
            // There is a smith object AND it can be forged on anvil
            anvilForgeingProgress++;

            AnvilForgeingRecipeSO anvilForgeingRecipeSO = GetAnvilForgeingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
            SmithObjectSO outputSmithObjectSO = GetOutputForInput(GetSmithObject().GetSmithObjectSO());

            Analytics.Instance.PlayerStrikeSomethingOnce(
        (float)anvilForgeingProgress, anvilForgeingRecipeSO.anvilForgeingProgressMax, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {

                progressNormalized = (float)anvilForgeingProgress / anvilForgeingRecipeSO.anvilForgeingProgressMax
            });

            if (anvilForgeingProgress >= anvilForgeingRecipeSO.anvilForgeingProgressMax)
            {
                GetSmithObject().DestroySelf();
                SmithObject.SpawnSmithObject(outputSmithObjectSO, this);

                Analytics.Instance.PlayerForgedSomething(outputSmithObjectSO.objectName, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            }
        }


    }

    private bool HasRecipeWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (AnvilForgeingRecipeSO anvilForgeingRecipeSO in anvilForgeingRecipeSOArray)
        {
            if (anvilForgeingRecipeSO.input == inputSmithObjectSO)
            {
                return true;
            }
        }
        return false;
    }

    private SmithObjectSO GetOutputForInput(SmithObjectSO inputSmithObjectSO)
    {
        AnvilForgeingRecipeSO anvilForgeingRecipeSO = GetAnvilForgeingRecipeSOWithInput(inputSmithObjectSO);
        if (anvilForgeingRecipeSO != null)
        {
            return anvilForgeingRecipeSO.output;
        }
        return null;
    }
    
    private AnvilForgeingRecipeSO GetAnvilForgeingRecipeSOWithInput(SmithObjectSO inputSmithObjectSO) 
    {
        foreach (AnvilForgeingRecipeSO anvilForgeingRecipeSO in anvilForgeingRecipeSOArray)
        {
            if (anvilForgeingRecipeSO.input == inputSmithObjectSO)
            {
                return anvilForgeingRecipeSO;
            }
        }
        return null;
    }
}
