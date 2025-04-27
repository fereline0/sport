using System;
using System.Linq;
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
    public class ProductViewModel : NotifyPropertyChanged
    {
        private readonly NavigationService NavigationService;
        private readonly UserService UserService;
        private readonly OrderService OrderService;
        private readonly OrderItemsService OrderItemsService;
        private readonly ProductService ProductService;

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
                    AddToOrderCommand.NotifyCanExecuteChanged();
                    RemoveFromOrderCommand.NotifyCanExecuteChanged();
                    UpdateProductCommand.NotifyCanExecuteChanged();
                    DeleteProductCommand.NotifyCanExecuteChanged();
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
                    AddToOrderCommand.NotifyCanExecuteChanged();
                    RemoveFromOrderCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private Order? _order;
        public Order? Order
        {
            get => _order;
            private set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged(nameof(Order));
                    AddToOrderCommand.NotifyCanExecuteChanged();
                    RemoveFromOrderCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private OrderItem? _orderItem;
        public OrderItem? OrderItem
        {
            get => _orderItem;
            private set
            {
                if (_orderItem != value)
                {
                    _orderItem = value;
                    OnPropertyChanged(nameof(OrderItem));
                    AddToOrderCommand.NotifyCanExecuteChanged();
                    RemoveFromOrderCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public AsyncRelayCommand AddToOrderCommand { get; }
        public AsyncRelayCommand RemoveFromOrderCommand { get; }
        public RelayCommand UpdateProductCommand { get; }
        public AsyncRelayCommand DeleteProductCommand { get; }

        public ProductViewModel(
            NavigationService navigationService,
            UserService userService,
            OrderService orderService,
            OrderItemsService orderItemsService,
            ProductService productService,
            WindowManager windowManager
        )
        {
            NavigationService = navigationService;
            UserService = userService;
            OrderService = orderService;
            OrderItemsService = orderItemsService;
            ProductService = productService;

            AddToOrderCommand = new AsyncRelayCommand(AddToOrder, CanAddToOrder);
            RemoveFromOrderCommand = new AsyncRelayCommand(RemoveFromOrder, CanRemoveFromOrder);
            UpdateProductCommand = new RelayCommand(UpdateProduct, CanUpdateProduct);
            DeleteProductCommand = new AsyncRelayCommand(DeleteProduct, CanDeleteProduct);

            LoadAuthedUserData().ConfigureAwait(false);
        }

        public void Initialize(Product product)
        {
            Product = product;
        }

        private async Task LoadAuthedUserData()
        {
            var userResult = await UserService.GetAuthedUserAsync();
            if (userResult.Error != null || userResult.Data == null)
            {
                AuthedUser = null;
                Order = null;
                OrderItem = null;
                return;
            }

            AuthedUser = userResult.Data;
            await LoadOrderDataAsync();
        }

        private async Task LoadOrderDataAsync()
        {
            if (AuthedUser == null)
                return;

            var ordersResult = await UserService.GetOrdersByUserIdAsync(
                AuthedUser.Id,
                shared.Enums.OrderStatus.Inactive
            );

            if (ordersResult.Error != null)
            {
                Order = null;
                OrderItem = null;
                return;
            }

            Order = ordersResult.Data?.FirstOrDefault();

            if (Order != null && Product != null)
            {
                await LoadOrderItemAsync();
            }
        }

        private async Task LoadOrderItemAsync()
        {
            if (Order == null || Product == null)
                return;

            var itemResult = await OrderService.GetOrderItemByOrderAndProductIdAsync(
                Order.Id,
                Product.Id
            );

            OrderItem = itemResult.Error == null ? itemResult.Data : null;
        }

        private bool CanAddToOrder() =>
            !AddToOrderCommand.IsRunning
            && AuthedUser != null
            && Product != null
            && OrderItem == null;

        private bool CanRemoveFromOrder() =>
            !RemoveFromOrderCommand.IsRunning && AuthedUser != null && OrderItem != null;

        private bool CanUpdateProduct() => AuthedUser?.Role == UserRole.Admin;

        private bool CanDeleteProduct() =>
            !DeleteProductCommand.IsRunning && AuthedUser?.Role == UserRole.Admin;

        private async Task AddToOrder()
        {
            if (Product == null || AuthedUser == null)
                return;

            if (Order == null)
            {
                var newOrder = new Order
                {
                    OrderStatus = shared.Enums.OrderStatus.Inactive,
                    UserId = AuthedUser.Id,
                };

                var orderResult = await OrderService.PostOrderAsync(newOrder);
                if (orderResult.Error != null || orderResult.Data == null)
                {
                    MessageBox.Show("Не удалось создать заказ");
                    return;
                }
                Order = orderResult.Data;
            }

            var newItem = new OrderItem { ProductId = Product.Id, OrderId = Order!.Id };

            var itemResult = await OrderItemsService.PostOrderItemAsync(newItem);
            if (itemResult.Error != null)
            {
                MessageBox.Show("Не удалось добавить товар в корзину");
                return;
            }

            await LoadOrderItemAsync();
        }

        private async Task RemoveFromOrder()
        {
            if (OrderItem == null)
                return;

            var result = await OrderItemsService.DeleteOrderItemAsync(OrderItem.Id);
            if (result.Error != null)
            {
                MessageBox.Show("Не удалось удалить товар из корзины");
                return;
            }

            await LoadOrderItemAsync();
        }

        private void UpdateProduct()
        {
            if (Product == null || AuthedUser?.Role != UserRole.Admin)
                return;

            NavigationService.NavigateTo<ProductFormPage>(Product);
        }

        private async Task DeleteProduct()
        {
            if (Product == null || AuthedUser?.Role != UserRole.Admin)
                return;

            var result = await ProductService.DeleteProductAsync(Product);
            if (result.Error != null)
            {
                MessageBox.Show("Не удалось удалить продукт");
                return;
            }

            NavigationService.NavigateTo<HomePage>();
        }
    }
}
