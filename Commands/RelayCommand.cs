using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TwitchPlaysAnything.Commands
{
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;
        private ICommand? startTwitchControl;
        private object canStartTwitchControl;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(ICommand? startTwitchControl, object canStartTwitchControl)
        {
            this.startTwitchControl = startTwitchControl;
            this.canStartTwitchControl = canStartTwitchControl;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute(parameter);

            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
