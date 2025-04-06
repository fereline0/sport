using System.Windows;
using frontend.Services;
using frontend.Utils;
using frontend.ViewModels;
using frontend.Views;
using Microsoft.Extensions.DependencyInjection;

namespace frontend
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = mainViewModel;

            navigationService.Initialize(MainFrame);

            navigationService.NavigateTo<HomePage>();
        }
    }
}
