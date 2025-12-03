using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] public GameObject levelEndPanel;
    [SerializeField] public GameObject nextLevelButton;
    [SerializeField] public TextMeshProUGUI endLevelStatement;
    [SerializeField] public int levelGoldGoal;
    private float normalFixedDeltaTime;

    private bool hasLevelEnded = false;

    private void Awake()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime; // Save default value
        endLevelStatement.text = "";
    }

    void Update()
    {
        // If level ended do nothing
        if (hasLevelEnded) return;
        endLevelStatement.text = LevelStats.Instance.gold >= levelGoldGoal ? "Congrats! You won this lvl!" : 
            LevelTime.Instance.timeRemaining <= 0 ? "Game over! :<" : "";
        if (endLevelStatement.text != "")
        {
            if(endLevelStatement.text == "Game over! :<")
                nextLevelButton.SetActive(false);
            levelEndPanel.SetActive(true);
            UnlockNewLevel();
            Time.timeScale = 0f;
            hasLevelEnded = true; // lvl ended, stop level
            Analytics.Instance.EndLevelTime(SceneManager.GetActiveScene().buildIndex, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
        }
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        Analytics.Instance.NextLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

}
