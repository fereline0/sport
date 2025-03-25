using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Utils
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    namespace frontend
    {
        public class WindowManager
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Dictionary<Type, Window> _openedWindows = new();

            public WindowManager(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public void ShowWindow<T>()
                where T : Window
            {
                if (_openedWindows.TryGetValue(typeof(T), out var window))
                {
                    window.Activate();
                    return;
                }

                window = _serviceProvider.GetRequiredService<T>();
                window.Closed += (_, _) => _openedWindows.Remove(typeof(T));

                _openedWindows[typeof(T)] = window;
                window.Show();
            }

            public bool? ShowDialog<T>()
                where T : Window
            {
                var window = _serviceProvider.GetRequiredService<T>();
                return window.ShowDialog();
            }

            public void CloseWindow<T>()
                where T : Window
            {
                if (_openedWindows.TryGetValue(typeof(T), out var window))
                {
                    window.Close();
                }
            }
        }
    }
}
