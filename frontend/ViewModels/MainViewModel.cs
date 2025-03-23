using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using frontend.Commands;
using frontend.Services;
using shared.Models;

namespace frontend.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly ProductService _productService;
        public TaskHandler LoadTaskHandler { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public RelayCommand ReloadCommand { get; }

        public MainViewModel(ProductService productService)
        {
            LoadTaskHandler = new TaskHandler(Load, 100);
            Products = new ObservableCollection<Product>();
            _productService = productService;
            ReloadCommand = new RelayCommand(LoadTaskHandler.Execute);
            LoadTaskHandler.Execute();
        }

        public async Task Load()
        {
            ServiceResult<List<Product>> products = await _productService.GetAllAsync();
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
