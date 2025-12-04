using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;

public class MainManuManagement : MonoBehaviour
{
    [SerializeField] Image AnalyticsPanel;

    private void Start()
    {
        if (AnalyticsPanel != null && PlayerPrefs.GetInt("AnalyticsSet") == 0)
        {
            AnalyticsPanel.gameObject.SetActive(true);
            AnalyticsService.Instance.StartDataCollection();

        }

    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Analytics.Instance.EndGame();
    }

    public void AgreeOnAnalytics()
    {
        PlayerPrefs.SetInt("AnalyticsSet", 1);
        PlayerPrefs.Save();
        AnalyticsPanel.gameObject.SetActive(false);

    }
    public void DontAgreeOnAnalytics()
    {
        Analytics.Instance.DisagreedForAnalytics();
        AnalyticsService.Instance.StopDataCollection();
        PlayerPrefs.SetInt("AnalyticsSet", 0);
        PlayerPrefs.Save();
        AnalyticsPanel.gameObject.SetActive(false);

    }

}
