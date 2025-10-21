﻿﻿﻿﻿using FarmSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmSimulation.Data
{
    public class FarmDbContext : DbContext
    {
        public DbSet<AnimalBase> Animals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cash> Cash { get; set; }
        public DbSet<AnimalLifecycle> AnimalLifecycles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=JEFT;Database=FarmSimulation;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AnimalBase için Primary Key yapılandırması
            modelBuilder.Entity<AnimalBase>()
                        .HasKey(a => a.Id);

            // AnimalBase soyut sınıfı için TPH (Table Per Hierarchy) yapılandırması
            modelBuilder.Entity<AnimalBase>()
                        .HasDiscriminator<string>("Ad")
                        .HasValue<Chicken>("Tavuk")
                        .HasValue<Cow>("İnek")
                        .HasValue<Sheep>("Koyun")
                        .HasValue<Goat>("Keçi");

            // AnimalLifecycle yapılandırması
            modelBuilder.Entity<AnimalLifecycle>()
                        .ToTable("AnimalLifecycle")
                        .HasKey(l => l.AnimalId);

            modelBuilder.Entity<AnimalLifecycle>()
                        .HasOne(l => l.Animal)
                        .WithOne(a => a.Lifecycle)
                        .HasForeignKey<AnimalLifecycle>(l => l.AnimalId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Product yapılandırması
            modelBuilder.Entity<Product>()
                        .HasKey(p => p.Id);

            // Cash yapılandırması
            modelBuilder.Entity<Cash>()
                        .HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}