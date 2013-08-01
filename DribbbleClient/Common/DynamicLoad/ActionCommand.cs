using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.Common.DynamicLoad
{
    /// <summary>
    ///     Delegate command - does it all
    /// </summary>
    public class ActionCommand<T> : IActionCommand<T>
    {
        private Action<T> _execute = obj => { };
        private Func<T, bool> _canExecute = obj => true;

        public event EventHandler CanExecuteChanged;
        public event EventHandler ActionChanged;

        public ActionCommand()
        {

        }

        public ActionCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public ActionCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void SetAction(Action<T> action)
        {
            _execute = action;
            var handler = ActionChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void SetCanExecute(Func<T, bool> canExecute)
        {
            _canExecute = canExecute;
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute((T)parameter);
            }
        }
    }
}
