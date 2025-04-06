using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace shared.Models
{
    public class Auth
    {
        [Required]
        public string? token { get; set; }
    }
}
