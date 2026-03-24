using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaFlow.Models;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //public DbSet<Medicine> Medicines { get; set; }
    //public DbSet<Category> Categories { get; set; }
    //public DbSet<Order> Orders { get; set; }
    //public DbSet<CartItem> CartItems { get; set; }
    //public DbSet<Prescription> Prescriptions { get; set; }
}