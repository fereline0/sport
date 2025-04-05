using System;
using System.Windows.Input;

namespace frontend.Commands
{
    public class RelayCommand : RelayCommandBase
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(p => execute(), canExecute != null ? p => canExecute() : null) { }

        public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
            : this(execute, canExecute != null ? p => canExecute() : null) { }

        public RelayCommand(Action execute, Func<object, bool> canExecute)
            : this(p => execute(), canExecute) { }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _execute(parameter);
    }
}
