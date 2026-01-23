using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private PlayerInputManager pim;

    [Header("Pixel Perfect")]
    [SerializeField] private PixelPerfectCamera ppc;

    [Header("Follow")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float followSmoothTime = 0.12f;

    [Header("Zoom (ORTHO SIZE)")]
    [SerializeField] private float minOrthoSize = 5f;     // najbli¿ej (zoom in)
    [SerializeField] private float maxOrthoSize = 10f;    // najdalej (zoom out)
    [SerializeField] private float zoomSmoothTime = 0.18f;

    [Header("Distance -> Zoom mapping (Unity units)")]
    [SerializeField] private float minDistance = 2f;      // dystans => minOrthoSize
    [SerializeField] private float maxDistance = 12f;     // dystans => maxOrthoSize

    [Header("Hybrid anti-shimmer")]
    [Tooltip("Snapuje pozycjê kamery do siatki pikseli (zmniejsza shimmer, ale mo¿e dodaæ 'mikro-klik').")]
    [SerializeField] private bool snapToPixelGrid = true;

    [Tooltip("Dodatkowy mno¿nik: jeœli nadal p³ywa, ustaw 2 (snap co 2 piksele), jak zbyt klika, ustaw 1.")]
    [SerializeField] private int snapPixelStep = 1;

    private Transform p1, p2;
    private Camera cam;

    private Vector3 followVelocity;
    private float zoomVelocity;

    private void Awake()
    {
        if (pim == null) pim = FindFirstObjectByType<PlayerInputManager>();

        cam = GetComponent<Camera>();
        cam.orthographic = true;

        if (ppc == null) ppc = GetComponent<PixelPerfectCamera>();
        if (ppc == null && Camera.main != null) ppc = Camera.main.GetComponent<PixelPerfectCamera>();

        snapPixelStep = Mathf.Max(1, snapPixelStep);
        if (GameManager.Instance.isMultiplayerSelected)
            ppc.enabled = false;
    }

    private void OnEnable()
    {
        if (pim != null)
        {
            pim.onPlayerJoined += OnPlayerJoined;
            pim.onPlayerLeft += OnPlayerLeft;
        }

        RefreshPlayers();
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
        RefreshPlayers();
    }

    private void OnPlayerLeft(PlayerInput _)
    {
        RefreshPlayers();
    }

    private void RefreshPlayers()
    {
        p1 = null;
        p2 = null;

        foreach (var pi in PlayerInput.all)
        {
            if (pi == null) continue;

            if (pi.playerIndex == 0) p1 = pi.transform;
            else if (pi.playerIndex == 1) p2 = pi.transform;
        }
    }

    private void LateUpdate()
    {
        if (p1 == null) return;

        // --- FOLLOW (smooth) ---
        Vector3 center = (p2 != null) ? (p1.position + p2.position) * 0.5f : p1.position;
        center.z = 0f; // stabilne 2D

        Vector3 desiredPos = center + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref followVelocity,
            followSmoothTime
        );

        // --- ZOOM (smooth) ---
        float targetSize = minOrthoSize;

        if (p2 != null)
        {
            float d = Vector2.Distance(p1.position, p2.position);
            float t = Mathf.InverseLerp(minDistance, maxDistance, d);
            targetSize = Mathf.Lerp(minOrthoSize, maxOrthoSize, t);
        }

        cam.orthographicSize = Mathf.SmoothDamp(
            cam.orthographicSize,
            targetSize,
            ref zoomVelocity,
            zoomSmoothTime
        );

        // --- HYBRID: snap camera position to pixel grid ---
        if (snapToPixelGrid)
            SnapCameraToPixelGrid();
    }

    private void SnapCameraToPixelGrid()
    {
        // Jeœli nie ma PixelPerfectCamera, nie mamy jak policzyæ units-per-pixel sensownie.
        if (ppc == null) return;

        // assetsPPU: ile pikseli ma 1 unit w œwiecie (Twoje PPU)
        float assetsPPU = ppc.assetsPPU;
        if (assetsPPU <= 0f) return;

        // pixelRatio: aktualny integer scale (np. 5)
        int ratio = ppc.pixelRatio;
        if (ratio <= 0) return;

        // 1 pixel w œwiecie = 1 / (assetsPPU * ratio) unity
        float unitsPerPixel = 1f / (assetsPPU * ratio);

        // Snap co N pikseli (snapPixelStep)
        float step = unitsPerPixel * snapPixelStep;

        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x / step) * step;
        pos.y = Mathf.Round(pos.y / step) * step;
        // z zostaje z offsetu (-10)

        transform.position = pos;
    }
}
