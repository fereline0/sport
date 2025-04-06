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
using System.Windows.Navigation;
using System.Windows.Shapes;
using frontend.ViewModels;
using shared.Models;

namespace frontend.Views
{
    public partial class ProductPage : Page
    {
        public ProductPage(ProductViewModel productViewModel)
        {
            InitializeComponent();
            DataContext = productViewModel;
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (Tag is Product product && DataContext is ProductViewModel viewModel)
            {
                await viewModel.Initialize(product);
            }
        }
    }
}
