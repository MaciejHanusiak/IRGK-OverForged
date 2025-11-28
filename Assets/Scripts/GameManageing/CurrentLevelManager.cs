using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] public GameObject levelEndPanel;
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
            LevelTime.timeRemaining <= 0 ? "Game over! :<" : "";
        if (endLevelStatement.text != "")
        {
            levelEndPanel.SetActive(true);
            UnlockNewLevel();
            Time.timeScale = 0f;
            hasLevelEnded = true; // lvl ended, stop level
        }
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
