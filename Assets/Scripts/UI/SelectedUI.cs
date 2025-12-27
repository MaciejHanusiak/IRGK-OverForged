using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image selectedPanel;
    public Image inputImage;
    public Image actionImage;
    public Image outputImage;
    public TextMeshProUGUI selectedObjectUIName;

    [Header("References")]
    public ToolUIDatabase database; // Przeci¹gnij tutaj swój asset Tool Database

    [Header("Optional Labels")]
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI outputText;
    public TextMeshProUGUI goldAmount;

    public Transform goldAmountPanel;

    private void Start()
    {
        // Subskrybujemy event
        Player.Instance.OnSelectedCounterChanged += UpdateToolUI;

        // Na start ukryj panel (¿eby nie œwieci³ na pocz¹tku gry)
        HidePanel();
        

    }

    private void OnDestroy()
    {
        // Wa¿ne! Odsubskrybuj, ¿eby nie by³o b³êdów po zniszczeniu obiektu
        Player.Instance.OnSelectedCounterChanged -= UpdateToolUI;
    }

    void UpdateToolUI(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter != null)
        {
            string objectName = e.selectedCounter.GetType().ToString();

            selectedPanel.gameObject.SetActive(true);

            // <<< OCHRONA PRZED NULLAMI >>>
            if (database == null)
            {
                Debug.LogError("ToolUIDatabase nie jest przypisany w Inspectorze w skrypcie SelectedUI!");
                ClearIconsAndLabels();
                return;
            }

            ToolUIConfig config = database.GetConfig(objectName);

            if (config != null)
            {
                selectedObjectUIName.text = config.objectUIName;
                inputImage.sprite = config.inputSprite;
                actionImage.sprite = config.actionSprite;
                outputImage.sprite = config.outputSprite;
               
                if (inputText) inputText.text = config.inputLabel;
                if (actionText) actionText.text = config.actionLabel;
                if (outputText) outputText.text = config.outputLabel;
                if (objectName == "ContainerCounter")
                {
                    goldAmountPanel.gameObject.SetActive(true);

                    outputImage.sprite = e.selectedCounter.GetOutputSmithObjectSO().sprite;
                    outputText.text = e.selectedCounter.GetOutputSmithObjectSO().name;
                    goldAmount.text = e.selectedCounter.GetOutputSmithObjectSO().price.ToString();
                    Debug.Log(e.selectedCounter.GetOutputSmithObjectSO().name);
                }
                else
                {
                    goldAmountPanel.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning($"Brak konfiguracji w ToolUIDatabase dla obiektu: \"{objectName}\"");
                ClearIconsAndLabels();
            }
        }
        else
        {
            HidePanel();
        }
    }

    private void ClearIconsAndLabels()
    {
        inputImage.sprite = null;
        actionImage.sprite = null;
        outputImage.sprite = null;

        if (inputText) inputText.text = "";
        if (actionText) actionText.text = "";
        if (outputText) outputText.text = "";
    }

    private void HidePanel()
    {
        selectedPanel.gameObject.SetActive(false);
        ClearIconsAndLabels();
    }
}