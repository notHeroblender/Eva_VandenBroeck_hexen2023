using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandQueue
{
    private List<ICommand> _commands = new List<ICommand>();

    private int _currentCommand = -1;

    public bool IsAtStart => _currentCommand < 0;

    public bool IsAtEnd => _currentCommand >= _commands.Count - 1;

    public void Execute(ICommand command)
    {
        _commands.Add(command);

        while (!IsAtEnd)
            Next();
    }
    public void Previous()
    {
        if (IsAtStart)
            return;

        _commands[_currentCommand].Undo();

        _currentCommand -= 1;

    }
    public void Next()
    {
        if (IsAtEnd)
            return;

        _currentCommand += 1;

        _commands[_currentCommand].Execute();
    }
    public void ReturnCommands()
    {
        if (!IsAtEnd)
        {
            Debug.Log("Eva: " + _currentCommand);
            for (int i = _commands.Count; i >= _currentCommand + 1; i--)
            {
                Debug.Log("Eva: " + _commands[i]);
                if (_commands.Count > 0)
                {
                    _commands.RemoveAt(i - 1);
                }
            }
        }
    }
}
