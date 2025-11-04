using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSimulation.Data
{
    /// <summary>
    /// Represents the data access layer for the farm simulation using Entity Framework and SQL Server
    /// </summary>
    public class FarmDataAccess
    {
        private readonly DbContext _context;

        public FarmDataAccess(DbContext context)
        {
            _context = context;
        }

        #region Animal Operations

        /// <summary>
        /// Adds a new animal to the database
        /// </summary>
        /// <param name="animal">The animal to add</param>
        public async Task<int> AddAnimalAsync(Animal animal)
        {
            var dbSet = _context.Set<Animal>();
            dbSet.Add(animal);
            await _context.SaveChangesAsync();
            return animal.Id;
        }

        /// <summary>
        /// Updates an existing animal in the database
        /// </summary>
        /// <param name="animal">The animal to update</param>
        public async Task UpdateAnimalAsync(Animal animal)
        {
            var dbSet = _context.Set<Animal>();
            dbSet.Update(animal);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an animal from the database
        /// </summary>
        /// <param name="animalId">The ID of the animal to delete</param>
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

        /// <summary>
        /// Gets all animals from the database
        /// </summary>
        /// <returns>List of animals</returns>
        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            var dbSet = _context.Set<Animal>();
            return await dbSet.ToListAsync();
        }

        /// <summary>
        /// Gets an animal by ID from the database
        /// </summary>
        /// <param name="animalId">The ID of the animal to get</param>
        /// <returns>The animal object</returns>
        public async Task<Animal> GetAnimalByIdAsync(int animalId)
        {
            var dbSet = _context.Set<Animal>();
            var animal = await dbSet.FindAsync(animalId);
            return animal ?? new Animal(); // Return default animal if not found
        }

        #endregion

        #region Product Operations

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product">The product to add</param>
        public async Task<int> AddProductAsync(Product product)
        {
            var dbSet = _context.Set<Product>();
            dbSet.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        /// <summary>
        /// Updates an existing product in the database
        /// </summary>
        /// <param name="product">The product to update</param>
        public async Task UpdateProductAsync(Product product)
        {
            var dbSet = _context.Set<Product>();
            dbSet.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all products from the database
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.ToListAsync();
        }

        /// <summary>
        /// Gets all unsold products from the database
        /// </summary>
        /// <returns>List of unsold products</returns>
        public async Task<List<Product>> GetUnsoldProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.Where(p => !p.IsSold).ToListAsync();
        }

        /// <summary>
        /// Gets all sold products from the database
        /// </summary>
        /// <returns>List of sold products</returns>
        public async Task<List<Product>> GetSoldProductsAsync()
        {
            var dbSet = _context.Set<Product>();
            return await dbSet.Where(p => p.IsSold).ToListAsync();
        }

        /// <summary>
        /// Deletes a product from the database by ID
        /// </summary>
        /// <param name="productId">The ID of the product to delete</param>
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

        #region Cash Operations

        /// <summary>
        /// Gets the cash from the database
        /// </summary>
        /// <returns>The cash object</returns>
        public async Task<Cash> GetCashAsync()
        {
            var dbSet = _context.Set<Cash>();
            var cash = await dbSet.FindAsync(1); // Cash always has Id = 1
            
            // If no cash record exists, create one with Id = 1
            if (cash == null)
            {
                cash = new Cash { Id = 1, Amount = 1000m }; // Start with 1000 cash as per our requirement
                dbSet.Add(cash);
                await _context.SaveChangesAsync(); // Save immediately to ensure the record exists
            }
            
            return cash;
        }

        /// <summary>
        /// Updates the cash in the database
        /// </summary>
        /// <param name="cash">The cash object to update</param>
        public async Task UpdateCashAsync(Cash cash)
        {
            var dbSet = _context.Set<Cash>();
            // Since Cash table only has one record with Id=1, we always update that record
            var existingCash = await dbSet.FindAsync(1);
            if (existingCash != null)
            {
                // Update the existing cash record
                existingCash.Amount = cash.Amount;
                dbSet.Update(existingCash);
            }
            else
            {
                // If no record exists, add the new one
                // Ensure the Id is set to 1
                cash.Id = 1;
                dbSet.Add(cash);
            }
            await _context.SaveChangesAsync();
        }

        #endregion

        /// <summary>
        /// Initializes the database and creates tables if they don't exist
        /// </summary>
        public void InitializeDatabase()
        {
            _context.Database.EnsureCreated();
        }
    }
}