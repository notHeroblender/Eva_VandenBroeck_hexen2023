using System;

internal class DelegateCommand : ICommand
{
    private Action _execute;
    private Action _undo;

    public DelegateCommand(Action execute, Action undo)
    {
        _execute = execute;
        _undo = undo;
    }


    public void Execute()
        => _execute();

    public void Undo()
        => _undo();
}
