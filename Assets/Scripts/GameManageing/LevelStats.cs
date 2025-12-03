using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public static LevelStats Instance;              // }
    public int gold = 0;                            // }
                                                    // }
    public void Awake()                             // }
    {                                               // }
        if (Instance == null) Instance = this;      // }
        else Destroy(gameObject);                   // }  Wzorzec Singleton

    }

    public void AddGold(int moneyAmount)
    {
        gold += moneyAmount;
        Debug.Log("Gold: " + gold);
        Analytics.Instance.GoldAdded(gold, moneyAmount, LevelTime.Instance.timeRemaining);

    }


    public bool SpendGold( int moneyAmount)
    {
        if (gold >= moneyAmount)
        {

        gold -= moneyAmount;
        Debug.Log("Gold: " + gold);
        Analytics.Instance.GoldSpend(gold, moneyAmount, LevelTime.Instance.timeRemaining);
            return true;
        }
        Debug.Log("Not enough gold");
        return false;


    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
