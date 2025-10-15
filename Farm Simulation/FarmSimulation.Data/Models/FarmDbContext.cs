using FarmSimulation.Data.Models;
using FarmSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmSimulation.Data
{
    public class FarmDbContext : DbContext
    {
        public DbSet<AnimalBase> Animals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cash> Cash { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Buraya kendi connection string'ini yaz
                // Örn: local SQL Server için
                optionsBuilder.UseSqlServer("Server=.;Database=FarmSim;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AnimalBase soyut sınıf, EF Core bunu TPH (Table Per Hierarchy) ile yönetir
            modelBuilder.Entity<AnimalBase>()
                        .HasDiscriminator<string>("AnimalType")
                        .HasValue<Chicken>("Chicken")
                        .HasValue<Cow>("Cow")
                        .HasValue<Sheep>("Sheep")
                        .HasValue<Goat>("Goat");

            base.OnModelCreating(modelBuilder);
        }
    }
}