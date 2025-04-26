using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class Product : BaseModel
    {
        public Product()
        {
            OrderItems = new Collection<OrderItem>();
        }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public Collection<OrderItem> OrderItems { get; set; }
    }
}
