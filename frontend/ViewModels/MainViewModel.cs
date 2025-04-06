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

public class MainViewModel : NotifyPropertyChanged
{
    private readonly ProductService ProductService;
    private readonly UserService UserService;
    private readonly OrderService OrderService;
    private readonly OrderItemsService OrderItemsService;
    private readonly TokenStorage TokenStorage;
    private readonly WindowManager WindowManager;

    public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
    public AsyncRelayCommand ReloadCommand { get; }
    public RelayCommand LoginCommand { get; set; }
    public RelayCommand LogoutCommand { get; set; }
    public AsyncRelayCommand<Product> AddToOrderCommand { get; }
    public AsyncRelayCommand<Product> RemoveFromOrderCommand { get; }

    private bool _isLoggedIn;
    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set
        {
            if (_isLoggedIn != value)
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
                AddToOrderCommand.NotifyCanExecuteChanged();
                RemoveFromOrderCommand.NotifyCanExecuteChanged();
                LoginCommand.NotifyCanExecuteChanged();
                LogoutCommand.NotifyCanExecuteChanged();
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
                IsLoggedIn = _authedUser != null;
                OnPropertyChanged(nameof(AuthedUser));
            }
        }
    }
    private bool _hasOrder;
    public bool HasOrder
    {
        get => _hasOrder;
        set
        {
            if (_hasOrder != value)
            {
                _hasOrder = value;
                OnPropertyChanged(nameof(HasOrder));
                RemoveFromOrderCommand.NotifyCanExecuteChanged();
            }
        }
    }
    private Order? _order;
    public Order? Order
    {
        get => _order;
        set
        {
            if (_order != value)
            {
                _order = value;
                HasOrder = _order != null;
            }
        }
    }
    public ObservableCollection<OrderItem> OrderItems { get; } =
        new ObservableCollection<OrderItem>();

    public MainViewModel(
        ProductService productService,
        UserService userService,
        OrderService orderService,
        OrderItemsService orderItemsService,
        TokenStorage tokenStorage,
        WindowManager windowManager
    )
    {
        ProductService = productService;
        UserService = userService;
        OrderService = orderService;
        OrderItemsService = orderItemsService;
        TokenStorage = tokenStorage;
        WindowManager = windowManager;
        ReloadCommand = new AsyncRelayCommand(LoadData, CanReload);
        LoginCommand = new RelayCommand(OpenLogin, CanLogin);
        LogoutCommand = new RelayCommand(Logout, CanLogout);
        AddToOrderCommand = new AsyncRelayCommand<Product>(AddToOrder, CanAddToOrder);
        RemoveFromOrderCommand = new AsyncRelayCommand<Product>(
            RemoveFromOrder,
            CanRemoveFromOrder
        );

        LoadData().ConfigureAwait(false);
    }

    private bool CanLogin() => !IsLoggedIn;

    private bool CanLogout() => IsLoggedIn;

    private bool CanReload() => !ReloadCommand.IsRunning;

    public bool CanAddToOrder(Product? product) => !AddToOrderCommand.IsRunning && IsLoggedIn;

    private bool CanRemoveFromOrder(Product? product) =>
        !RemoveFromOrderCommand.IsRunning && IsLoggedIn && HasOrder;

    private async Task LoadData()
    {
        await Task.WhenAll(LoadProducts(), LoadAuthedUserData());
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

    private async Task LoadAuthedUserData()
    {
        var authedUserResult = await UserService.GetAuthedUserAsync();

        if (authedUserResult.Error != null || authedUserResult.Data == null)
        {
            return;
        }

        AuthedUser = authedUserResult.Data;

        var ordersResult = await UserService.GetOrdersByUserIdAsync(
            AuthedUser.Id,
            shared.Enums.OrderStatus.Inactive
        );

        if (ordersResult.Error != null || ordersResult.Data == null)
        {
            return;
        }

        Order = ordersResult.Data[0];
    }

    private void OpenLogin()
    {
        WindowManager.ShowWindow<LoginView>();
        WindowManager.CloseWindow<MainView>();
    }

    private void Logout()
    {
        TokenStorage.ClearToken();
        AuthedUser = null;
    }

    private async Task AddToOrder(Product? product)
    {
        if (product == null)
            return;

        if (HasOrder)
        {
            var orderItemResult = await OrderService.GetOrderItemByOrderAndProductIdAsync(
                Order!.Id,
                product.Id
            );

            if (orderItemResult.Error == null && orderItemResult.Data != null)
            {
                MessageBox.Show("Данный товар уже добавлен");
                return;
            }
        }
        else
        {
            var newOrder = new Order
            {
                OrderStatus = shared.Enums.OrderStatus.Inactive,
                UserId = AuthedUser!.Id,
            };

            var postedOrderResult = await OrderService.PostOrderAsync(newOrder);

            if (postedOrderResult.Error != null || postedOrderResult.Data == null)
            {
                MessageBox.Show("Не удалось создать новый заказ");
                return;
            }

            Order = postedOrderResult.Data;
        }

        var newOrderItem = new OrderItem { ProductId = product.Id, OrderId = Order!.Id };

        var postedOrderItemResult = await OrderItemsService.PostOrderItemAsync(newOrderItem);

        if (postedOrderItemResult.Error != null)
        {
            MessageBox.Show("Не удалось добавить товар в заказ");
            return;
        }

        await LoadData();
    }

    private async Task RemoveFromOrder(Product? product)
    {
        if (product == null)
            return;

        var orderItemResult = await OrderService.GetOrderItemByOrderAndProductIdAsync(
            Order!.Id,
            product.Id
        );

        if (orderItemResult.Error != null || orderItemResult.Data == null)
        {
            MessageBox.Show("Товар отсутсвует в заказе");
            return;
        }

        var deletedOrderItemResult = await OrderItemsService.DeleteOrderItemAsync(
            orderItemResult.Data.Id
        );

        if (deletedOrderItemResult.Error != null)
        {
            MessageBox.Show("Не удалось удалить товар из заказа");
        }
    }
}
