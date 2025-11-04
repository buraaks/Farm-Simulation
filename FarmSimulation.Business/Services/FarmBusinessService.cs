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
    /// Tarım simülasyonu için iş mantığını yönetir
    /// </summary>
    public class FarmBusinessService
    {
        private readonly FarmDataAccess _dataAccess;
        private DateTime _lastSimulatedTime;
        public FarmDataAccess dataAccess => _dataAccess;
        public List<Animal> Animals { get; private set; }
        public List<Product> Products { get; private set; }
        public Cash Cash { get; private set; }

        public FarmBusinessService(FarmDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            Animals = new List<Animal>();
            Products = new List<Product>();
            Cash = new Cash();
            _lastSimulatedTime = DateTime.Now;
        }

        public async Task InitializeAsync()
        {
            await LoadDataFromDatabaseAsync();
        }

        private async Task LoadDataFromDatabaseAsync()
        {
            Animals = await _dataAccess.GetAllAnimalsAsync();
            Products = await _dataAccess.GetAllProductsAsync();
            Cash = await _dataAccess.GetCashAsync();
        }

        public async Task<int> AddAnimalAsync(Animal animal)
        {
            int animalId = await _dataAccess.AddAnimalAsync(animal);
            Animals.Add(animal);
            return animalId;
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            await _dataAccess.UpdateAnimalAsync(animal);
            
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

        public async Task DeleteAnimalAsync(int animalId)
        {
            await _dataAccess.DeleteAnimalAsync(animalId);
            
            Animals.RemoveAll(a => a.Id == animalId);
        }

        public async Task<int> AddProductAsync(Product product)
        {
            int productId = await _dataAccess.AddProductAsync(product);
            Products.Add(product);
            return productId;
        }

        /// <summary>
        /// Tarımdaki ve veritabanındaki mevcut bir ürünü günceller
        /// </summary>
        /// <param name="product">Güncellenecek ürün</param>
        public async Task UpdateProductAsync(Product product)
        {
            await _dataAccess.UpdateProductAsync(product);
            
            // Yerel listedeki ürünü de güncelle
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
        /// Tarımdan ve veritabanından bir ürünü siler
        /// </summary>
        /// <param name="productId">Silinecek ürünün ID'si</param>
        public async Task DeleteProductAsync(int productId)
        {
            await _dataAccess.DeleteProductAsync(productId);
            
            // Ürünü yerel listeden de kaldır
            Products.RemoveAll(p => p.Id == productId);
        }

        /// <summary>
        /// Tarımdan ID'ye göre bir hayvan alır
        /// </summary>
        /// <param name="animalId">Alınacak hayvanın ID'si</param>
        /// <returns>Hayvan nesnesi</returns>
        public Animal GetAnimalById(int animalId)
        {
            return Animals.FirstOrDefault(a => a.Id == animalId) ?? new Animal();
        }

        /// <summary>
        /// Tarımdan ID'ye göre bir ürün alır
        /// </summary>
        /// <param name="productId">Alınacak ürünün ID'si</param>
        /// <returns>Ürün nesnesi</returns>
        public Product GetProductById(int productId)
        {
            return Products.FirstOrDefault(p => p.Id == productId) ?? new Product();
        }

        public async Task SimulateTickAsync()
        {
            DateTime currentTime = DateTime.Now;
            var timeSinceLastSimulation = currentTime - _lastSimulatedTime;
            _lastSimulatedTime = currentTime;
            
            const int SECONDS_PER_GAME_DAY = 300;

            foreach (var animal in Animals.ToList())
            {
                if (animal.IsAlive)
                {
                    if (animal.CanProduce && animal.ProductProductionTime > 0)
                    {
                        animal.ProductProductionProgress += 100 / animal.ProductProductionTime;

                        if (animal.ProductProductionProgress >= 100)
                        {
                            var product = CreateProductForAnimal(animal);
                            await AddProductAsync(product);

                            animal.ProductProductionProgress = 0;
                            
                            await UpdateAnimalAsync(animal);
                        }
                    }
                }
                
                if (animal.IsAlive) 
                {
                    int daysToAge = (int)(timeSinceLastSimulation.TotalSeconds / SECONDS_PER_GAME_DAY);
                    
                    if (daysToAge > 0)
                    {
                        animal.Age += daysToAge;
                    }
                }
                
                if (animal.IsAlive && animal.Age >= animal.MaxAge)
                {
                    animal.IsAlive = false;
                }
            }
            
            Animals.RemoveAll(a => !a.IsAlive);
            
            await SaveChangesToDatabaseAsync();
        }

        private Product CreateProductForAnimal(Animal animal)
        {
            var product = new Product
            {
                ProductType = GetProductTypeByAnimal(animal.Type),
                Name = GetProductNameByAnimal(animal.Type),
                Quantity = 1,
                Price = GetProductPriceByAnimal(animal.Type),
                ProducedByAnimalType = animal.Type,
                DateProduced = DateTime.Now
            };

            return product;
        }

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

        private decimal GetProductPriceByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => 0.5m,
                "cow" => 2.5m,
                "sheep" => 3.0m,
                _ => 0m
            };
        }

        public async Task<decimal> SellProductsAsync()
        {
            decimal totalEarnings = 0m;

            var unsoldProducts = Products.Where(p => !p.IsSold).ToList();

            foreach (var product in unsoldProducts)
            {
                totalEarnings += product.Price * product.Quantity;
                product.IsSold = true;
                
                await UpdateProductAsync(product);
            }

            Cash.Amount += totalEarnings;
            await _dataAccess.UpdateCashAsync(Cash);

            return totalEarnings;
        }

        public decimal GetAnimalPrice(string animalType)
        {
            return animalType.ToLower() switch
            {
                "chicken" => 10m,
                "cow" => 500m,
                "sheep" => 150m,
                _ => 0m
            };
        }

        public Animal CreateAnimal(string animalType, string name)
        {
            var animal = new Animal
            {
                Name = name,
                Type = animalType,
                Gender = new Random().Next(0, 2) == 0 ? "Male" : "Female",
                DateOfBirth = DateTime.Now
            };

            switch (animalType.ToLower())
            {
                case "chicken":
                    animal.MaxAge = 15;
                    animal.ProductProductionTime = 30;
                    break;
                case "cow":
                    animal.MaxAge = 20;
                    animal.ProductProductionTime = 60;
                    break;
                case "sheep":
                    animal.MaxAge = 25;
                    animal.ProductProductionTime = 90;
                    break;
                default:
                    animal.MaxAge = 10;
                    animal.ProductProductionTime = 60;
                    break;
            }

            return animal;
        }

        private async Task SaveChangesToDatabaseAsync()
        {
            foreach (var animal in Animals)
            {
                await _dataAccess.UpdateAnimalAsync(animal);
            }

            await _dataAccess.UpdateCashAsync(Cash);
        }

        public async Task<Product> CollectProductFromAnimalAsync(Animal animal)
        {
            if (animal != null && animal.IsAlive && animal.CanProduce)
            {
                var product = CreateProductForAnimal(animal);
                await AddProductAsync(product);
                
                animal.ProductProductionProgress = 0;
                
                await UpdateAnimalAsync(animal);
                
                return product;
            }
            return new Product();
        }

        public async Task<List<Product>> CollectAllProductsAsync()
        {
            var collectedProducts = new List<Product>();
            
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

        public async Task<int> DeleteSoldProductsAsync()
        {
            int deletedCount = 0;
            
            var soldProducts = await _dataAccess.GetSoldProductsAsync();
            
            foreach (var product in soldProducts)
            {
                await _dataAccess.DeleteProductAsync(product.Id);
                deletedCount++;
            }
            
            Products.RemoveAll(p => p.IsSold);
            
            return deletedCount;
        }
    }
}