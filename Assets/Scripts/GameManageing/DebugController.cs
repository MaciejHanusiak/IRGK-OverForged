using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    bool showConsole;

    string input;
    public void Start()
    {
        gameInput.OnToggleDebug += GameInput_OnToggleDebug;
    }

    private void GameInput_OnToggleDebug(object sender, System.EventArgs e)
    {
        Debug.Log("ConsoleOn");
        OnToggleDebug();
    }

    public void OnToggleDebug()
    {
        
        showConsole = !showConsole;
    }

    private void OnGUI()
    {
        if (!showConsole) { return; }
        
        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

    }
}
