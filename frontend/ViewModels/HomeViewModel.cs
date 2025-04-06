using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class HomeViewModel : NotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private readonly NavigationService _navigationService;

        public ObservableCollection<Product> Products { get; } =
            new ObservableCollection<Product>();

        public IRelayCommand<Product> ShowProductCommand { get; }

        public HomeViewModel(ProductService productService, NavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            ShowProductCommand = new RelayCommand<Product>(ShowProduct);

            LoadProducts().ConfigureAwait(false);
        }

        private async Task LoadProducts()
        {
            var products = await _productService.GetProductsAsync();

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

        private void ShowProduct(Product? product)
        {
            if (product == null)
                return;

            _navigationService.NavigateTo<ProductPage>(product);
        }
    }
}
