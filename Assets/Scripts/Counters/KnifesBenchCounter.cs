using System;
using UnityEngine;

public class KnifesBenchCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private KnifePlaningRecipeSO[] knifePlaningRecipeSOArray;

    private int knifePlaningProgress;
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
                    knifePlaningProgress = 0;

                    KnifePlaningRecipeSO knifePlaningRecipeSO = GetKnifePlaningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {

                        progressNormalized = (float)knifePlaningProgress / knifePlaningRecipeSO.knifePlaningProgressMax
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
            knifePlaningProgress++;


            KnifePlaningRecipeSO knifePlaningRecipeSO = GetKnifePlaningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
            SmithObjectSO outputSmithObjectSO = GetOutputForInput(GetSmithObject().GetSmithObjectSO());

            Analytics.Instance.PlayerWhittledSomethingOnce(
                    (float)knifePlaningProgress, knifePlaningRecipeSO.knifePlaningProgressMax, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {

                progressNormalized = (float)knifePlaningProgress / knifePlaningRecipeSO.knifePlaningProgressMax,
                
            });

            if (knifePlaningProgress >= knifePlaningRecipeSO.knifePlaningProgressMax)
            {
                GetSmithObject().DestroySelf();
                SmithObject.SpawnSmithObject(outputSmithObjectSO, this);

                Analytics.Instance.PlayerPlannedSomething(outputSmithObjectSO.objectName, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            }
        }


    }

    private bool HasRecipeWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (KnifePlaningRecipeSO knifePlaningForgeingRecipeSO in knifePlaningRecipeSOArray)
        {
            if (knifePlaningForgeingRecipeSO.input == inputSmithObjectSO)
            {
                return true;
            }
        }
        return false;
    }
    private SmithObjectSO GetOutputForInput(SmithObjectSO inputSmithObjectSO)
    {
        KnifePlaningRecipeSO knifePlaningRecipeSO = GetKnifePlaningRecipeSOWithInput(inputSmithObjectSO);
        if (knifePlaningRecipeSO != null)
        {
            return knifePlaningRecipeSO.output;
        }
        return null;
    }
    private KnifePlaningRecipeSO GetKnifePlaningRecipeSOWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (KnifePlaningRecipeSO knifePlaningRecipeSO in knifePlaningRecipeSOArray)
        {
            if (knifePlaningRecipeSO.input == inputSmithObjectSO)
            {
                return knifePlaningRecipeSO;
            }
        }
        return null;
    }
}
