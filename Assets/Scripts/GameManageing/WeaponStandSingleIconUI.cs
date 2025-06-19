using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStandSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text objectName;

    public void SetSmithObjectSO(SmithObjectSO smithObjectSO)
    {
        image.sprite = smithObjectSO.sprite;
        objectName.text = smithObjectSO.objectName;
    }

}
