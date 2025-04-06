using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class OrderItem : BaseModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
