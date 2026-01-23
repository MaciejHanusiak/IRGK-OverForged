using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CameraMovement : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float speed = 5f;

    [Header("Auto source")]
    [SerializeField] private PlayerInputManager pim;

    [Header("Pixel Perfect")]
    [SerializeField] private PixelPerfectCamera ppc;
    [SerializeField] private int multiplayerRefResX = 360;
    [SerializeField] private int multiplayerRefResY = 180;

    private int defaultRefResX;

    private Transform p1;
    private Transform p2;

    private void Awake()
    {
        if (pim == null) pim = FindFirstObjectByType<PlayerInputManager>();
        if (ppc == null)
            ppc = Camera.main.GetComponent<PixelPerfectCamera>();

        if (ppc != null)
            defaultRefResX = ppc.refResolutionX;
    }

    private void OnEnable()
    {
        if (pim != null)
        {
            pim.onPlayerJoined += OnPlayerJoined;
            pim.onPlayerLeft += OnPlayerLeft;
        }

        RefreshPlayers(); // wa¿ne jeœli P1 ju¿ jest na scenie
    }

    private void OnDisable()
    {
        if (pim != null)
        {
            pim.onPlayerJoined -= OnPlayerJoined;
            pim.onPlayerLeft -= OnPlayerLeft;
        }
    }

    private void OnPlayerJoined(PlayerInput _)
    {
        RefreshPlayers(); // po join P2 -> prze³¹czy w multi
        UpdatePixelPerfect();
    }

    private void OnPlayerLeft(PlayerInput _)
    {
        RefreshPlayers(); // po leave -> wróci do solo
        UpdatePixelPerfect();
    }

    private void RefreshPlayers()
    {
        // PlayerInput.all jest Ÿród³em prawdy (kolejnoœæ = playerIndex)
        p1 = null;
        p2 = null;

        foreach (var pi in PlayerInput.all)
        {
            if (pi == null) continue;

            if (pi.playerIndex == 0) p1 = pi.transform;
            else if (pi.playerIndex == 1) p2 = pi.transform;
        }

        // Fallback, gdyby coœ by³o nietypowe:
        if (p1 == null)
        {
            var anyPlayer = FindFirstObjectByType<Player>();
            if (anyPlayer != null) p1 = anyPlayer.transform;
        }
    }
    private void UpdatePixelPerfect()
    {
        if (ppc == null) return;

        bool isMultiplayer = (p2 != null);

        if (isMultiplayer)
        {
            ppc.refResolutionX = multiplayerRefResX;
            ppc.refResolutionY = multiplayerRefResY;
        }
        else
        {
            ppc.refResolutionX = defaultRefResX;
        }
    }

    private void Update()
    {
        if (p1 == null) return;

        Vector3 target;

        // MULTI: œrodek pomiêdzy p1 i p2
        if (p2 != null)
            target = (p1.position + p2.position) * 0.5f;
        else
            target = p1.position;

        target += offset;

        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }
}