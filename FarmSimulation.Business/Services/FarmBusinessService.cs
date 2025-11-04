using FarmSimulation.Data;
using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSimulation.Business.Services
{
    /// <summary>
    /// Manages the business logic for the farm simulation using Entity Framework and SQL Server
    /// </summary>
    public class FarmBusinessService
    {
        private readonly FarmDataAccess _dataAccess;
        private DateTime _lastSimulatedTime;
        public FarmDataAccess dataAccess => _dataAccess; // Public accessor for data access
        public List<Animal> Animals { get; private set; }
        public List<Product> Products { get; private set; }
        public Cash Cash { get; private set; }

        public FarmBusinessService(FarmDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            Animals = new List<Animal>();
            Products = new List<Product>();
            Cash = new Cash(); // Start with 0 cash as per requirements
            _lastSimulatedTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes the farm data from the database
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadDataFromDatabaseAsync();
        }

        /// <summary>
        /// Loads all data from the database
        /// </summary>
        private async Task LoadDataFromDatabaseAsync()
        {
            Animals = await _dataAccess.GetAllAnimalsAsync();
            Products = await _dataAccess.GetAllProductsAsync();
            Cash = await _dataAccess.GetCashAsync();
        }

        /// <summary>
        /// Adds a new animal to the farm and database
        /// </summary>
        /// <param name="animal">The animal to add</param>
        public async Task<int> AddAnimalAsync(Animal animal)
        {
            int animalId = await _dataAccess.AddAnimalAsync(animal);
            Animals.Add(animal);
            return animalId;
        }

        /// <summary>
        /// Updates an existing animal in the farm and database
        /// </summary>
        /// <param name="animal">The animal to update</param>
        public async Task UpdateAnimalAsync(Animal animal)
        {
            await _dataAccess.UpdateAnimalAsync(animal);
            
            // Update the animal in the local list as well
            var localAnimal = Animals.FirstOrDefault(a => a.Id == animal.Id);
            if (localAnimal != null)
            {
                localAnimal.Name = animal.Name;
                localAnimal.Age = animal.Age;
                localAnimal.Gender = animal.Gender;
                localAnimal.Type = animal.Type;
                localAnimal.DateOfBirth = animal.DateOfBirth;
                localAnimal.MaxAge = animal.MaxAge;
                localAnimal.ProductProductionTime = animal.ProductProductionTime;
                localAnimal.ProductProductionProgress = animal.ProductProductionProgress;
                localAnimal.IsAlive = animal.IsAlive;
                localAnimal.CanProduce = animal.CanProduce;
                localAnimal.IsReadyToSell = animal.IsReadyToSell;
            }
        }

        /// <summary>
        /// Deletes an animal from the farm and database
        /// </summary>
        /// <param name="animalId">The ID of the animal to delete</param>
        public async Task DeleteAnimalAsync(int animalId)
        {
            await _dataAccess.DeleteAnimalAsync(animalId);
            
            // Remove the animal from the local list as well
            Animals.RemoveAll(a => a.Id == animalId);
        }

        /// <summary>
        /// Adds a new product to the farm and database
        /// </summary>
        /// <param name="product">The product to add</param>
        public async Task<int> AddProductAsync(Product product)
        {
            int productId = await _dataAccess.AddProductAsync(product);
            Products.Add(product);
            return productId;
        }

        /// <summary>
        /// Updates an existing product in the farm and database
        /// </summary>
        /// <param name="product">The product to update</param>
        public async Task UpdateProductAsync(Product product)
        {
            await _dataAccess.UpdateProductAsync(product);
            
            // Update the product in the local list as well
            var localProduct = Products.FirstOrDefault(p => p.Id == product.Id);
            if (localProduct != null)
            {
                localProduct.ProductType = product.ProductType;
                localProduct.Name = product.Name;
                localProduct.Quantity = product.Quantity;
                localProduct.Price = product.Price;
                localProduct.DateProduced = product.DateProduced;
                localProduct.IsSold = product.IsSold;
                localProduct.ProducedByAnimalType = product.ProducedByAnimalType;
            }
        }

        /// <summary>
        /// Deletes a product from the farm and database
        /// </summary>
        /// <param name="productId">The ID of the product to delete</param>
        public async Task DeleteProductAsync(int productId)
        {
            await _dataAccess.DeleteProductAsync(productId);
            
            // Remove the product from the local list as well
            Products.RemoveAll(p => p.Id == productId);
        }

        /// <summary>
        /// Gets an animal by ID from the farm
        /// </summary>
        /// <param name="animalId">The ID of the animal to get</param>
        /// <returns>The animal object</returns>
        public Animal GetAnimalById(int animalId)
        {
            return Animals.FirstOrDefault(a => a.Id == animalId) ?? new Animal();
        }

        /// <summary>
        /// Gets a product by ID from the farm
        /// </summary>
        /// <param name="productId">The ID of the product to get</param>
        /// <returns>The product object</returns>
        public Product GetProductById(int productId)
        {
            return Products.FirstOrDefault(p => p.Id == productId) ?? new Product();
        }

        /// <summary>
        /// Simulates one tick of the farm simulation (e.g., 1 second)
        /// </summary>
        public async Task SimulateTickAsync()
        {
            int aliveAnimals = 0;
            int deadAnimals = 0;
            
            // Calculate elapsed time since last simulation
            DateTime currentTime = DateTime.Now;
            var timeSinceLastSimulation = currentTime - _lastSimulatedTime;
            _lastSimulatedTime = currentTime;
            
            // Define a threshold for aging - animals age 1 day every 300 seconds (5 minutes) of real time
            // This can be tuned as needed for gameplay balance
            const int SECONDS_PER_GAME_DAY = 300; // 5 minutes real time = 1 game day

            foreach (var animal in Animals.ToList()) // Use ToList() to avoid collection modification issues
            {
                if (animal.IsAlive)
                {
                    // Update product production progress
                    if (animal.CanProduce && animal.ProductProductionTime > 0)
                    {
                        animal.ProductProductionProgress += 100 / animal.ProductProductionTime; // Increment progress based on time

                        // Check if product production is complete
                        if (animal.ProductProductionProgress >= 100)
                        {
                            // Create a new product based on the animal type
                            var product = CreateProductForAnimal(animal);
                            await AddProductAsync(product);

                            // Reset production progress
                            animal.ProductProductionProgress = 0;
                            
                            // Update animal in database
                            await UpdateAnimalAsync(animal);
                        }
                    }
                    
                    aliveAnimals++;
                }
                
                // Age the animal based on elapsed time since last simulation
                // Only animals that are alive should age
                if (animal.IsAlive) 
                {
                    // Calculate how many game days have passed since last simulation for this animal
                    int daysToAge = (int)(timeSinceLastSimulation.TotalSeconds / SECONDS_PER_GAME_DAY);
                    
                    if (daysToAge > 0)
                    {
                        animal.Age += daysToAge;
                    }
                }
                
                // Check if animal should die based on age
                if (animal.IsAlive && animal.Age >= animal.MaxAge)
                {
                    animal.IsAlive = false;
                    deadAnimals++;
                }
            }
            
            // Remove dead animals from the list
            Animals.RemoveAll(a => !a.IsAlive);
            
            // Save changes to database
            await SaveChangesToDatabaseAsync();
        }

        /// <summary>
        /// Creates a product based on the animal type
        /// </summary>
        /// <param name="animal">The animal that produces the product</param>
        /// <returns>A new product instance</returns>
        private Product CreateProductForAnimal(Animal animal)
        {
            var product = new Product
            {
                ProductType = GetProductTypeByAnimal(animal.Type),
                Name = GetProductNameByAnimal(animal.Type),
                Quantity = 1, // Default quantity
                Price = GetProductPriceByAnimal(animal.Type),
                ProducedByAnimalType = animal.Type,
                DateProduced = DateTime.Now
            };

            return product;
        }

        /// <summary>
        /// Gets the product type based on the animal type
        /// </summary>
        /// <param name="animalType">The type of animal</param>
        /// <returns>The product type produced by the animal</returns>
        private string GetProductTypeByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => "Egg",
                "cow" => "Milk",
                "sheep" => "Wool",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets the product name based on the animal type
        /// </summary>
        /// <param name="animalType">The type of animal</param>
        /// <returns>The product name</returns>
        private string GetProductNameByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => "Egg",
                "cow" => "Milk",
                "sheep" => "Wool",
                _ => "Unknown Product"
            };
        }

        /// <summary>
        /// Gets the product price based on the animal type
        /// </summary>
        /// <param name="animalType">The type of animal</param>
        /// <returns>The product price</returns>
        private decimal GetProductPriceByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => 0.5m, // Egg price
                "cow" => 2.5m,     // Milk price
                "sheep" => 3.0m,   // Wool price
                _ => 0m
            };
        }

        /// <summary>
        /// Sells all unsold products and adds the proceeds to cash
        /// </summary>
        /// <returns>The total amount earned from selling products</returns>
        public async Task<decimal> SellProductsAsync()
        {
            decimal totalEarnings = 0m;

            // Find all unsold products
            var unsoldProducts = Products.Where(p => !p.IsSold).ToList();

            foreach (var product in unsoldProducts)
            {
                totalEarnings += product.Price * product.Quantity;
                product.IsSold = true; // Mark as sold
                
                // Update product in database
                await UpdateProductAsync(product);
            }

            // Add earnings to cash
            Cash.Amount += totalEarnings;
            await _dataAccess.UpdateCashAsync(Cash);

            return totalEarnings;
        }

        /// <summary>
        /// Gets the price for buying a specific animal type
        /// </summary>
        /// <param name="animalType">The type of animal to buy</param>
        /// <returns>The price of the animal</returns>
        public decimal GetAnimalPrice(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => 10m,  // Chicken price
                "cow" => 500m,     // Cow price
                "sheep" => 150m,   // Sheep price
                _ => 0m
            };
        }

        /// <summary>
        /// Creates a new animal based on the specified type
        /// </summary>
        /// <param name="animalType">The type of animal to create</param>
        /// <param name="name">The name of the animal</param>
        /// <returns>A new animal instance</returns>
        public Animal CreateAnimal(string animalType, string name)
        {
            var animal = new Animal
            {
                Name = name,
                Type = animalType,
                Gender = new Random().Next(0, 2) == 0 ? "Male" : "Female", // Random gender
                DateOfBirth = DateTime.Now
            };

            // Set animal-specific properties
            switch (animalType.ToLower())
            {
                case "chicken":
                    animal.MaxAge = 15; // 15 days
                    animal.ProductProductionTime = 30; // 30 seconds
                    break;
                case "cow":
                    animal.MaxAge = 20; // 20 days
                    animal.ProductProductionTime = 60; // 60 seconds
                    break;
                case "sheep":
                    animal.MaxAge = 25; // 25 days
                    animal.ProductProductionTime = 90; // 90 seconds
                    break;
                default:
                    animal.MaxAge = 10; // Default max age
                    animal.ProductProductionTime = 60; // Default production time
                    break;
            }

            return animal;
        }

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        private async Task SaveChangesToDatabaseAsync()
        {
            // Update all animals in the database
            foreach (var animal in Animals)
            {
                await _dataAccess.UpdateAnimalAsync(animal);
            }

            // Update cash in the database
            await _dataAccess.UpdateCashAsync(Cash);
        }

        /// <summary>
        /// Collects a product from a specific animal
        /// </summary>
        /// <param name="animal">The animal to collect product from</param>
        /// <returns>The collected product</returns>
        public async Task<Product> CollectProductFromAnimalAsync(Animal animal)
        {
            if (animal != null && animal.IsAlive && animal.CanProduce)
            {
                // Create a new product based on the animal type
                var product = CreateProductForAnimal(animal);
                await AddProductAsync(product);
                
                // Reset production progress
                animal.ProductProductionProgress = 0;
                
                // Update animal in database
                await UpdateAnimalAsync(animal);
                
                return product;
            }
            return new Product(); // Return a default product instead of null
        }

        /// <summary>
        /// Collects products from all animals that can produce
        /// </summary>
        /// <returns>List of collected products</returns>
        public async Task<List<Product>> CollectAllProductsAsync()
        {
            var collectedProducts = new List<Product>();
            
            // Collect products from all animals that can produce
            foreach (var animal in Animals.Where(a => a.IsAlive && a.CanProduce).ToList())
            {
                var product = await CollectProductFromAnimalAsync(animal);
                if (product != null)
                {
                    collectedProducts.Add(product);
                }
            }
            
            return collectedProducts;
        }

        /// <summary>
        /// Deletes all sold products from the database and updates the local list
        /// </summary>
        /// <returns>Number of deleted products</returns>
        public async Task<int> DeleteSoldProductsAsync()
        {
            int deletedCount = 0;
            
            // Get all sold products from database
            var soldProducts = await _dataAccess.GetSoldProductsAsync();
            
            // Delete each sold product
            foreach (var product in soldProducts)
            {
                await _dataAccess.DeleteProductAsync(product.Id);
                deletedCount++;
            }
            
            // Remove sold products from local list
            Products.RemoveAll(p => p.IsSold);
            
            return deletedCount;
        }
    }
}