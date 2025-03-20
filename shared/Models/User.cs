using System.Collections.ObjectModel;

namespace shared.Models
{
    public class User : BaseModel
    {
        public User()
        {
            Orders = new Collection<Order>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public Collection<Order> Orders { get; set; }
    }
}
