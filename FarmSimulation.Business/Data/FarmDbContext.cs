using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmSimulation.Business.Data
{
    /// <summary>
    /// Tarım simülasyonu için veritabanı bağlamını temsil eder
    /// </summary>
    public class FarmDbContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cash> Cash { get; set; }

        public FarmDbContext(DbContextOptions<FarmDbContext> options) : base(options)
        {
        }

        public FarmDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FarmSimulationDB;Trusted_Connection=true;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DateProduced).HasColumnType("datetime");
                entity.Property(e => e.ProducedByAnimalType).HasMaxLength(50);
            });

            modelBuilder.Entity<Cash>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}