using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class PickupPoint : BaseModel
    {
        public PickupPoint()
        {
            Orders = new Collection<Order>();
        }

        [Required]
        public string? Address { get; set; }
        public Collection<Order> Orders { get; set; }
    }
}
