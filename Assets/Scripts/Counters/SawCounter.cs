using System;
using UnityEngine;

public class SawCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public enum State
    {
        Idle,
        Cutting,
        Cutted,
        OverCutted,
    }

    [Header("VFX")]
    [SerializeField] private GameObject cuttingVfxRoot;
    [SerializeField] private GameObject ocerCuttingVfxRoot;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    [SerializeField] private OverCuttingRecipeSO[] overCuttingRecipeSOArray;

    [Header("SFX (Loop)")]
    [SerializeField] private AudioSource loopSource;     // AudioSource na tym samym obiekcie (pile)
    [SerializeField] private AudioClip cuttingLoopA;    // State.Cutting
    [SerializeField] private AudioClip doneLoopB;        // State.Cutted + State.Overcutted

    private State lastState;

    private State state;
    private float cuttingTimer;
    private float overCuttingTimer;
    private CuttingRecipeSO cuttingRecipeSO;
    private OverCuttingRecipeSO overCuttingRecipeSO;

    [SerializeField] private Animator animator;

    private ParticleSystem[] cuttingVfxSystems;
    private ParticleSystem[] overCuttingVfxSystems;

    private void Start()
    {
        animator = GetComponent<Animator>();
        state = State.Idle;
        ApplyAudioState();
        if (cuttingVfxRoot != null && ocerCuttingVfxRoot != null)
        {
            cuttingVfxSystems = cuttingVfxRoot.GetComponentsInChildren<ParticleSystem>(true);
            overCuttingVfxSystems = ocerCuttingVfxRoot.GetComponentsInChildren<ParticleSystem>(true);
        }
        ApplyVfxState();
    }
    private void Update()
    {
        animator.SetInteger("State", (int)state);
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

                        Analytics.Instance.PlayerCutSomething(cuttingRecipeSO.input.objectName,
                            cuttingTimer,
                            cuttingRecipeSO.cuttingTimerMax,
                            LevelStats.Instance.gold,
                            LevelTime.Instance.timeRemaining);

                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());
                        overCuttingTimer = 0f;
                    }
                    break;

                case State.Cutted:
                    overCuttingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = overCuttingTimer / overCuttingRecipeSO.overCuttingTimeMax,
                        sawState = State.Cutted
                    });

                    if (overCuttingTimer > overCuttingRecipeSO.overCuttingTimeMax)
                    {

                        // overCutted

                        GetSmithObject().DestroySelf();
                        SmithObject.SpawnSmithObject(overCuttingRecipeSO.output, this);
                        Debug.Log("ObjectOverCutted!");

                                            Analytics.Instance.PlayerOvercutSomething(overCuttingRecipeSO.input.objectName,
                        overCuttingTimer,
                        overCuttingRecipeSO.overCuttingTimeMax,
                        LevelStats.Instance.gold,
                        LevelTime.Instance.timeRemaining);

                        overCuttingTimer = 0f;
                        overCuttingRecipeSO = GetOverCuttingRecipeSOWithInput(GetSmithObject().GetSmithObjectSO());

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                        state = State.OverCutted;
                    }
                    break;

                case State.OverCutted:
                    break;
            }
        }
        ApplyVfxState();

        if (state != lastState)
        {
            lastState = state;
            ApplyAudioState();
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
                cuttingTimer = 0f;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = cuttingTimer / cuttingRecipeSO.cuttingTimerMax
                });
            }
        }
    }

    private void ApplyAudioState()
    {
        if (loopSource == null) return;

        AudioClip target = null;

        switch (state)
        {
            case State.Cutting:
                target = cuttingLoopA;
                break;
            case State.Cutted:
            case State.OverCutted:
                target = doneLoopB;
                break;
            case State.Idle:
            default:
                target = null;
                break;
        }

        // Nic siê nie zmienia -> nie ruszaj AudioSource
        if (loopSource.clip == target)
        {
            // jeœli target null, upewnij siê ¿e nie gra
            if (target == null && loopSource.isPlaying) loopSource.Stop();
            return;
        }

        // Zmiana klipu/stanu
        loopSource.Stop();
        loopSource.clip = target;

        if (target != null)
            loopSource.Play();
    }
    private void ApplyVfxState()
    {
        // Idle => off
        bool forgeingOn = (state == State.Cutting);
        bool forgedOn = (state == State.Cutted || state == State.OverCutted);

        SetGroupPlaying(cuttingVfxSystems, forgeingOn);
        SetGroupPlaying(overCuttingVfxSystems, forgedOn);
    }
    private void SetGroupPlaying(ParticleSystem[] group, bool shouldPlay)
    {
        if (group == null) return;

        for (int i = 0; i < group.Length; i++)
        {
            var ps = group[i];
            if (ps == null) continue;

            if (shouldPlay)
            {
                if (!ps.isPlaying)
                {

                    ps.Play();
                    Debug.Log("efekty robi¹ ziuuu");
                }
            }
            else
            {
                if (ps.isPlaying) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
