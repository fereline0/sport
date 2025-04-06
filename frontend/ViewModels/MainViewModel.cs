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
    private readonly TokenStorage TokenStorage;
    private readonly WindowManager WindowManager;

    public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
    public RelayCommand<Product> ShowDialogProductViewCommand { get; set; }
    public AsyncRelayCommand ReloadCommand { get; }
    public RelayCommand ShowLoginViewCommand { get; set; }
    public RelayCommand LogoutCommand { get; set; }

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
                ShowLoginViewCommand.NotifyCanExecuteChanged();
                LogoutCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public MainViewModel(
        ProductService productService,
        UserService userService,
        TokenStorage tokenStorage,
        WindowManager windowManager
    )
    {
        ProductService = productService;
        UserService = userService;
        TokenStorage = tokenStorage;
        WindowManager = windowManager;
        ShowDialogProductViewCommand = new RelayCommand<Product>(ShowDialogProductView);
        ReloadCommand = new AsyncRelayCommand(LoadData, CanReload);
        ShowLoginViewCommand = new RelayCommand(ShowLoginView, CanShowLoginView);
        LogoutCommand = new RelayCommand(Logout, CanLogout);

        LoadData().ConfigureAwait(false);
    }

    private bool CanShowLoginView() => AuthedUser == null;

    private bool CanLogout() => AuthedUser != null;

    private bool CanReload() => !ReloadCommand.IsRunning;

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
    }

    private void ShowLoginView()
    {
        WindowManager.ShowWindow<LoginView>();
        WindowManager.CloseWindow<MainView>();
    }

    private void Logout()
    {
        TokenStorage.ClearToken();
        AuthedUser = null;
    }

    private void ShowDialogProductView(Product? product)
    {
        if (product == null)
            return;

        WindowManager.ShowDialog<ProductView>(product.Id);
    }
}
