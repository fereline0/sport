using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using frontend.Commands;
using frontend.Services;
using frontend.Utils.frontend;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly ProductService ProductService;
        private readonly UserService UserService;
        private readonly OrderItemsService OrderItemsService;
        private readonly WindowManager WindowManager;

        public ObservableCollection<Product> Products { get; } =
            new ObservableCollection<Product>();
        public AsyncRelayCommand ReloadCommand { get; }
        public RelayCommand LoginCommand { get; set; }
        public AsyncRelayCommand AddToOrderCommand { get; }
        public AsyncRelayCommand RemoveFromOrderCommand { get; }
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
                }
            }
        }
        private bool HasOrders;

        public MainViewModel(
            ProductService productService,
            UserService userService,
            OrderItemsService orderItemsService,
            WindowManager windowManager
        )
        {
            ProductService = productService;
            UserService = userService;
            OrderItemsService = orderItemsService;
            WindowManager = windowManager;
            ReloadCommand = new AsyncRelayCommand(LoadData, CanReload);
            LoginCommand = new RelayCommand(OpenLogin, CanLogin);
            AddToOrderCommand = new AsyncRelayCommand(AddToOrder, CanAddToOrder);
            RemoveFromOrderCommand = new AsyncRelayCommand(RemoveFromOrder, CanRemoveToOrder);

            LoadData().ConfigureAwait(false);
        }

        private bool CanLogin() => AuthedUser == null;

        private bool CanReload() => ReloadCommand.IsExecuted;

        private bool CanAddToOrder() => AddToOrderCommand.IsExecuted;

        private bool CanRemoveToOrder() => RemoveFromOrderCommand.IsExecuted;

        private async Task LoadData()
        {
            await LoadProducts();
            await CheckUserAuth();
        }

        private async Task LoadProducts()
        {
            var products = await ProductService.GetProductsAsync();

            if (products.Error != null || products.Data == null)
            {
                MessageBox.Show("Не удалось загрузить список товаров");
                return;
            }

            Products.Clear();
            foreach (var product in products.Data)
            {
                Products.Add(product);
            }
        }

        private async Task CheckUserAuth()
        {
            var authedUser = await UserService.GetAuthedUserAsync();

            if (authedUser.Error != null || authedUser.Data == null)
            {
                HasOrders = false;
                return;
            }

            AuthedUser = authedUser.Data;

            var orders = await UserService.GetOrdersByUserIdAsync(AuthedUser.Id);
            HasOrders = orders.Data != null && orders.Data.Count > 0;
        }

        private void OpenLogin()
        {
            WindowManager.ShowWindow<LoginView>();
            WindowManager.CloseWindow<MainView>();
        }

        private async Task AddToOrder(object parameter)
        {
            if (!(parameter is Product product))
                return;
        }

        private async Task RemoveFromOrder(object parameter)
        {
            if (!(parameter is Product product))
                return;
        }
    }
}
