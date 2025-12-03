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
        //Analytics.Instance.GoldChanged(gold,);

    }

    public void AddGold(int moneyAmount)
    {
        gold += moneyAmount;
        Debug.Log("Gold: " + gold);
    }


    public bool SpendGold( int amount)
    {
        if (gold >= amount)
        {

        gold -= amount;
        Debug.Log("Gold: " + gold);
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
