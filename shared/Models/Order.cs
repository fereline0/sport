using System.ComponentModel.DataAnnotations;
using shared.Enums;

namespace shared.Models
{
    public class Order : BaseModel
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Inactive;
        public int? PickupPointId { get; set; }

        [Required]
        public int UserId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
