
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
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

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
        AnalyticsService.Instance.Flush();
        Debug.Log("next_level");
    }
    public void RestartGame()
    {
        AnalyticsService.Instance.RecordEvent("restart_game");
        AnalyticsService.Instance.Flush();
        Debug.Log("restart_game");

    }
    public void GamePaused()
    {
        AnalyticsService.Instance.RecordEvent("game_paused");
        AnalyticsService.Instance.Flush();
        Debug.Log("game_paused"); 
    }
    public void GameResumed()
    {
        AnalyticsService.Instance.RecordEvent("game_resumed");
        AnalyticsService.Instance.Flush();
        Debug.Log("game_resumed");
    }
    public void GoHome()
    {
        AnalyticsService.Instance.RecordEvent("go_home");
        AnalyticsService.Instance.Flush();
    }
    public void GoldChanged(int goldAmount, float levelTime)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("gold_changed")
        {
            {"gold_amount", goldAmount},
            {"level_time", levelTime}
        };
        AnalyticsService.Instance.RecordEvent("gold_changed");
        AnalyticsService.Instance.Flush();
    }
    public void RecipeGenerated(string recipeName, float levelTime, int goldAmount)
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
        AnalyticsService.Instance.RecordEvent("recipe_generated");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerBuySomething(string productName, float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_buy_something")
        {
            {"product_name", productName},
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_buy_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerSmeltSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_smelt_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_smelt_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerBurntSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_burnt_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_burnt_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerCutSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_cut_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_cut_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerOvercutSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_overcut_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_overcut_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerWhittledSomethingOnce(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_whittled_something_once")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_whittled_something_once");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerStrikeSomethingOnce(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_whittled_something_once")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_whittled_something_once");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerForgedSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_forged_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_forged_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerPlanedSomething(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_planed_something")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_planed_something");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerCreateWeaponStand(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_create_weapon_stand")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_create_weapon_stand");
        AnalyticsService.Instance.Flush();
    }
    public void PlayerAddWeaponPart(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("player_add_weapon_part")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("player_add_weapon_part");
        AnalyticsService.Instance.Flush();
    }
    public void WeaponCompleted(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("weapon_completed")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("weapon_completed");
        AnalyticsService.Instance.Flush();
    }
    public void TimeForRecipeEnd(float levelTime, int goldAmount, int recipeId, string recipeName)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("recipe_time_end")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount },
            {"recipe_id", recipeId},
            {"recipeName", recipeName }
        };
        AnalyticsService.Instance.RecordEvent("recipe_time_end");
        AnalyticsService.Instance.Flush();
    }
    public void GoodRecipeDelivered(float levelTime, int goldAmount, int recipeId, string recipeName)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("good_recipe_delivered")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount },
            {"recipe_id", recipeId},
            {"recipeName", recipeName }
        };
        AnalyticsService.Instance.RecordEvent("good_recipe_delivered");
        AnalyticsService.Instance.Flush();
    }
    public void EndLevelTime(float levelTime, int goldAmount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("recipe_time_end")
        {
            {"level_time", levelTime},
            {"gold_amount", goldAmount }
        };
        AnalyticsService.Instance.RecordEvent("recipe_time_end");
        AnalyticsService.Instance.Flush();
    }

}
