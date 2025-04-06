using System.Windows;
using System.Windows.Controls;
using frontend.Services;
using frontend.Utils;
using frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace frontend.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }
    }
}
