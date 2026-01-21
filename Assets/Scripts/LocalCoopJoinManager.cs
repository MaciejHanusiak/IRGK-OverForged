using UnityEngine;
using UnityEngine.InputSystem;

public class LocalCoopJoinManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager pim;

    [Header("UI (single shared UI)")]
    [SerializeField] private SelectedUI sharedSelectedUI;


    private void Awake()
    {
        if (pim == null) pim = FindFirstObjectByType<PlayerInputManager>();
    }
    private void Start()
    {
        // Obs³u¿ gracza, który jest ju¿ w scenie od pocz¹tku (nie przeszed³ przez onPlayerJoined)
        TryBindPrimaryPlayer();
    }

    private void OnEnable()
    {
        if (pim != null) pim.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (pim != null) pim.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Po do³¹czeniu nowego gracza: nadal binduj tylko "primary" (P1)
        TryBindPrimaryPlayer();
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

        if (primary == null) return;

        foreach (var vis in FindObjectsByType<SelectedCounterVisual>(FindObjectsSortMode.None))
            vis.Bind(primary);

        if (sharedSelectedUI != null)
            sharedSelectedUI.Bind(primary);
    }
    public bool IsKeyboardPlayer
    {
        get
        {
            var pi = GetComponent<PlayerInput>();
            if (pi == null) return false;

            foreach (var d in pi.devices)
                if (d is Keyboard) return true;

            return false;
        }
    }
}
