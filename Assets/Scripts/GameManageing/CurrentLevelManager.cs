using UnityEngine;
using UnityEngine.SceneManagement;
public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] public int levelGoldGoal;
    private string LevelEndStatement = string.Empty;
    private float normalFixedDeltaTime;

    private bool hasLevelEnded = false;

    private void Awake()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime; // Save default value
    }

    void Update()
    {
        // If level ended do nothing
        if (hasLevelEnded) return;
        LevelEndStatement = LevelStats.Instance.gold >= levelGoldGoal ? "Congrats! You won this lvl!" : 
            LevelTime.timeRemaining <= 0 ? "Game over! :<" : "";
        if (LevelEndStatement != "")
        {
            ShowLevelEndScreen(LevelEndStatement);
            UnlockNewLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
            hasLevelEnded = true; // lvl ended, stop level
        }
    }

    public void ShowLevelEndScreen(string lvlEndStmt)
    {
        Debug.Log(lvlEndStmt);
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
