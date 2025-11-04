using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSimulation.Data
{
    // Tarım simülasyonu için veri erişim katmanını temsil eder
    public class FarmDataAccess
    {
        private readonly DbContext _context;

        public FarmDataAccess(DbContext context)
        {
            _context = context;
        }

        #region Hayvan İşlemleri

        public async Task<int> AddAnimalAsync(Animal animal)
        {
            var dbSet = _context.Set<Animal>();
            dbSet.Add(animal);
            await _context.SaveChangesAsync();
            return animal.Id;
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            var dbSet = _context.Set<Animal>();
            dbSet.Update(animal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            var dbSet = _context.Set<Animal>();
            var animal = await dbSet.FindAsync(animalId);
            if (animal != null)
            {
                dbSet.Remove(animal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            var dbSet = _context.Set<Animal>();
            return await dbSet.ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int animalId)
        {
            var dbSet = _context.Set<Animal>();
            var animal = await dbSet.FindAsync(animalId);
            return animal ?? new Animal();
        }

        #endregion

        #region Ürün İşlemleri

        public async Task<int> AddProductAsync(Product product)
        {
            var dbSet = _context.Set<Product>();
            dbSet.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var dbSet = _context.Set<Product>();
            dbSet.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.ToListAsync();
        }

        public async Task<List<Product>> GetUnsoldProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.Where(p => !p.IsSold).ToListAsync();
        }

        public async Task<List<Product>> GetSoldProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.Where(p => p.IsSold).ToListAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var dbSet = _context.Set<Product>();
            var product = await dbSet.FindAsync(productId);
            if (product != null)
            {
                dbSet.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Nakit İşlemleri

        public async Task<Cash> GetCashAsync()
        {
            var dbSet = _context.Set<Cash>();
            var cash = await dbSet.FindAsync(1);
            
            if (cash == null)
            {
                cash = new Cash { Id = 1, Amount = 1000m };
                dbSet.Add(cash);
                await _context.SaveChangesAsync();
            }
            
            return cash;
        }

        public async Task UpdateCashAsync(Cash cash)
        {
            var dbSet = _context.Set<Cash>();
            var existingCash = await dbSet.FindAsync(1);
            if (existingCash != null)
            {
                existingCash.Amount = cash.Amount;
                dbSet.Update(existingCash);
            }
            else
            {
                cash.Id = 1;
                dbSet.Add(cash);
            }
            await _context.SaveChangesAsync();
        }

        #endregion

        public void InitializeDatabase()
        {
            _context.Database.EnsureCreated();
        }
    }
}