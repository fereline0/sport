using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
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
        private string? _name;
        private string? _email;
        public string? Password { private get; set; }
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
            return !LoginCommand.IsRunning;
        }

        public string? Name
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

        public string? Email
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

            var postedAuthResult = await AuthService.PostAuthAsync(user);

            if (postedAuthResult.Data == null)
            {
                return;
            }

            TokenStorage.SaveToken(postedAuthResult.Data.token!);
            WindowManager.ShowWindow<MainWindow>();
            WindowManager.CloseWindow<LoginWindow>();
        }
    }
}
