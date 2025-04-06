using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using frontend;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly UserService _userService;
        private readonly TokenStorage _tokenStorage;
        private readonly WindowManager _windowManager;

        public RelayCommand ShowLoginCommand { get; }
        public RelayCommand LogoutCommand { get; }

        private User? _authedUser;
        public User? AuthedUser
        {
            get => _authedUser;
            set
            {
                if (_authedUser != value)
                {
                    _authedUser = value;
                    OnPropertyChanged(nameof(AuthedUser));
                    ShowLoginCommand.NotifyCanExecuteChanged();
                    LogoutCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public MainViewModel(
            UserService userService,
            TokenStorage tokenStorage,
            WindowManager windowManager
        )
        {
            _userService = userService;
            _tokenStorage = tokenStorage;
            _windowManager = windowManager;

            ShowLoginCommand = new RelayCommand(ShowLogin, CanShowLogin);
            LogoutCommand = new RelayCommand(Logout, CanLogout);

            LoadAuthedUserData().ConfigureAwait(false);
        }

        private bool CanShowLogin() => AuthedUser == null;

        private bool CanLogout() => AuthedUser != null;

        private async Task LoadAuthedUserData()
        {
            var authedUserResult = await _userService.GetAuthedUserAsync();
            if (authedUserResult.Error != null || authedUserResult.Data == null)
                return;
            AuthedUser = authedUserResult.Data;
        }

        private void ShowLogin()
        {
            _windowManager.ShowWindow<LoginWindow>();
            _windowManager.CloseWindow<MainWindow>();
        }

        private void Logout()
        {
            _tokenStorage.ClearToken();
            AuthedUser = null;
        }
    }
}
