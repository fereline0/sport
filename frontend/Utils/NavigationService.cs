using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace frontend.Utils
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private Frame? _mainFrame;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
            _mainFrame.Navigated += OnFrameNavigated;
        }

        public void NavigateTo<T>()
            where T : Page
        {
            NavigateTo<T>(null);
        }

        public void NavigateTo<T>(object parameter)
            where T : Page
        {
            if (_mainFrame == null)
                throw new InvalidOperationException(
                    "MainFrame is not initialized. Call Initialize first."
                );

            var page = _serviceProvider.GetRequiredService<T>();

            page.Tag = parameter;
            _mainFrame.Navigate(page);
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page && page.Tag != null)
            {
                if (page.DataContext is IParameterReceiver receiver)
                {
                    receiver.ReceiveParameter(page.Tag);
                }
            }
        }
    }

    public interface IParameterReceiver
    {
        void ReceiveParameter(object parameter);
    }
}
