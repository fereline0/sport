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
        private readonly ProductService ProductService;
        private readonly UserService UserService;
        private readonly NavigationService NavigationService;

        public ObservableCollection<Product> Products { get; } =
            new ObservableCollection<Product>();

        public IRelayCommand ShowAddProductFormCommand { get; }

        private User? _authedUser;
        public User? AuthedUser
        {
            get => _authedUser;
            private set
            {
                if (_authedUser != value)
                {
                    _authedUser = value;
                    OnPropertyChanged(nameof(AuthedUser));
                    ShowAddProductFormCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public IRelayCommand<Product> ShowProductCommand { get; }

        public HomeViewModel(
            ProductService productService,
            UserService userService,
            NavigationService navigationService
        )
        {
            ProductService = productService;
            UserService = userService;
            NavigationService = navigationService;

            ShowProductCommand = new RelayCommand<Product>(ShowProduct);
            ShowAddProductFormCommand = new RelayCommand(ShowAddProductForm, CanShowAddProductForm);

            LoadProducts().ConfigureAwait(false);
            LoadAuthedUserData().ConfigureAwait(false);
        }

        private async Task LoadProducts()
        {
            var products = await ProductService.GetProductsAsync();

            if (products.Error != null || products.Data == null)
            {
                MessageBox.Show("Не удалось загрузить список товаров или он пуст");
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
            var userResult = await UserService.GetAuthedUserAsync();
            if (userResult.Error != null || userResult.Data == null)
            {
                AuthedUser = null;
                return;
            }

            AuthedUser = userResult.Data;
        }

        private void ShowAddProductForm()
        {
            if (AuthedUser?.Role != UserRole.Admin)
            {
                MessageBox.Show("Нессанкционированный доступ");
                return;
            }

            NavigationService.NavigateTo<ProductFormPage>();
        }

        private bool CanShowAddProductForm()
        {
            return AuthedUser?.Role == UserRole.Admin;
        }

        private void ShowProduct(Product? product)
        {
            if (product == null)
                return;

            NavigationService.NavigateTo<ProductPage>(product);
        }
    }
}
