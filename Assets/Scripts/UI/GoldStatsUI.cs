using UnityEngine;
using TMPro;

public class GoldStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmountText;

    void Update()
    {
        goldAmountText.text =  LevelStats.Instance.gold.ToString();
    }
}
