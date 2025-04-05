using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using frontend.Commands;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class LoginViewModel : NotifyPropertyChanged
    {
        private readonly AuthService AuthService;
        private readonly TokenStorage TokenStorage;
        private readonly WindowManager WindowManager;
        private string _name;
        private string _email;
        public string Password { private get; set; }
        public AsyncRelayCommand LoginCommand { get; set; }

        public LoginViewModel(
            AuthService authService,
            TokenStorage tokenStorage,
            WindowManager windowManager
        )
        {
            AuthService = authService;
            TokenStorage = tokenStorage;
            WindowManager = windowManager;
            LoginCommand = new AsyncRelayCommand(Login, CanLogin);
        }

        private bool CanLogin()
        {
            return LoginCommand.IsExecuted;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public async Task Login()
        {
            var user = new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
            };

            var auth = await AuthService.PostAuthAsync(user);

            if (auth.Data == null)
            {
                return;
            }

            TokenStorage.SaveToken(auth.Data.token);
            WindowManager.ShowWindow<MainView>();
            WindowManager.CloseWindow<LoginView>();
        }
    }
}
