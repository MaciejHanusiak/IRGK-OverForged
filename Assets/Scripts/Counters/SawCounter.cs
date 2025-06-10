using System;
using UnityEngine;

public class SawCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    private enum State
    {
        Idle,
        Cutting,
        Cutted,
        OverCutted,
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    [SerializeField] private OverCuttingRecipeSO[] overCuttingRecipeSOArray;

    private State state;
    private float cuttingTimer;
    private float overCuttingTimer;
    private CuttingRecipeSO cuttingRecipeSO;
    private OverCuttingRecipeSO overCuttingRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasSmithObject())
        {

            switch (state)
            {
                case State.Idle:
                    break;
                case State.Cutting:
                    cuttingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = cuttingTimer / cuttingRecipeSO.cuttingTimerMax
                    });

                    if (cuttingTimer > cuttingRecipeSO.cuttingTimerMax)
                    {
                        cuttingTimer = 0f;
                        // forged
                        Debug.Log("Cutted!");
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(cuttingRecipeSO.output, this);
                        Debug.Log("ObjectCutted!");
                        state = State.Cutted;
                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                        overCuttingTimer = 0f;
                    }
                    Debug.Log(cuttingTimer);
                    break;

                case State.Cutted:
                    overCuttingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = cuttingTimer / cuttingRecipeSO.cuttingTimerMax
                    });

                    if (overCuttingTimer > overCuttingRecipeSO.overCuttingTimeMax)
                    {

                        // overCutted

                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(overCuttingRecipeSO.output, this);
                        Debug.Log("ObjectOverCutted!");
                        
                        overCuttingTimer = 0f;
                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                        state = State.OverCutted;
                    }
                    Debug.Log(overCuttingTimer);
                    break;

                case State.OverCutted:
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


                    cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                    state = State.Cutting;
                    cuttingTimer = 0f;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = cuttingTimer / cuttingRecipeSO.cuttingTimerMax
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
            }
        }
    }

    private bool HasRecipeWithInput(SmithObjectSO inputSmithObjectSO)
    {
        CuttingRecipeSO forgeingRecipeSO = GetCuttingRecipeSOWithInput(inputSmithObjectSO);
        return forgeingRecipeSO != null;
    }

    private SmithObjectSO GetOutputForInput(SmithObjectSO inputSmithObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputSmithObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (CuttingRecipeSO forgeingRecipeSO in cuttingRecipeSOArray)
        {
            if (forgeingRecipeSO.input == inputSmithObjectSO)
            {
                return forgeingRecipeSO;
            }
        }
        return null;
    }

    private OverCuttingRecipeSO GetOverCuttingRecipeSOWithInput(SmithObjectSO inputSmithObjectSO)
    {
        foreach (OverCuttingRecipeSO overCuttingRecipeSO in overCuttingRecipeSOArray)
        {
            if (overCuttingRecipeSO.input == inputSmithObjectSO)
            {
                return overCuttingRecipeSO;
            }
        }
        return null;
    }
}
