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
        if (isHintShowing)
        {
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
            actualHintNumber.text = hintIndex.ToString() + "/ 15";


            isHintShowing = true;

        }
    }

    string GetHintByIndex(int index)
    {
        string hint;
        switch (index)
        {
            case 0:
                hint = "text0 - pokazanie z³ota i czasu";
                break;
            case 1:
                hint = "text1 - pokazanie przepisu";
                break;
            case 2:
                hint = "text2 - pokazanie skrzynki z rud¹";
                break;
            case 3:
                hint = "text3 - pokazanie ¿e zabra³o z³oto";
                break;
            case 4:
                hint = "text4 - pokazuje na piec ¿eby przepaliæ";
                break;
            case 5:
                hint = "text5 - pokazuje na kowad³o ¿eby przekuæ sztabkê";
                break;
            case 6:
                hint = "text6 - kliknij na piec, ¿eby wrzuciæ rudê miedzi";
                break;
            case 7:
                hint = "text7 - poczekaj, a¿ ruda siê przepali na sztabkê miedzi";
                break;
            case 8:
                hint = "text8 - weŸ sztabkê miedzi z pieca";
                break;
            case 9:
                hint = "text9 - podejdŸ do kowad³a i kliknij, ¿eby otworzyæ menu kucia";
                break;
            case 10:
                hint = "text10 - wybierz przepis na miedziany miecz (lub kilof)";
                break;
            case 11:
                hint = "text11 - kliknij „Wykuæ” – zu¿yje sztabkê i drewno";
                break;
            case 12:
                hint = "text12 - poczekaj, a¿ przedmiot zostanie wykuty";
                break;
            case 13:
                hint = "text13 - weŸ gotowy miedziany miecz z kowad³a!";
                break;
            case 14:
                hint = "text14 - otwórz ekwipunek (klawisz I) i za³ó¿ miecz";
                break;
            case 15:
                hint = "text15 - gratulacje! Wiesz ju¿ jak robiæ narzêdzia i broñ. Powodzenia w grze!";
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
        hintIndex++;             //  ZWIÊKSZAMY INDEKS DOPIERO TUTAJ!
        Time.timeScale = 1f;
    }
}
