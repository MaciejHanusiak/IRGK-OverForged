using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    bool showConsole;

    string input;

    public static DebugCommand<int> SET_GOLD;

    public List<DebugCommandBase> commandList;
    public void OnToggleDebug()
    {
        
        showConsole = !showConsole;
    }
    private void GameInput_OnReturn(object sender, System.EventArgs e)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }
    private void Awake()
    {
        SET_GOLD = new DebugCommand<int>("set_gold", "Add amount of gold.", "set_gold", (x) =>
        {
            
            LevelStats.Instance.gold = x;
            Debug.Log("cheat-Gold Added");
        });

        commandList = new List<DebugCommandBase>()
        {
            SET_GOLD,
        };
    }
    public void Start()
    {
        gameInput.OnToggleDebug += GameInput_OnToggleDebug;
        gameInput.OnReturn += GameInput_OnReturn;
    }


    private void GameInput_OnToggleDebug(object sender, System.EventArgs e)
    {
        Debug.Log("ConsoleOn");
        OnToggleDebug();
    }


    private void OnGUI()
    {
        if (!showConsole) { return; }
        
        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("ConsoleInput");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("ConsoleInput");

    }

    private void HandleInput()
    {
        Debug.Log($"> {input}");
        string[] parts = input.Trim().Split(' ');
        if (parts.Length == 0) return;

        string commandName = parts[0].ToLower();

        foreach (var cmd in commandList)
        {
            if (cmd.CommandID.ToLower() == commandName)
            {
                if (cmd is DebugCommand<int> intCommand)
                {
                    if (parts.Length >= 2 && int.TryParse(parts[1], out int value))
                    {
                        intCommand.Invoke(value);
                        return;
                    }
                }
            }
        }


        //for (int i = 0; i < commandList.Count; i++)
        //{
        //    DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
        //    Debug.Log("commandBASE HERE");
        //    if (input.Contains(commandBase.CommandID))
        //    {
        //    Debug.Log("commandBASE HERE2");
        //        if (commandList[i] as DebugCommand<int> != null)
        //        {
        //    Debug.Log("commandBASE HERE3");
        //            // Cast to this type and invole the command
        //            (commandList[i] as DebugCommand<int>).Invoke();
        //        }
        //    }
        //}
    }
}