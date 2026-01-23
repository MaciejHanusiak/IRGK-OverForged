using UnityEngine;
using UnityEngine.InputSystem;

public class LocalCoopJoinManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager pim;

    [Header("UI (single shared UI)")]
    [SerializeField] private SelectedUI sharedSelectedUI;
    [SerializeField] private InputPanelUI inputPanelUI;

    private Player lastPrimaryBound;
    private bool didInitialBind = false;

    [SerializeField] private SelectedUI selectedUI_P1; // mo¿esz tu daæ to co masz jako sharedSelectedUI
    [SerializeField] private SelectedUI selectedUI_P2;
    private void Awake()
    {
        if (pim == null) pim = FindFirstObjectByType<PlayerInputManager>();
        pim.enabled = GameManager.Instance.isMultiplayerSelected;
    }
    private void Start()
    {
        // Obs³uga gracza, który jest ju¿ w scenie od pocz¹tku (nie przeszed³ przez onPlayerJoined)

        UpdateMultiplayerUI();
    }
    private void Update()
    {
        Debug.Log("[LocalCoopJoinManager] Update tick");
        if (didInitialBind) return;

        // Czekamy a¿ pojawi siê pierwszy Player w scenie
        var anyPlayer = FindFirstObjectByType<Player>();
        if (anyPlayer == null) return;

        // Teraz ju¿ ma sens
        TryBindPrimaryPlayer();
        BindAllPlayersToSelectedCounterVisuals();
        BindAllPlayersToWeaponStandUI();
        BindSelectedUIPanels();
        UpdateMultiplayerUI();

        didInitialBind = true;

        Debug.Log("[LocalCoopJoinManager] Initial bind done (players found).");
    }
    private void OnEnable()
    {
        if (pim != null)
        {
            pim.onPlayerJoined += OnPlayerJoined;
            pim.onPlayerLeft += OnPlayerLeft;
        }
    }

    private void OnDisable()
    {
        if (pim != null)
        {
            pim.onPlayerJoined -= OnPlayerJoined;
            pim.onPlayerLeft -= OnPlayerLeft;
        }
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        TryBindPrimaryPlayer();
        BindAllPlayersToSelectedCounterVisuals();
        BindAllPlayersToWeaponStandUI();
        BindSelectedUIPanels();
        UpdateMultiplayerUI();
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        // Jak ktoœ wyszed³: odœwie¿ UI
        BindAllPlayersToSelectedCounterVisuals();
        BindAllPlayersToWeaponStandUI();
        UpdateMultiplayerUI();
    }
    private void TryBindPrimaryPlayer()
    {
        // Primary = playerIndex 0, a jak go nie ma, to pierwszy znaleziony Player
        Player primary = null;

        foreach (var p in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            var pi = p.GetComponent<PlayerInput>();
            if (pi != null && pi.playerIndex == 0)
            {
                primary = p;
                break;
            }
        }

        if (primary == null)
            primary = FindFirstObjectByType<Player>();

        if (primary == null)
        {
            Debug.LogWarning("[LocalCoopJoinManager] TryBindPrimaryPlayer: no Player found");
            return;
        }

        Debug.Log($"[LocalCoopJoinManager] TryBindPrimaryPlayer -> primary = {primary.name}");



        // 1) sharedSelectedUI (wspólny panel) - binduj tylko jeœli primary siê zmieni³
        if (sharedSelectedUI != null && primary != lastPrimaryBound)
        {
            lastPrimaryBound = primary;
            Debug.Log($"[LocalCoopJoinManager] Binding sharedSelectedUI {sharedSelectedUI.name} to {primary.name}");
            sharedSelectedUI.Bind(primary);
        }
    }
    private void BindAllPlayersToSelectedCounterVisuals()
    {
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        var visuals = FindObjectsByType<SelectedCounterVisual>(FindObjectsSortMode.None);

        foreach (var vis in visuals)
        {
            foreach (var p in players)
            {
                vis.Bind(p);
            }
        }
    }

    private void BindAllPlayersToWeaponStandUI()
    {
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        var iconsUIs = FindObjectsByType<WeaponStandIconsUI>(FindObjectsSortMode.None);

        foreach (var iconsUI in iconsUIs)
        {
            foreach (var p in players)
            {
                iconsUI.Bind(p);
            }
        }
    }
    private void BindSelectedUIPanels()
    {
        Debug.Log("[LocalCoopJoinManager] BindSelectedUIPanels CALLED");
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);

        Player p1 = null;
        Player p2 = null;

        foreach (var p in players)
        {
            if (p.PlayerIndex == 0) p1 = p;
            else if (p.PlayerIndex == 1) p2 = p;
        }

        // P1
        if (sharedSelectedUI != null)
        {
            if (p1 != null) sharedSelectedUI.Bind(p1);
            else sharedSelectedUI.HidePanel(); // jeœli masz publiczne HidePanel
        }

        // P2
        if (selectedUI_P2 != null)
        {
            if (p2 != null)
            {
                selectedUI_P2.gameObject.SetActive(true);
                selectedUI_P2.Bind(p2);
            }
            else
            {
                selectedUI_P2.gameObject.SetActive(false);
            }
        }
        Debug.Log(p1 + "p1,            p2," + p2);
    }
    private void UpdateMultiplayerUI()
    {
        int playerCount = PlayerInput.all.Count;

        bool isMultiplayer = playerCount >= 2;

        inputPanelUI.PlayerTwoJoinPanel.gameObject.SetActive(!isMultiplayer);
        inputPanelUI.PlayerTwoPanel.gameObject.SetActive(isMultiplayer);
    }


}
