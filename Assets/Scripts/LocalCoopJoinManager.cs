using UnityEngine;
using UnityEngine.InputSystem;

public class LocalCoopJoinManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager pim;

    [Header("UI (single shared UI)")]
    [SerializeField] private SelectedUI sharedSelectedUI;
    [SerializeField] private InputPanelUI inputPanelUI;

    private void Awake()
    {
        if (pim == null) pim = FindFirstObjectByType<PlayerInputManager>();
        pim.enabled = GameManager.Instance.isMultiplayerSelected;
    }
    private void Start()
    {
        // Obs³uga gracza, który jest ju¿ w scenie od pocz¹tku (nie przeszed³ przez onPlayerJoined)
        TryBindPrimaryPlayer();
        BindAllPlayersToWeaponStandUI();
        UpdateMultiplayerUI();
        InvokeRepeating(nameof(RebindUI), 0f, 0.25f);
    }
    private void RebindUI()
    {
        TryBindPrimaryPlayer();
        BindAllPlayersToWeaponStandUI();
        UpdateMultiplayerUI();
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
        CancelInvoke(nameof(RebindUI));
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Nadal mo¿na robiæ bind "primary" pod SelectedCounterVisual i sharedSelectedUI,
        // ale WeaponStandIconsUI bindowane jest do WSZYSTKICH graczy.
        TryBindPrimaryPlayer();
        BindAllPlayersToWeaponStandUI();
        UpdateMultiplayerUI();
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        // Jak ktoœ wyszed³: odœwie¿ UI
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

        // 1) SelectedCounterVisual (jeœli to masz jako wspólne / P1 logic)
        foreach (var vis in FindObjectsByType<SelectedCounterVisual>(FindObjectsSortMode.None))
        {
            Debug.Log($"[LocalCoopJoinManager] Binding SelectedCounterVisual {vis.name} to {primary.name}");
            vis.Bind(primary);
        }

        // 2) sharedSelectedUI (wspólny panel)
        if (sharedSelectedUI != null)
        {
            Debug.Log($"[LocalCoopJoinManager] Binding sharedSelectedUI {sharedSelectedUI.name} to {primary.name}");
            sharedSelectedUI.Bind(primary);
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
    private void UpdateMultiplayerUI()
    {
        int playerCount = PlayerInput.all.Count;

        bool isMultiplayer = playerCount >= 2;

        inputPanelUI.PlayerTwoJoinPanel.gameObject.SetActive(!isMultiplayer);
        inputPanelUI.PlayerTwoPanel.gameObject.SetActive(isMultiplayer);
    }


}
