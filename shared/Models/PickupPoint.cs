using System.Collections.ObjectModel;

namespace shared.Models
{
    public class PickupPoint : BaseModel
    {
        public PickupPoint()
        {
            Orders = new Collection<Order>();
        }

        public string Address { get; set; }
        public Collection<Order> Orders { get; set; }
    }
}
