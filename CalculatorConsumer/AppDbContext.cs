using CalculatorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatorApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Calculation> Calculations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=CalculationsDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}