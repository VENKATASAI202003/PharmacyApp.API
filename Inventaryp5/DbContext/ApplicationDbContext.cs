using Microsoft.EntityFrameworkCore;
using PharmacyOrderingSystemp5.Models;

namespace PharmacyOrderingSystemp5.Data
{
    // This class connects our app to SQL Server database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // All database tables go here
        public DbSet<User> Users { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}