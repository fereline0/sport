using System.Collections.ObjectModel;

namespace shared.Models
{
    public class Product : BaseModel
    {
        public Product()
        {
            OrderItem = new Collection<OrderItem>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public Collection<OrderItem> OrderItem { get; set; }
    }
}
