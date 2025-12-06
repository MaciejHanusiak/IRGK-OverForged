using UnityEngine;
using TMPro;

public class SavedPlayerPrefs : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI analytics;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("AnalyticsSet") == 0)
            analytics.text = "Off";
        else
            analytics.text = "On";
    }
}
