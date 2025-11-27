using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] public int levelGoldGoal;
    string LevelEndStatement { get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LevelEndStatement = LevelStats.Instance.gold >= levelGoldGoal ? "Congrats! You won this lvl!" : 
            LevelTime.timeRemaining <= 0 ? "Game over! :<" : "";
        Debug.Log(LevelEndStatement);
    }


   
}
