using UnityEngine;
using TMPro;
public class TutorialLevelScript : MonoBehaviour
{


    [SerializeField] private Transform tutorialPanel;
    [SerializeField] private Transform levelEndMenu;

    [SerializeField] private TextMeshProUGUI actualHintNumber;
    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private Player player;
    [SerializeField] private ForgeCounter forgeCounter;
    [SerializeField] private AnvilCounter anvilCounter;
    [SerializeField] private WorkBenchCounter workBenchCounter;
    [SerializeField] private SawCounter sawCounter;
    [SerializeField] private KnifesBenchCounter KnifesBenchCounter;
    [SerializeField] private ClearCounter clearCounter;

    private int hintIndex = 0;

    private bool isHintShowing = false;
    private void Awake()
    {

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(hintIndex > 0)
            isHintShowing = tutorialPanel.gameObject.activeSelf;
        if (isHintShowing)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                tutorialPanel.gameObject.SetActive(false);
                isHintShowing = false;   // teraz mo¿e pojawiæ siê nastêpny
                if (GetConditionByIndex(hintIndex))
                    hintIndex++;             //  ZWIÊKSZAMY INDEKS DOPIERO TUTAJ!
                Time.timeScale = 1f;
            }

            Time.timeScale = 0f;
            return;
        }
        else
        {
            Time.timeScale = 1f;
        }
        if (hintIndex >= 16)
            levelEndMenu.gameObject.SetActive(true);

        if (GetConditionByIndex(hintIndex))
        {
           
            
            tutorialPanel.gameObject.SetActive(true);
            hintText.text = GetHintByIndex(hintIndex);
            actualHintNumber.text = (hintIndex + 1).ToString()  + "/ 16";
            isHintShowing = true;

        }

    }

    string GetHintByIndex(int index)
    {
        string hint;
        switch (index)
        {
            case 0:
                hint = "Hello! In OverForged your main goal is to collect the right amount of gold before time runs out. Currently, you have 5 pieces of gold, and your goal is to obtain at least 10. In the tutorial level, you have unlimited time, so you don't need to worry about it.\r\n\r\nIf you forget last hint, just click on green box with “?” symbol on the right :)";
                break;
            case 1:
                hint = "You can earn gold by fulfilling weapon orders. The list of pending orders is in the top-left corner. As you can see, there's an order for a copper sword in the queue. Let's get to work!";
                break;
            case 2:
                hint = "First, you need to make a copper blade. To do this, approach the chest with the greenish stone. When you're in front of the chest, press “E” to buy copper ore.";
                break;
            case 3:
                hint = "Buying the copper ore cost 2 gold. You can notice that the gold has been deducted from your resources. Also, look at the panel at the top edge of the screen—it shows what you're currently looking at, what you can offer, and what you'll receive in exchange.";
                break;
            case 4:
                hint = "Now you need to smelt the ore into an ingot. To do this, place the ore in the smelter above you using the “E” key and wait for it to smelt. Be careful not to ruin it—if the progress bar turns red, it means you need to pull the ingot out of the smelter as quickly as possible";
                break;
            case 5:
                hint = "The smelter has smelted the ingot. Now pick it up from the smelter and place it on the anvil. When the ingot is on the anvil, press “F” to hammer it and shape it into a blade. Keep hammering until it reaches the desired shape.";
                break;
            case 6:
                hint = "Great! You've successfully created the blade. Now approach the workbench and press “F” to craft a weapon rack. The weapon stand will appear on the table, and you can place the copper blade on it";
                break;
            case 7:
                hint = "You've created a weapon stand are where you can place finished weapon parts. Remember, the weapon stand must be on the workbench so you can assemble the next weapon parts. Outside the workbench, you don't have the necessary tools for this, so ALWAYS ASSEMBLE WEAPONS WHEN THE WEAPON STAND IS ON THE WORKBENCH! And you'll be the best blacksmith in the world!\r\n\r\nOnce you've placed the blade on the weapon stand, approach the log chest on the left side of the yard and buy one.";
                break;
            case 8:
                hint = "Your gold reserves have shrunk again, but don't worry—you'll soon have a finished weapon to sell. Take the log to the saw table to saw it into planks. Here too, watch the time so the planks don't get ruined.";
                break;
            case 9:
                hint = "Excellent! Now, from the planks, you can whittle a handle on the knife table to the right. Whittle it just like forging the blade, using “F” and then pick it up!";
                break;
            case 10:
                hint = "Awesome! You have a finished handle in your hands. Now take it to the workbench to assemble the weapon completely. Once you place the handle, the weapon will be complete. You can compare it to the order. If it matches, you can pick up the weapon stand.";
                break;
            case 11:
                hint = "You can only hold one item at a time. If you need to set something down just for a moment, you can use the empty table in the middle of the yard. Let's go for it!";
                break;
            case 12:
                hint = "Now pick up the weapon stand with the finished weapon again and place it at the delivery station—it's the long table at the bottom of the yard.";
                break;
            case 13:
                hint = "When you place a weapon stand here with the appropriate matching parts as in the recipe, you can sell the weapon and receive payment for it. Note that each recipe has its own delivery time—if you manage to deliver it within that time, you'll get a bonus for express order fulfillment!\r\nNow I'll show you what to do if you accidentally ruin an item. Buy copper ore and put it in the smeltr, wait until the red bar above the furnace fills up.";
                break;
            case 14:
                hint = "Now there's a cracked copper ingot in the furnace that's good for nothing—you can only throw it into the scrap bin. It's located in the bottom-right corner of the yard.\r\nPlace the ruined ingot in the scrap bin.";
                break;
            case 15:
                hint = "The item has just been thrown into the scrap.\r\nCongratulations! You've completed the tutorial! Good luck, Blacksmith!";
                break;
            default:
                hint = "default text";
                break;
        }
        return hint;
    }

    bool GetConditionByIndex(int hintIndex)
    {
        bool condition = false;
        switch (hintIndex)
        {
            case 0: condition = true; // brak warunku
                break;
            case 1: condition = true; // brak warunku
                break;
            case 2: condition = true; // brak warunku 
                break;
            case 3: condition = (player.GetSmithObject() != null && player.GetSmithObject().GetSmithObjectSO().name == "CopperOre");
                break;
            case 4: condition = true;
                break;
            case 5: condition = (forgeCounter.GetSmithObject() != null && forgeCounter.GetSmithObject().GetSmithObjectSO().name == "CopperIngot");
                break;
            case 6:
                condition = (anvilCounter.GetSmithObject() != null && anvilCounter.GetSmithObject().GetSmithObjectSO().name == "CopperBlade");
                break;
            case 7:
                condition = (workBenchCounter.GetSmithObject() != null && workBenchCounter.GetSmithObject().GetSmithObjectSO().name == "WeaponStand");
                break;
            case 8:
                condition =(player.GetSmithObject() != null && player.GetSmithObject().GetSmithObjectSO().name == "WoodenLog");
                break;
            case 9:
                condition = (player.GetSmithObject() != null && player.GetSmithObject().GetSmithObjectSO().name == "WoodenPlanks");
                break;
            case 10:
                condition = (player.GetSmithObject() != null && player.GetSmithObject().GetSmithObjectSO().name == "WoodenSwordHandle");
                break;
            case 11:
                condition =(player.GetSmithObject() != null && player.GetSmithObject().GetSmithObjectSO().name == "WeaponStand");
                break;
            case 12:
                condition = (clearCounter.GetSmithObject() != null && clearCounter.GetSmithObject().GetSmithObjectSO().name == "WeaponStand");
                break;
            case 13:
                condition = LevelStats.Instance.gold >= 4;
                break;
            case 14:
                condition = (forgeCounter.GetSmithObject() != null && forgeCounter.GetSmithObject().GetSmithObjectSO().name == "CopperIngotBurned");
                break;
            case 15:
                condition = (player.GetSmithObject() == null) && forgeCounter.GetSmithObject() == null;
                break;
            default: condition = false;
                break;
        }

        return condition;
    }

    public void IGotItButton()
    {
        tutorialPanel.gameObject.SetActive(false);
        isHintShowing = false;   // teraz mo¿e pojawiæ siê nastêpny
        if(GetConditionByIndex(hintIndex))
            hintIndex++;             //  ZWIÊKSZAMY INDEKS DOPIERO TUTAJ!
        Time.timeScale = 1f;
    }
}
