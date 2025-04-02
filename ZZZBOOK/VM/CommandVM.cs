using System.Windows.Input;

namespace ZZZBOOK.VM
{
    public class CommandMvvm : ICommand
    {
        Action action;
        Func<bool> canExecute;

        public CommandMvvm(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
}