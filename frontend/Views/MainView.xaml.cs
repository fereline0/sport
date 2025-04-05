using System.ComponentModel;
using System.Windows;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;
using frontend.Views;

namespace frontend
{
    public partial class MainView : Window
    {
        public MainView(
            ProductService productService,
            UserService userService,
            OrderItemsService orderItemsService,
            WindowManager windowManager
        )
        {
            InitializeComponent();
            DataContext = new MainViewModel(
                productService,
                userService,
                orderItemsService,
                windowManager
            );
        }
    }
}
