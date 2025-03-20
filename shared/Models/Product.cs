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
        public float Price { get; set; }
        public float? Discount { get; set; }
        public Collection<OrderItem> OrderItem { get; set; }
    }
}
