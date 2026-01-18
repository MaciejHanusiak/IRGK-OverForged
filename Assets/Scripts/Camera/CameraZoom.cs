using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraZoom: MonoBehaviour
{
    [SerializeField] private PixelPerfectCamera ppc;

    [Header("Zoom levels: smaller refRes = more zoom in")]
    [SerializeField]
    private Vector2Int[] zoomLevels = new Vector2Int[]
    {
        new Vector2Int(80, 45),  // zoom out (bardziej oddalone)
        new Vector2Int(107, 53), // default
        new Vector2Int(160, 80),   // zoom in (bardziej przybli¿one)
        new Vector2Int(240, 120),  
    };

    [SerializeField] private int startIndex = 1;
    [SerializeField] private float scrollDeadzone = 0.05f;

    private int idx;

    private void Awake()
    {
        if (ppc == null) ppc = GetComponent<PixelPerfectCamera>();
        idx = Mathf.Clamp(startIndex, 0, zoomLevels.Length - 1);
        Apply();
    }

    private void Update()
    {
        float s = Input.mouseScrollDelta.y;
        if (Mathf.Abs(s) < scrollDeadzone) return;

        // scroll up = zoom in
        if (s > 0f) idx = Mathf.Max(0, idx - 1);
        else idx = Mathf.Min(zoomLevels.Length - 1, idx + 1);

        Apply();
    }

    private void Apply()
    {
        var v = zoomLevels[idx];
        ppc.refResolutionX = v.x;
        ppc.refResolutionY = v.y;
    }
}