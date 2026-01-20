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
        var player = playerInput.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Spawned player prefab has no Player component.");
            return;
        }

        // 1) Jeœli masz jedno UI na ekran (obs³uguje tylko P1):
        if (sharedSelectedUI != null && player.PlayerIndex == 0)
            sharedSelectedUI.Bind(player);

        // 2) Bind wszystkich SelectedCounterVisual w scenie do tego gracza
        //    (dla 2 graczy oba bêd¹ dzia³aæ równolegle, bo ka¿dy visual ma swoje visualP1/visualP2)
        foreach (var vis in FindObjectsByType<SelectedCounterVisual>(FindObjectsSortMode.None))
            vis.Bind(player);
    }
}
