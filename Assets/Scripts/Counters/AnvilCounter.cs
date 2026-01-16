using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnvilCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private AnvilForgeingRecipeSO[] anvilForgeingRecipeSOArray;

    [Header("Hit Flash (URP 2D)")]
    [SerializeField] private Light2D hitFlashLight;
    [SerializeField] private float hitFlashMinIntensity = 1.8f;
    [SerializeField] private float hitFlashMaxIntensity = 2.2f;
    [SerializeField] private float hitFlashInTime = 0.02f;   // szybki zap³on
    [SerializeField] private float hitFlashOutTime = 1f;  // gaœniêcie
    [SerializeField] private float hitFlashChance = 1f;      // 1 = zawsze, np. 0.85 = czasem

    private float hitFlashTimer;
    private float hitFlashPeak;
    private bool hitFlashActive;

    private int anvilForgeingProgress;

    private void Update()
    {
        UpdateHitFlash();
    }

    private void UpdateHitFlash()
    {
        if (hitFlashLight == null)
            return;

        if (!hitFlashActive)
        {
            hitFlashLight.intensity = 0f;
            return;
        }

        hitFlashTimer += Time.deltaTime;

        float tIn = hitFlashInTime <= 0f ? 0.0001f : hitFlashInTime;
        float tOut = hitFlashOutTime <= 0f ? 0.0001f : hitFlashOutTime;

        float intensity;
        if (hitFlashTimer <= tIn)
        {
            float a = hitFlashTimer / tIn;
            intensity = Mathf.Lerp(0f, hitFlashPeak, a);
        }
        else
        {
            float a = (hitFlashTimer - tIn) / tOut;
            intensity = Mathf.Lerp(hitFlashPeak, 0f, a);

            if (a >= 1f)
            {
                intensity = 0f;
                hitFlashActive = false;
            }
        }

        hitFlashLight.intensity = intensity;
    }
    private void TriggerHitFlash()
    {
        if (hitFlashLight == null) return;
        if (hitFlashChance < 1f && UnityEngine.Random.value > hitFlashChance) return;

        hitFlashPeak = UnityEngine.Random.Range(hitFlashMinIntensity, hitFlashMaxIntensity);
        hitFlashTimer = 0f;
        hitFlashActive = true;
    }
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
            TriggerHitFlash();

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
