using System;
using System.Windows.Input;

namespace CsvVisualizer.Core.Commands
{
    /// <summary>
    /// Command base.
    /// </summary>
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// Can execute changed evemt.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Body.</param>
        /// <param name="canExecute">Func to ensure executability.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Ensure executability.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>Command executability.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
