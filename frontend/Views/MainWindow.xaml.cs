using System.Windows;
using frontend.Services;
using frontend.ViewModels;

namespace frontend
{
    public partial class MainWindow : Window
    {
        public MainWindow(ProductService productService)
        {
            InitializeComponent();
            DataContext = new MainViewModel(productService);
        }
    }
}
