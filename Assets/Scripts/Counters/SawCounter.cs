using UnityEngine;

public class SawCounter : BaseCounter
{
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
    private float cuttingTime;
    private float overCuttingTime;
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
                    cuttingTime += Time.deltaTime;
                    if (cuttingTime > cuttingRecipeSO.cuttingTimerMax)
                    {
                        cuttingTime = 0f;
                        // forged
                        Debug.Log("Cutted!");
                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(cuttingRecipeSO.output, this);
                        Debug.Log("ObjectCutted!");
                        state = State.Cutted;
                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                        overCuttingTime = 0f;
                    }
                    Debug.Log(cuttingTime);
                    break;
                case State.Cutted:
                    overCuttingTime += Time.deltaTime;
                    if (overCuttingTime > overCuttingRecipeSO.overCuttingTimerMax)
                    {

                        // overforged

                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(overCuttingRecipeSO.output, this);
                        Debug.Log("ObjectOverCutted!");
                        state = State.OverCutted;
                        overCuttingTime = 0f;
                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                    }
                    Debug.Log(overCuttingTime);
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
                    cuttingTime = 0f;

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
