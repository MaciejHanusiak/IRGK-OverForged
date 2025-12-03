using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;


public class ForgeCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public enum State
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

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = forgeingTimer / forgeingRecipeSO.forgeingTimerMax
                    });

                    if (forgeingTimer > forgeingRecipeSO.forgeingTimerMax)
                    {
                        forgeingTimer = 0f;
                        
                        
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(forgeingRecipeSO.output, this);
                        Debug.Log("ObjectForged!");
                        state = State.Forged;

                        Analytics.Instance.PlayerSmeltSomething(forgeingRecipeSO.input.objectName,
                            forgeingTimer,
                            forgeingRecipeSO.forgeingTimerMax,
                            LevelStats.Instance.gold,
                            LevelTime.Instance.timeRemaining);

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                        burningTimer = 0f;

                    }
                    break;

                case State.Forged:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax,
                        forgeState = State.Forged
                    });
                    

                    if (burningTimer > burningRecipeSO.burningTimerMax )
                    {
                        
                        // overforged
                        
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(burningRecipeSO.output, this);

                        Debug.Log("ObjectBurned!");

                                            Analytics.Instance.PlayerBurntSomething(burningRecipeSO.input.objectName,
                        burningTimer,
                        burningRecipeSO.burningTimerMax,
                        LevelStats.Instance.gold,
                        LevelTime.Instance.timeRemaining);

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                        state = State.Burned;                       
                    }
                    break;

                case State.Burned:
                    break;
            }
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

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = forgeingTimer / forgeingRecipeSO.forgeingTimerMax
                    });
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
                forgeingTimer = 0f;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = forgeingTimer / forgeingRecipeSO.forgeingTimerMax
                });
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
