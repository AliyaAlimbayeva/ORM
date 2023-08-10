using EFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFDemo.Data
{
    public class EFDemoContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFDb;" +
                "Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
