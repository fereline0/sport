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
    public partial class ProductFormPage : Page
    {
        public ProductFormPage(ProductFormViewModel productFormViewModel)
        {
            InitializeComponent();
            DataContext = productFormViewModel;
            Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (Tag is Product product && DataContext is ProductFormViewModel productFormViewModel)
            {
                productFormViewModel.Initialize(product);
            }
        }
    }
}
