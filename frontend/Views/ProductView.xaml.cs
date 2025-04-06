using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;

namespace frontend.Views
{
    public partial class ProductView : Window
    {
        public ProductView(
            int id,
            ProductService productService,
            UserService userService,
            OrderService orderService,
            OrderItemsService orderItemsService,
            WindowManager windowManager
        )
        {
            InitializeComponent();
            DataContext = new ProductViewModel(
                id,
                productService,
                userService,
                orderService,
                orderItemsService,
                windowManager
            );
        }
    }
}
