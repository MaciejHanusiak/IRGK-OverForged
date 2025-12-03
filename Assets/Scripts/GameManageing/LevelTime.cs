using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelTime : TimerSliderUI
{
    [SerializeField] private float maxTime = 60f;
    public float timeRemaining;

    public static LevelTime Instance;               // }
                                                    // }
                                                    // }
    public void Awake()                             // }
    {                                               // }
        if (Instance == null) Instance = this;      // }
        else Destroy(gameObject);                   // }  Wzorzec Singleton
        //Analytics.Instance.GoldChanged(gold,);

    }

    void Start()
    {

        timeRemaining = maxTime;
        if (timerSlider)
        {
            timerSlider.maxValue = maxTime;
            timerSlider.value = timeRemaining;
        }
        
    }

    
    override protected void Update()
    {
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0;

        }

        base.Update();
        
        // Koniec czasu
        if (timeRemaining <= 0 && !IsExpired())
        {
            //Analytics.Instance.EndLevelTime(SceneManager.GetActiveScene().buildIndex, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            Debug.Log("Koniec gry!");
            // gameover - to ju¿ chyba nie dzia³a
        }
    }

    protected override float GetRemainingTime() => timeRemaining;
    protected override float GetTotalTime() => maxTime;
    protected override bool IsExpired() => timeRemaining <= 0;

    protected override string GetLabelText()
    {
        int seconds = Mathf.FloorToInt(timeRemaining);
        int milliseconds = Mathf.FloorToInt((timeRemaining - seconds) * 100);
        return $"{seconds:D2}:{milliseconds:D2}";
    }


   
}
