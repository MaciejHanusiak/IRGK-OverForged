using System.Collections.Generic;
using UnityEngine;

public class WeaponStandIconsUI : MonoBehaviour
{
    [SerializeField] private WeaponStandSmithObject weaponStandSmithObject;
    [SerializeField] private Transform iconTemplate;

    // Multi-bind: panel reaguje na dowoln¹ liczbê graczy
    private readonly HashSet<Player> boundPlayers = new HashSet<Player>();

    // Którzy gracze AKTUALNIE wskazuj¹ ten WeaponStand
    private readonly HashSet<Player> playersSelectingThis = new HashSet<Player>();

    private void Awake()
    {
        Debug.Log($"[WeaponStandIconsUI] Awake on {gameObject.name}");

        if (iconTemplate != null)
        {
            Debug.Log("[WeaponStandIconsUI] iconTemplate found, disabling it");
            iconTemplate.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[WeaponStandIconsUI] iconTemplate == NULL!");
        }

        HideUI();
    }
    private void Start()
    {
        Debug.Log($"[WeaponStandIconsUI] Start on {gameObject.name}");

        if (weaponStandSmithObject != null)
        {
            Debug.Log("[WeaponStandIconsUI] Subscribed to weaponStandSmithObject.OnWeaponPartAdded");
            weaponStandSmithObject.OnWeaponPartAdded += WeaponStandSmithObject_OnWeaponPartAdded;
        }
        else
        {
            Debug.LogWarning("[WeaponStandIconsUI] weaponStandSmithObject == NULL!");
        }

        // UWAGA: nie robimy AutoBind tutaj (bo w multi jest losowo i psuje logikê)
        // Bindowanie robi LocalCoopJoinManager dla wszystkich graczy.
    }
    public void Update()
    {
        // logujemy rzadko, ¿eby nie zaspamowaæ
    }
    public void Bind(Player p)
    {
        if (p == null) return;
        if (boundPlayers.Contains(p)) return;

        boundPlayers.Add(p);
        p.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

        Debug.Log($"[WeaponStandIconsUI] Bound player {p.name} (idx={p.PlayerIndex})");
    }
    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        Debug.Log($"[WeaponStandIconsUI] Instance_OnSelectedCounterChanged e.selectedCounter = {e.selectedCounter}");

        if (e.selectedCounter != null && e.selectedCounter.GetSmithObject() == weaponStandSmithObject)
        {
            Debug.Log("[WeaponStandIconsUI] Instance: MATCH -> ShowUI()");
            ShowUI();
        }
        else
        {
            Debug.Log("[WeaponStandIconsUI] Instance: NO MATCH -> HideUI()");
            HideUI();
        }
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        var p = sender as Player;
        if (p == null) return;

        bool isSelectingThis =
            e.selectedCounter != null &&
            weaponStandSmithObject != null &&
            e.selectedCounter.GetSmithObject() == weaponStandSmithObject;

        if (isSelectingThis)
            playersSelectingThis.Add(p);
        else
            playersSelectingThis.Remove(p);

        UpdateVisibility();
    }
    private void UpdateVisibility()
    {
        if (playersSelectingThis.Count > 0) ShowUI();
        else HideUI();
    }
    private void WeaponStandSmithObject_OnWeaponPartAdded(object sender, WeaponStandSmithObject.OnWeaponPartAddedEventArgs e)
    {
        Debug.Log("[WeaponStandIconsUI] EVENT: WeaponPartAdded -> UpdateVisual()");

        // Jeœli UI jest widoczne (ktoœ patrzy), odœwie¿amy ikony od razu.
        // Jeœli nikt nie patrzy, nie ma sensu generowaæ ikon na ukrytym panelu.
        if (playersSelectingThis.Count > 0)
            UpdateVisual();
    }
    private void UpdateVisual()
    {
        Debug.Log("[WeaponStandIconsUI] UpdateVisual() called");

        if (weaponStandSmithObject == null)
        {
            Debug.LogWarning("[WeaponStandIconsUI] UpdateVisual skipped -> weaponStandSmithObject == NULL");
            return;
        }

        if (iconTemplate == null)
        {
            Debug.LogWarning("[WeaponStandIconsUI] UpdateVisual skipped -> iconTemplate == NULL");
            return;
        }

        // Clear old icons
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // Create new icons
        foreach (SmithObjectSO smithObjectSO in weaponStandSmithObject.GetSmithObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<WeaponStandSingleIconUI>().SetSmithObjectSO(smithObjectSO);
        }
    }
    public void ShowUI()
    {
        // Najpierw zbuduj aktualny stan ikon
        UpdateVisual();

        // Poka¿ wszystko poza template
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            child.gameObject.SetActive(true);
        }
    }
    public void HideUI()
    {
        // Schowaj wszystko poza template
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            child.gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (weaponStandSmithObject != null)
            weaponStandSmithObject.OnWeaponPartAdded -= WeaponStandSmithObject_OnWeaponPartAdded;

        foreach (var p in boundPlayers)
            if (p != null)
                p.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;

        boundPlayers.Clear();
        playersSelectingThis.Clear();
    }

}
