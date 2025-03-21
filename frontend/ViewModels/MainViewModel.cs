using System.Collections.ObjectModel;
using System.Windows;
using frontend.Services;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly ProductService _productService;
        public ObservableCollection<Product> Products { get; set; }

        public MainViewModel(ProductService productService)
        {
            Products = new ObservableCollection<Product>();
            _productService = productService;
            LoadProducts();
        }

        public async void LoadProducts()
        {
            List<Product> products = await _productService.GetAllAsync();
            Products.Clear();
            foreach (Product product in products)
            {
                Products.Add(product);
            }
        }
    }
}
