using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using shared.Models;

namespace frontend.ViewModels
{
    public class ProductViewModel : NotifyPropertyChanged
    {
        private int Id;
        private readonly ProductService ProductService;
        private readonly UserService UserService;
        private readonly OrderService OrderService;
        private readonly OrderItemsService _orderItemsService;

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

        public ProductViewModel(
            ProductService productService,
            UserService userService,
            OrderService orderService,
            OrderItemsService orderItemsService,
            WindowManager windowManager
        )
        {
            ProductService = productService;
            UserService = userService;
            OrderService = orderService;
            _orderItemsService = orderItemsService;

            AddToOrderCommand = new AsyncRelayCommand(AddToOrder, CanAddToOrder);
            RemoveFromOrderCommand = new AsyncRelayCommand(RemoveFromOrder, CanRemoveFromOrder);
        }

        public async Task Initialize(Product product)
        {
            Id = product.Id;
            await LoadData();
        }

        private async Task LoadData()
        {
            await Task.WhenAll(LoadProduct(), LoadAuthedUserData());
        }

        private async Task LoadProduct()
        {
            var result = await ProductService.GetProductAsync(Id);
            if (result.Error != null || result.Data == null)
            {
                MessageBox.Show("Продукт не найден");
                return;
            }

            Product = result.Data;
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
            if (AuthedUser == null || Product == null)
                return;

            var ordersResult = await UserService.GetOrdersByUserIdAsync(
                AuthedUser.Id,
                shared.Enums.OrderStatus.Inactive
            );

            if (ordersResult.Error != null || ordersResult.Data == null)
            {
                Order = null;
                OrderItem = null;
                return;
            }

            Order = ordersResult.Data[0];
            await LoadOrderItemAsync();
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

            var itemResult = await _orderItemsService.PostOrderItemAsync(newItem);
            if (itemResult.Error != null)
            {
                MessageBox.Show("Не удалось добавить товар в корзину");
                return;
            }

            await LoadAuthedUserData();
        }

        private async Task RemoveFromOrder()
        {
            if (OrderItem == null)
                return;

            var result = await _orderItemsService.DeleteOrderItemAsync(OrderItem.Id);
            if (result.Error != null)
            {
                MessageBox.Show("Не удалось удалить товар из корзины");
                return;
            }

            OrderItem = null;
        }
    }
}
