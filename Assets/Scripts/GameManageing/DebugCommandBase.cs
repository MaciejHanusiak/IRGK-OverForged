using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DebugCommandBase
{

    private string commandId;
    private string commandDescription;
    private string commandFormat;


    public string CommandID { get { return commandId; } }
    public string CommandDescription { get { return commandDescription; } }
    public string CommandFormat { get { return commandFormat; } }

    public DebugCommandBase(string id, string description, string format)
    {
        commandId = id;
        commandDescription = description;
        commandFormat = format;
    }
}

public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> command;

    public DebugCommand(string id, string description, string format, Action<T> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T value)
    {
        command.Invoke(value);
    }
}