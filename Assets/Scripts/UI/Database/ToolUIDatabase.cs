using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 2. Baza danych - ScriptableObject (tworzysz jako asset w projekcie)
[CreateAssetMenu(fileName = "ToolDatabase", menuName = "UI/Tool Database")]
public class ToolUIDatabase : ScriptableObject
{
    public List<ToolUIConfig> configs = new List<ToolUIConfig>();

    // Szybkie wyszukiwanie po nazwie obiektu
    public ToolUIConfig GetConfig(string objectName)
    {
        return configs.FirstOrDefault(c => c.objectName == objectName);
    }
}