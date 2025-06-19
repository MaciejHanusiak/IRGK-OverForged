using UnityEngine;
using UnityEngine.UI;

public class WeaponStandSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetSmithObjectSO(SmithObjectSO smithObjectSO)
    {
        image.sprite = smithObjectSO.sprite;
    }

}
