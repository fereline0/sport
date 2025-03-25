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
        private readonly WindowManager WindowManager;

        public TaskHandler LoadTaskHandler { get; }
        public ObservableCollection<Product> Products { get; }
        public AsyncRelayCommand ReloadCommand { get; }
        public RelayCommand LoginCommand { get; }

        public MainViewModel(ProductService productService, WindowManager windowManager)
        {
            ProductService = productService;
            WindowManager = windowManager;

            LoadTaskHandler = new TaskHandler(Load);
            Products = new ObservableCollection<Product>();

            ReloadCommand = new AsyncRelayCommand(LoadTaskHandler.Execute, CanReload);
            LoginCommand = new RelayCommand(OpenLogin);

            LoadTaskHandler.Execute();
        }

        private bool CanReload() => LoadTaskHandler.Loaded;

        private void OpenLogin()
        {
            WindowManager.ShowWindow<LoginView>();
            WindowManager.CloseWindow<MainView>();
        }

        public async Task Load()
        {
            var products = await ProductService.GetProductsAsync();

            if (products.Error != null || products.Data == null)
            {
                MessageBox.Show("Error loading products");
                return;
            }

            Products.Clear();
            foreach (var product in products.Data)
            {
                Products.Add(product);
            }
        }
    }
}
