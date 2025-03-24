using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using frontend.Commands;
using frontend.Services;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private readonly AuthService _authService;
        private readonly MainView _mainView;
        public TaskHandler LoadTaskHandler { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public AsyncRelayCommand ReloadCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        public MainViewModel(
            MainView mainView,
            ProductService productService,
            AuthService authService
        )
        {
            LoadTaskHandler = new TaskHandler(Load);
            Products = new ObservableCollection<Product>();
            _mainView = mainView;
            _productService = productService;
            _authService = authService;
            ReloadCommand = new AsyncRelayCommand(LoadTaskHandler.Execute);
            LoginCommand = new RelayCommand(OpenLogin);
            LoadTaskHandler.Execute();
        }

        public void OpenLogin()
        {
            LoginView loginView = new LoginView(_authService);
            loginView.Show();
            _mainView.Close();
        }

        public async Task Load()
        {
            ServiceResult<List<Product>> products = await _productService.GetProductsAsync();
            List<Product> data = products.Data!;

            if (products.Error != null)
            {
                return;
            }

            Products.Clear();
            foreach (Product product in data)
            {
                Products.Add(product);
            }
        }
    }
}
