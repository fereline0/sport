using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;
using frontend.Views;
using shared.Enums;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly UserService UserService;
        private readonly TokenStorage TokenStorage;
        private readonly WindowManager WindowManager;
        private readonly NavigationService NavigationService;

        public RelayCommand ShowLoginCommand { get; }
        public RelayCommand ShowOrderCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public AsyncRelayCommand ReloadUserDataCommand { get; }

        private Order? _order;
        public Order? Order
        {
            get => _order;
            private set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged(nameof(Order));
                    ShowOrderCommand.NotifyCanExecuteChanged();
                    ShowLoginCommand.NotifyCanExecuteChanged();
                    LogoutCommand.NotifyCanExecuteChanged();
                    ReloadUserDataCommand.NotifyCanExecuteChanged();
                }
            }
        }

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
                    ReloadUserDataCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public MainViewModel(
            UserService userService,
            OrderService orderService,
            TokenStorage tokenStorage,
            WindowManager windowManager,
            NavigationService navigationService
        )
        {
            UserService = userService;
            TokenStorage = tokenStorage;
            WindowManager = windowManager;
            NavigationService = navigationService;

            ShowLoginCommand = new RelayCommand(ShowLogin, CanShowLogin);
            ShowOrderCommand = new RelayCommand(ShowOrder, CanShowOrder);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            ReloadUserDataCommand = new AsyncRelayCommand(LoadAuthedUserData, CanReloadUserData);

            LoadAuthedUserData().ConfigureAwait(false);
        }

        private bool CanShowLogin() => AuthedUser == null;

        private bool CanLogout() => AuthedUser != null;

        private bool CanShowOrder() => Order != null;

        private bool CanReloadUserData() => AuthedUser != null && !ReloadUserDataCommand.IsRunning;

        private async Task LoadAuthedUserData()
        {
            var authedUserResult = await UserService.GetAuthedUserAsync();
            AuthedUser = authedUserResult.Data;

            if (AuthedUser == null)
            {
                return;
            }

            var ordersResult = await UserService.GetOrdersByUserIdAsync(
                AuthedUser.Id,
                OrderStatus.Inactive
            );
            Order = ordersResult.Data?[0];
        }

        private void ShowLogin()
        {
            WindowManager.ShowWindow<LoginWindow>();
            WindowManager.CloseWindow<MainWindow>();
        }

        private void ShowOrder()
        {
            if (Order == null)
                return;

            NavigationService.NavigateTo<OrderPage>(Order);
        }

        private void Logout()
        {
            TokenStorage.ClearToken();
            AuthedUser = null;
            Order = null;
        }
    }
}
