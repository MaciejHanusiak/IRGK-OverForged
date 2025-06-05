using UnityEngine;

public class AnvilCounter : BaseCounter
{

    [SerializeField] private AnvilForgeingRecipeSO[] anvilForgeingRecipeSOArray;
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
            SmithObjectSO outputSmithObjectSO = GetOutputForInput(GetSmithObject().GetSmithObjectSO());
            GetSmithObject().DestroySelf();
            SmithObject.SpawnSmithObject(outputSmithObjectSO, this);
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
        foreach (AnvilForgeingRecipeSO anvilForgeingRecipeSO in anvilForgeingRecipeSOArray)
        {
            if (anvilForgeingRecipeSO.input == inputSmithObjectSO)
            {
                return anvilForgeingRecipeSO.output;
            }
        }
        return null;
    }
}
