using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class InputPanelUI : MonoBehaviour
{
    [SerializeField] public Transform SinglePlayerControlsPanel;
    [SerializeField] public Transform MultiPlayerControlsPanel;
    [SerializeField] public Transform SmallInputPanel;

    [SerializeField] public Transform PlayerTwoJoinPanel;
    [SerializeField] public Transform PlayerTwoPanel;

    [SerializeField] public TextMeshProUGUI SmallPanelText;

    [SerializeField] private float moveSpeed = 8f;

    private RectTransform smallRT;
    private Vector2 targetOffsetMin;
    private Vector2 targetOffsetMax;

    private bool isExpanded = false;

    void Awake()
    {
        smallRT = SmallInputPanel.GetComponent<RectTransform>();

        smallRT.anchorMin = Vector2.zero;
        smallRT.anchorMax = Vector2.one;

        // startowa pozycja = DOWN
        SetTargetDown();
        ApplyInstant();
    }
    private void ApplyInstant()
    {
        smallRT.offsetMin = targetOffsetMin;
        smallRT.offsetMax = targetOffsetMax;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SinglePlayerControlsPanel.gameObject.SetActive(false);
        MultiPlayerControlsPanel.gameObject.SetActive(false);
        SmallInputPanel.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (isExpanded)
            SmallPanelText.text = "to hide controls";
        else SmallPanelText.text = "to show controls";
            // animacja
            smallRT.offsetMin = Vector2.Lerp(
                smallRT.offsetMin,
                targetOffsetMin,
                Time.deltaTime * moveSpeed);

        smallRT.offsetMax = Vector2.Lerp(
            smallRT.offsetMax,
            targetOffsetMax,
            Time.deltaTime * moveSpeed);

        // input
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (!GameManager.Instance.isMultiplayerSelected)
            ToggleSingle();
        else
            ToggleMulti();
    }
    private void ToggleSingle()
    {
        if (!isExpanded)
        {
            SetTargetSingleUp();
            SinglePlayerControlsPanel.gameObject.SetActive(true);
            isExpanded = true;
        }
        else
        {
            SetTargetDown();
            SinglePlayerControlsPanel.gameObject.SetActive(false);
            isExpanded = false;
        }
    }

    private void ToggleMulti()
    {
        if (!isExpanded)
        {
            SetTargetMultiUp();
            MultiPlayerControlsPanel.gameObject.SetActive(true);
            isExpanded = true;
        }
        else
        {
            SetTargetDown();
            MultiPlayerControlsPanel.gameObject.SetActive(false);
            isExpanded = false;
        }
    }
    private void SetTargetSingleUp()
    {
        targetOffsetMin = new Vector2(400f, 120f);
        targetOffsetMax = new Vector2(-400f, -40f);
    }

    private void SetTargetMultiUp()
    {
        targetOffsetMin = new Vector2(400f, 250f);
        targetOffsetMax = new Vector2(-400f, 90f);
    }

    private void SetTargetDown()
    {
        targetOffsetMin = new Vector2(400f, 0f);
        targetOffsetMax = new Vector2(-400f, -180f);
    }
}
