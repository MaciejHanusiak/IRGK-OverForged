using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 1. Konfiguracja pojedynczego narzêdzia/obiektu
[System.Serializable]
public class ToolUIConfig
{
    public string objectName;      // Nazwa obiektu (GameObject.name) na który patrzy gracz
    public string objectUIName;    // Nazwaw która wyœwietli siê w UI
    public Sprite inputSprite;     // Ikona/sprite dla INPUT
    public Sprite actionSprite;    // Ikona/sprite dla ACTION
    public Sprite outputSprite;    // Ikona/sprite dla OUTPUT

    // Opcjonalnie: stringi na etykiety tekstowe
    public string inputLabel = "";
    public string actionLabel = "";
    public string outputLabel = "";
}