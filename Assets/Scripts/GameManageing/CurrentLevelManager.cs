using UnityEngine;

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
            hasLevelEnded = true; // lvl ended, stop level
        }
    }

    public void ShowLevelEndScreen(string lvlEndStmt)
    {
        Debug.Log(lvlEndStmt);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }

}
