
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    public static Analytics Instance;
    private bool _isInitialized = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true; ;
    }

    public void NextLevel(int currentLevel)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("next_level")
        {
            {"level_index", currentLevel }
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        
        Debug.Log("next_level");
    }
    public void RestartGame()
    {
        AnalyticsService.Instance.RecordEvent("restart_game");
        
        Debug.Log("restart_game");

    }
    public void GamePaused()
    {
        AnalyticsService.Instance.RecordEvent("game_paused");
        
        Debug.Log("game_paused"); 
    }
    public void GameResumed()
    {
        AnalyticsService.Instance.RecordEvent("game_resumed");
        
        Debug.Log("game_resumed");
    }
    public void GoHome()
    {
        AnalyticsService.Instance.RecordEvent("go_home");
        Debug.Log("go_home");


    }
    public void GoldAdded(int goldAmount,int goldChange, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("gold_added")
        {
            {"gold_amount", goldAmount},
            {"gold_added", goldChange },
            {"level_time", levelTime},
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("gold_added");


    }
    public void GoldSpend(int goldAmount, int goldChange, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("gold_spend")
        {
            {"gold_amount", goldAmount},
            {"gold_spend", goldChange },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("gold_spend");


    }
    public void RecipeGenerated(string recipeName, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("recipe_generated")
        {
            {"recipe_name", recipeName},
            {"level_time", levelTime},
            {"gold_amount", goldAmount}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("recipe_generated");


    }
    public void RecipeQuerryFull( int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("recipe_querry_full")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("recipe_querry_full");


    }
    public void PlayerBuySomething(string productName, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_buy_something")
        {
            {"product_name", productName},
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_buy_something");

    }
    public void PlayerSmeltSomething(string smeltingItemName, float smeltingItemTime, float smeltingItemTimeMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_smelt_something")
        {
            {"smelting_item_name", smeltingItemName},
            {"smelting_item_time", smeltingItemTime },
            {"smelting_item_time_max", smeltingItemTimeMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_smelt_something");

    }
    public void PlayerBurntSomething(string burningItemName, float burningItemTime, float burningItemTimeMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_burnt_something")
        {
            {"burning_item_name", burningItemName},
            {"burning_item_time", burningItemTime },
            {"burning_item_time_max", burningItemTimeMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_burnt_something");


    }
    public void PlayerCutSomething(string cuttingItemName, float cuttingItemTime, float cuttingItemTimeMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_cut_something")
        {
            {"cutting_item_name", cuttingItemName},
            {"cutting_item_time", cuttingItemTime },
            {"cutting_item_time_max", cuttingItemTimeMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_cut_something");

    }
    public void PlayerOvercutSomething(string overcuttingItemName, float overcuttingItemTime, float overcuttingItemTimeMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_overcut_something")
        {
            {"overcutting_item_name", overcuttingItemName},
            {"overcutting_item_time", overcuttingItemTime },
            {"overcutting_item_time_max", overcuttingItemTimeMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_overcut_something");

    }
    public void PlayerWhittledSomethingOnce(float knifePlanningProgress, float knifePlanningProgressMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_whittled_something_once")
        {
            {"knife_planing_progress", knifePlanningProgress},
            {"knife_planning_progress_max", knifePlanningProgressMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_whittled_something_once");

    }
    public void PlayerStrikeSomethingOnce(float forgeingProgress, float forgeingProgressMax, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_striked_something_once")
        {
            {"forgeing_progress", forgeingProgress},
            {"forgeing_progress_max", forgeingProgressMax },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_striked_something_once");


    }
    public void PlayerForgedSomething(string forgedItemName, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_forged_something")
        {
            {"forged_item_name", forgedItemName},
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_forged_something");

    }
    public void PlayerPlannedSomething(string PlannedItemName, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_planned_something")
        {
            {"planned_item_name", PlannedItemName},
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_planned_something");

    }
    public void PlayerCreateWeaponStand(int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_create_weapon_stand")
        {
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_create_weapon_stand");


    }
    public void PlayerAddWeaponPart(string weaponPartName, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_add_weapon_part")
        {
            {"weapon_part_name", weaponPartName},
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("player_add_weapon_part");


    }
    //public void WeaponCompleted(float levelTime, int goldAmount)
    //{
    //    if (!_isInitialized)
    //    {
    //        return;
    //    }
    //    CustomEvent myEvent = new CustomEvent("weapon_completed")
    //    {
    //        {"level_time", levelTime},
    //        {"gold_amount", goldAmount }
    //    };
    //    AnalyticsService.Instance.RecordEvent("weapon_completed");
    //    
    //}
    public void TimeForRecipeEnd(string recipeName, int recipeId, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("recipe_time_end")
        {

            {"recipe_name", recipeName},
            {"recipe_id", recipeId },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("recipe_time_end");


    }
    public void GoodRecipeDelivered(string recipeName, int recipeId, int goldAmount, float recipeTime, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("good_recipe_delivered")
        {

            {"recipe_name", recipeName},
            {"recipe_id", recipeId },
            {"gold_amount", goldAmount },
            {"recipe_time", recipeTime },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("good_recipe_delivered");


    }
    public void EndLevelTime(int levelId, int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("level_time_end")
        {
            {"level_id", levelId },
            {"gold_amount", goldAmount },
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Debug.Log("level_time_end");


    }
    public void EndGame()
    {
        if (!_isInitialized)
        {
            return;
        }

        AnalyticsService.Instance.RecordEvent("end_game");
        AnalyticsService.Instance.Flush();
        Debug.Log("end_game_data_flushed");


    }
}
