using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace frontend.ViewModels
{
    public abstract class TaskHandlerBase : NotifyPropertyChanged
    {
        private bool _loaded = true;
        public bool Loaded
        {
            get => _loaded;
            protected set
            {
                if (_loaded != value)
                {
                    _loaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public abstract Task Execute();
    }

    public class TaskHandler : TaskHandlerBase
    {
        private readonly Func<Task> _task;
        private readonly int _delayMilliseconds;

        public TaskHandler(Func<Task> task, int delayMilliseconds = 0)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _delayMilliseconds = delayMilliseconds;
        }

        public override async Task Execute()
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

    public class TaskHandler<T> : TaskHandlerBase
    {
        private readonly Func<Task<T>> _task;
        private readonly int _delayMilliseconds;

        private T? _data;
        public T? Data
        {
            get => _data;
            private set
            {
                if (!EqualityComparer<T?>.Default.Equals(_data, value))
                {
                    _data = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaskHandler(Func<Task<T>> task, int delayMilliseconds = 0)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _delayMilliseconds = delayMilliseconds;
        }

        public override async Task Execute()
        {
            Loaded = false;
            try
            {
                Data = await _task();

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
