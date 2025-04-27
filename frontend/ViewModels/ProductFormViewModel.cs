using System.Windows;
using CommunityToolkit.Mvvm.Input;
using frontend.Services;
using frontend.Utils;
using frontend.Views;
using shared.Models;

namespace frontend.ViewModels
{
    public class ProductFormViewModel : NotifyPropertyChanged
    {
        private readonly ProductService ProductService;
        private readonly UserService UserService;
        private readonly NavigationService NavigationService;

        private Product? _product;
        public Product? Product
        {
            get => _product;
            private set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged(nameof(Product));
                }
            }
        }

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
                }
            }
        }

        public IRelayCommand SaveProductCommand { get; }

        public ProductFormViewModel(
            ProductService productService,
            UserService userService,
            NavigationService navigationService
        )
        {
            ProductService = productService;
            UserService = userService;
            NavigationService = navigationService;

            SaveProductCommand = new RelayCommand(SaveProduct);
            LoadAuthedUserData().ConfigureAwait(false);
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
            Product = new Product();
        }

        private async void SaveProduct()
        {
            if (Product == null)
                return;

            if (AuthedUser == null || AuthedUser.Role != UserRole.Admin)
            {
                MessageBox.Show(
                    "Несанкционированный доступ",
                    "Ошибка доступа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                NavigationService.NavigateTo<HomePage>();
                return;
            }

            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                MessageBox.Show("Название продукта обязательно для заполнения");
                return;
            }

            if (Product.Price < 0)
            {
                MessageBox.Show("Цена продукта должна быть неотрицательной");
                return;
            }

            if (string.IsNullOrWhiteSpace(Product.Image))
            {
                MessageBox.Show("Ссылка на изображение продукта не может быть пустой");
                return;
            }

            if (Product.Id == 0)
            {
                var result = await ProductService.PostProductAsync(Product);
                if (result.Error != null)
                {
                    MessageBox.Show("Ошибка при создании продукта");
                    return;
                }
            }
            else
            {
                var result = await ProductService.PutProductAsync(Product);
                if (
                    result.Error != null
                    && result.Error
                        != "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0."
                )
                {
                    MessageBox.Show("Ошибка при обновлении продукта");
                    return;
                }
            }

            NavigationService.NavigateTo<HomePage>();
        }

        public void Initialize(Product product)
        {
            Product = product;
        }
    }
}
