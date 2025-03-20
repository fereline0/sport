using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace backend.Data
{
    public class backendContext : DbContext
    {
        public backendContext (DbContextOptions<backendContext> options)
            : base(options)
        {
        }

        public DbSet<shared.Models.User> User { get; set; } = default!;
        public DbSet<shared.Models.Order> Order { get; set; } = default!;
        public DbSet<shared.Models.OrderItem> OrderItem { get; set; } = default!;
        public DbSet<shared.Models.PickupPoint> PickupPoint { get; set; } = default!;
        public DbSet<shared.Models.Product> Product { get; set; } = default!;
    }
}
