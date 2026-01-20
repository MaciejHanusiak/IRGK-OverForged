using UnityEngine;

public class WeaponStandIconsUI : MonoBehaviour
{
    [SerializeField] private WeaponStandSmithObject weaponStandSmithObject;
    [SerializeField] private Transform iconTemplate;

    private Player player;

    private void Awake()
    {
        if (iconTemplate != null)
            iconTemplate.gameObject.SetActive(false);

        HideUI();
    }
    private void Start()
    {
        if (weaponStandSmithObject != null)
            weaponStandSmithObject.OnWeaponPartAdded += WeaponStandSmithObject_OnWeaponPartAdded;
    }

    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
 
        if (e.selectedCounter != null && e.selectedCounter.GetSmithObject() == weaponStandSmithObject)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }


    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter != null && e.selectedCounter.GetSmithObject() == weaponStandSmithObject)
            ShowUI();
        else
            HideUI();
    }
    public void Bind(Player p)
    {
        if (player != null) return;

        if (p.PlayerIndex != 0) return;
        
        if(player != null)
        player.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;

        player = p;

        if (player != null)
            player.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

        HideUI();
    }
    private void OnDestroy()
    {
        if (weaponStandSmithObject != null)
            weaponStandSmithObject.OnWeaponPartAdded -= WeaponStandSmithObject_OnWeaponPartAdded;

        if (player != null)
            player.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
    }
    private void WeaponStandSmithObject_OnWeaponPartAdded(object sender, WeaponStandSmithObject.OnWeaponPartAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (weaponStandSmithObject == null || iconTemplate == null) return;
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (SmithObjectSO smithObjectSO in weaponStandSmithObject.GetSmithObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<WeaponStandSingleIconUI>().SetSmithObjectSO(smithObjectSO);
        }
    }

    public void ShowUI()
    {
        if (weaponStandSmithObject != null)
            transform.gameObject.SetActive(true);
        UpdateVisual();
    }
    public void HideUI()
    {
        if (weaponStandSmithObject != null) 
        transform.gameObject.SetActive(false);
    }
}
