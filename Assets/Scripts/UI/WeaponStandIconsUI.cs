using UnityEngine;

public class WeaponStandIconsUI : MonoBehaviour
{
    [SerializeField] private WeaponStandSmithObject weaponStandSmithObject;
    [SerializeField] private Transform iconTemplate;


    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        weaponStandSmithObject.OnWeaponPartAdded += WeaponStandSmithObject_OnWeaponPartAdded;
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
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

    private void WeaponStandSmithObject_OnWeaponPartAdded(object sender, WeaponStandSmithObject.OnWeaponPartAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
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
    }
    public void HideUI()
    {
        if (weaponStandSmithObject != null) 
        transform.gameObject.SetActive(false);
    }
}
