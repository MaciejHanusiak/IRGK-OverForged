using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ForgeCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Forgeing,
        Forged,
        Burned,
    }

    [SerializeField] private ForgeingRecipeSO[] forgeingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float forgeingTimer;
    private float burningTimer;
    private ForgeingRecipeSO forgeingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasSmithObject())
        {

            switch(state)
            {
                case State.Idle:
                    break;
                case State.Forgeing:
                    forgeingTimer += Time.deltaTime;
                    if (forgeingTimer > forgeingRecipeSO.forgeingTimerMax)
                    {
                        forgeingTimer = 0f;
                        // forged
                        Debug.Log("Fried!");
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(forgeingRecipeSO.output, this);
                        Debug.Log("ObjectFried!");
                        state = State.Forged;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                        burningTimer = 0f;
                    }
                    Debug.Log(forgeingTimer);
                    break;
                case State.Forged:
                    burningTimer += Time.deltaTime;
                    if (burningTimer > burningRecipeSO.burningTimerMax )
                    {
                        
                        // overforged
                        
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(burningRecipeSO.output, this);
                        Debug.Log("ObjectBurned!");
                        state = State.Burned;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                    }
                   Debug.Log(burningTimer);
                    break;
                case State.Burned:
                    break;
            }
            Debug.Log(state);

            
            

            
        }
    }

    public override void Interact(Player player)
    {
        if (!HasSmithObject())
        {
            //There is no SmithObject
            if (player.HasSmithObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetSmithObject().GetSmithObjectSO()))
                {
                    // Player carrying something that can be forged
                    player.GetSmithObject().SetSmithObjectParent(this);


                    forgeingRecipeSO = GetForgeingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                    state = State.Forgeing;
                    forgeingTimer = 0f;

                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is smith object here
            if (player.HasSmithObject())
            {
                // Player is carrying something
            }
            else
            {
                // Player is not carrying anything
                GetSmithObject().SetSmithObjectParent(player);

                state = State.Idle;
            }
        }
    }

    private bool HasRecipeWithInput(SmithObjectSO inputSmithObjectSO)
    {
        ForgeingRecipeSO forgeingRecipeSO = GetForgeingRecipeSOWithInput(inputSmithObjectSO);
        return forgeingRecipeSO != null;
    }

    private SmithObjectSO GetOutputForInput(SmithObjectSO inputSmithObjectSO)
    {
        ForgeingRecipeSO forgeingRecipeSO = GetForgeingRecipeSOWithInput(inputSmithObjectSO);
        if (forgeingRecipeSO != null)
        {
            return forgeingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private ForgeingRecipeSO GetForgeingRecipeSOWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (ForgeingRecipeSO forgeingRecipeSO in forgeingRecipeSOArray)
        {
            if (forgeingRecipeSO.input == inputSmithObjectSO)
            {
                return forgeingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputSmithObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
