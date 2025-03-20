using System.ComponentModel.DataAnnotations.Schema;

namespace shared.Models
{
    public class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
