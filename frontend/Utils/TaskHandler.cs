using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using frontend.ViewModels;

namespace frontend.Utils
{
    public class TaskHandler : NotifyPropertyChanged
    {
        private bool _loaded = true;
        private readonly Func<Task> _task;
        private readonly int _delayMilliseconds;

        public bool Loaded
        {
            get => _loaded;
            private set
            {
                if (_loaded != value)
                {
                    _loaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaskHandler(Func<Task> task, int delayMilliseconds = 0)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _delayMilliseconds = delayMilliseconds;
        }

        public async Task Execute()
        {
            Loaded = false;
            try
            {
                await _task();

                if (_delayMilliseconds > 0)
                {
                    await Task.Delay(_delayMilliseconds);
                }
            }
            finally
            {
                Loaded = true;
            }
        }
    }
}
