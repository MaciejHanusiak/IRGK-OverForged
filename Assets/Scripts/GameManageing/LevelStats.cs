using UnityEngine;

public class LevelStats : MonoBehaviour
{
    [SerializeField] AudioSource goldSound;

    [SerializeField] AudioClip earnGold;
    [SerializeField] AudioClip spendGold;

    [Header("UI VFX")]
    [SerializeField] private CoinFlyUI coinFlyUI;
    [SerializeField] private RectTransform goldIconUI; // RectTransform ikonki monet w HUD

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
        goldSound.clip = earnGold;
        goldSound.Play();

    }


    public bool SpendGold( int moneyAmount)
    {
        if (gold >= moneyAmount)
        {

        gold -= moneyAmount;
        Debug.Log("Gold: " + gold);
        Analytics.Instance.GoldSpend(gold, moneyAmount, LevelTime.Instance.timeRemaining);
            goldSound.clip = spendGold;
            goldSound.Play();
            if (coinFlyUI != null && goldIconUI != null)
                coinFlyUI.PlaySpend(moneyAmount, goldIconUI);
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
