using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class User : BaseModel
    {
        public User()
        {
            Orders = new Collection<Order>();
        }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public Collection<Order> Orders { get; set; }
    }
}
