using System.Windows;
using frontend.Services;
using frontend.ViewModels;

namespace frontend
{
    public partial class MainView : Window
    {
        public MainView(ProductService productService, AuthService authService)
        {
            InitializeComponent();
            DataContext = new MainViewModel(this, productService, authService);
        }
    }
}
