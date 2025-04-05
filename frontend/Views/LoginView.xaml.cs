using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;

namespace frontend.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView(
            AuthService authService,
            TokenStorage tokenStorage,
            WindowManager windowManager
        )
        {
            InitializeComponent();
            DataContext = new LoginViewModel(authService, tokenStorage, windowManager);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = Password.Password;
            }
        }
    }
}
