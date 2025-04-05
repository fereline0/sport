using System;
using System.Threading.Tasks;

namespace frontend.Commands
{
    public class AsyncRelayCommand : RelayCommandBase
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        public bool IsExecuted { get; private set; } = true;
        public bool IsExecuting { get; private set; }

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
            : this(
                p => execute(),
                canExecute != null ? p => canExecute() : (Func<object, bool>)null
            ) { }

        public AsyncRelayCommand(Func<object, Task> execute, Func<bool> canExecute = null)
            : this(execute, canExecute != null ? p => canExecute() : (Func<object, bool>)null) { }

        public AsyncRelayCommand(Func<Task> execute, Func<object, bool> canExecute)
            : this(p => execute(), canExecute) { }

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return IsExecuted && !IsExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public override async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                IsExecuted = false;
                IsExecuting = true;
                RaiseCanExecuteChanged();
                await _execute(parameter);
            }
            finally
            {
                IsExecuted = true;
                IsExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
    }
}
