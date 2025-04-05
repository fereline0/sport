using System.Windows;
using System.Windows.Controls;

namespace frontend.Components
{
    public partial class ProductPreview : UserControl
    {
        public static readonly DependencyProperty NameProductProperty = DependencyProperty.Register(
            "NameProduct",
            typeof(string),
            typeof(ProductPreview),
            new PropertyMetadata(string.Empty)
        );

        public string NameProduct
        {
            get { return (string)GetValue(NameProductProperty); }
            set { SetValue(NameProductProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(ProductPreview),
            new PropertyMetadata(string.Empty)
        );

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty PriceProperty = DependencyProperty.Register(
            "Price",
            typeof(decimal),
            typeof(ProductPreview),
            new PropertyMetadata(0m)
        );

        public decimal Price
        {
            get { return (decimal)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image",
            typeof(string),
            typeof(ProductPreview),
            new PropertyMetadata(string.Empty)
        );

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty EndContentProperty = DependencyProperty.Register(
            "EndContent",
            typeof(UIElement),
            typeof(ProductPreview),
            new PropertyMetadata(null)
        );

        public UIElement EndContent
        {
            get { return (UIElement)GetValue(EndContentProperty); }
            set { SetValue(EndContentProperty, value); }
        }

        public ProductPreview()
        {
            InitializeComponent();
        }
    }
}
