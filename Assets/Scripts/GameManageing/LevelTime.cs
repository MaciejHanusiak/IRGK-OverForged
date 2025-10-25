using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelTime : MonoBehaviour
{
    public float timeRemaining = 60f;
    public float maxTime = 60f;
    private bool timerIsRunning = false;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider timerSlider;
    
    void Start()
    {
        timerIsRunning = true;

        if (timerSlider != null)
        {
            timerSlider.maxValue = maxTime;
            timerSlider.value = timeRemaining;
        }
        UpdateTimerUI();
    }

    
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerUI();
                Debug.Log("Koniec gry");
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.FloorToInt(timeRemaining);
            int milliseconds = Mathf.FloorToInt((timeRemaining - seconds) * 1000);
            timerText.text = string.Format("{0}", LevelStats.Instance.gold);
        }
        if (timerSlider != null)
        {
            timerSlider.value = timeRemaining;
        }
    }
}
