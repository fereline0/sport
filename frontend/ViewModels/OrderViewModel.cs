using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using frontend.Services;
using frontend.Utils;
using frontend.Views;
using shared.Enums;
using shared.Models;

namespace frontend.ViewModels
{
    public class OrderViewModel : NotifyPropertyChanged
    {
        private readonly ProductService ProductService;
        private readonly OrderService OrderService;
        private readonly PickupPointService PickupPointService;
        private readonly NavigationService NavigationService;

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
                    LoadProducts().ConfigureAwait(false);
                }
            }
        }

        public ObservableCollection<Product> Products { get; } =
            new ObservableCollection<Product>();
        public ObservableCollection<PickupPoint> PickupPoints { get; } =
            new ObservableCollection<PickupPoint>();

        private PickupPoint? _selectedPickupPoint;
        public PickupPoint? SelectedPickupPoint
        {
            get => _selectedPickupPoint;
            set
            {
                if (_selectedPickupPoint != value)
                {
                    _selectedPickupPoint = value;
                    OnPropertyChanged(nameof(SelectedPickupPoint));
                }
            }
        }

        public IRelayCommand<Product> ShowProductCommand { get; }
        public IRelayCommand PlaceOrderCommand { get; }

        public OrderViewModel(
            ProductService productService,
            PickupPointService pickupPointService,
            OrderService orderService,
            NavigationService navigationService
        )
        {
            ProductService = productService;
            PickupPointService = pickupPointService;
            OrderService = orderService;
            NavigationService = navigationService;

            ShowProductCommand = new RelayCommand<Product>(ShowProduct);
            PlaceOrderCommand = new RelayCommand(async () => await PutOrder());
            LoadPickupPoints().ConfigureAwait(false);
        }

        private async Task LoadProducts()
        {
            if (Order == null)
            {
                MessageBox.Show("Заказ не был инициализирован");
                NavigationService.NavigateTo<HomePage>();
            }

            var products = await ProductService.GetProductsByOrderAsync(Order.Id);

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

        private async Task LoadPickupPoints()
        {
            var pickupPointsResult = await PickupPointService.GetPickupPointsAsync();

            if (pickupPointsResult.Error != null || pickupPointsResult.Data == null)
            {
                MessageBox.Show("Не удалось загрузить список пунктов выдачи");
                return;
            }

            PickupPoints.Clear();
            foreach (var pickupPoint in pickupPointsResult.Data)
            {
                PickupPoints.Add(pickupPoint);
            }
        }

        public void Initialize(Order order)
        {
            Order = order;
        }

        private void ShowProduct(Product? product)
        {
            if (product == null)
                return;

            NavigationService.NavigateTo<ProductPage>(product);
        }

        private async Task PutOrder()
        {
            if (Order == null)
            {
                MessageBox.Show("Заказ не был инициализирован");
                return;
            }

            if (SelectedPickupPoint == null)
            {
                MessageBox.Show("Пожалуйста, выберите пункт выдачи");
                return;
            }

            Order.PickupPointId = SelectedPickupPoint.Id;
            Order.OrderStatus = OrderStatus.New;

            var result = await OrderService.PutOrderAsync(Order);
            if (
                result.Error != null
                && result.Error
                    != "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0."
                && result.Data == null
            )
            {
                MessageBox.Show("Не удалось обновить заказ");
            }
            else
            {
                MessageBox.Show("Заказ успешно сформирован");
                NavigationService.NavigateTo<HomePage>();
            }
        }
    }
}
