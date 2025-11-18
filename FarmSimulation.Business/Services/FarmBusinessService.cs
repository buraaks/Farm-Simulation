
using FarmSimulation.Data;
using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSimulation.Business.Services
{
    // Çiftlik simülasyonunun iş mantığını yönetir
    public class FarmBusinessService
    {
        private readonly FarmDataAccess _dataAccess;
        private DateTime _lastSimulatedTime;
        private Dictionary<int, double> _animalTimeAccumulator = new Dictionary<int, double>();
        private bool _isSimulating = false;
        
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
            try
            {
                Animals = await _dataAccess.GetAllAnimalsAsync();
                Products = await _dataAccess.GetAllProductsAsync();
                Cash = await _dataAccess.GetCashAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading data from database: {ex.Message}", ex);
            }
        }

        public async Task<int> AddAnimalAsync(Animal animal)
        {
            try
            {
                int animalId = await _dataAccess.AddAnimalAsync(animal);
                Animals.Add(animal);
                return animalId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding animal: {ex.Message}", ex);
            }
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            try
            {
                _dataAccess.UpdateAnimal(animal);
                await _dataAccess.SaveChangesAsync();

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
            catch (Exception ex)
            {
                throw new Exception($"Error updating animal: {ex.Message}", ex);
            }
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            try
            {
                await _dataAccess.DeleteAnimalAsync(animalId);
                Animals.RemoveAll(a => a.Id == animalId);
                _animalTimeAccumulator.Remove(animalId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting animal: {ex.Message}", ex);
            }
        }

        public async Task<int> AddProductAsync(Product product)
        {
            try
            {
                int productId = await _dataAccess.AddProductAsync(product);
                Products.Add(product);
                return productId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding product: {ex.Message}", ex);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _dataAccess.UpdateProduct(product);
                await _dataAccess.SaveChangesAsync();

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
            catch (Exception ex)
            {
                throw new Exception($"Error updating product: {ex.Message}", ex);
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                await _dataAccess.DeleteProductAsync(productId);
                Products.RemoveAll(p => p.Id == productId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting product: {ex.Message}", ex);
            }
        }

        public Animal GetAnimalById(int animalId)
        {
            return Animals.FirstOrDefault(a => a.Id == animalId) ?? new Animal();
        }

        public Product GetProductById(int productId)
        {
            return Products.FirstOrDefault(p => p.Id == productId) ?? new Product();
        }

        public async Task SimulateTickAsync()
        {
            // Race condition (yarış durumu) koruması
            if (_isSimulating) return;
            _isSimulating = true;

            try
            {
                DateTime currentTime = DateTime.Now;
                var timeSinceLastSimulation = currentTime - _lastSimulatedTime;
                _lastSimulatedTime = currentTime;
                double elapsedSeconds = timeSinceLastSimulation.TotalSeconds;

                bool hasChanges = false;
                
                // Önce yeni ürünleri tutacak bir liste oluşturalım
                var newProducts = new List<Product>();

                // ToList() ile elde edilen kopya üzerinde çalışmak koleksiyonda değişiklik yapmaya izin verir
                foreach (var animal in Animals.ToList()) 
                {
                    if (animal.IsAlive)
                    {
                        // Ürün üretim ilerlemesi (hassas double tabanlı)
                        if (animal.CanProduce && animal.ProductProductionTime > 0)
                        {
                            double progressIncrease = (100.0 / animal.ProductProductionTime) * elapsedSeconds;
                            if (progressIncrease > 0)
                            {
                                animal.ProductProductionProgress += progressIncrease;
                                hasChanges = true;
                            }

                            // Üretim tamamlandığında ürünleri otomatik olarak topla
                            if (animal.ProductProductionProgress >= 100)
                            {
                                int productsMade = (int)(animal.ProductProductionProgress / 100);
                                for (int i = 0; i < productsMade; i++)
                                {
                                    var newProduct = CreateProductForAnimal(animal);
                                    newProducts.Add(newProduct);
                                }
                                animal.ProductProductionProgress %= 100; // Kalan ilerlemeyi sakla
                            }
                        }

                        // Yaşlanma sistemi
                        if (!_animalTimeAccumulator.ContainsKey(animal.Id))
                        {
                            _animalTimeAccumulator[animal.Id] = 0;
                        }
                        _animalTimeAccumulator[animal.Id] += elapsedSeconds;

                        if (_animalTimeAccumulator[animal.Id] >= GameSettings.SecondsPerGameDay)
                        {
                            int daysToAge = (int)(_animalTimeAccumulator[animal.Id] / GameSettings.SecondsPerGameDay);
                            animal.Age += daysToAge;
                            _animalTimeAccumulator[animal.Id] %= GameSettings.SecondsPerGameDay;
                            hasChanges = true;
                        }
                        
                        // Ölüm kontrolü
                        if (animal.Age >= animal.MaxAge)
                        {
                            animal.IsAlive = false;
                            hasChanges = true;
                        }
                    }
                }

                // Yeni üretilen ürünleri veritabanına ve mevcut listeye ekle
                if (newProducts.Any())
                {
                    foreach (var product in newProducts)
                    {
                        // AddProductAsync hem veritabanına hem de 'Products' listesine ekler
                        await AddProductAsync(product); 
                    }
                    // hasChanges zaten true olmalı ancak yine de emin olalım
                    hasChanges = true;
                }

                // Ölü hayvanları listeden ve veritabanından kaldır
                var deadAnimals = Animals.Where(a => !a.IsAlive).ToList();
                if (deadAnimals.Any())
                {
                    foreach (var deadAnimal in deadAnimals)
                    {
                        // Bu metot hem veritabanından hem listeden siler
                        await DeleteAnimalAsync(deadAnimal.Id); 
                    }
                    // DeleteAnimalAsync zaten listeden çıkardığı için burada tekrar çıkarmaya gerek yok
                    hasChanges = false; // Değişiklikler DeleteAnimalAsync içinde zaten kaydedildi.
                }

                // Başka değişiklikler varsa (yaşlanma, ilerleme) veritabanını güncelle
                if (hasChanges)
                {
                    await SaveChangesToDatabaseAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in simulation tick: {ex.Message}", ex);
            }
            finally
            {
                _isSimulating = false;
            }
        }

        private Product CreateProductForAnimal(Animal animal)
        {
            return new Product
            {
                ProductType = GetProductTypeByAnimal(animal.Type),
                Name = GetProductNameByAnimal(animal.Type),
                Quantity = 1,
                Price = GetProductPriceByAnimal(animal.Type),
                ProducedByAnimalType = animal.Type,
                DateProduced = DateTime.Now
            };
        }

        private string GetProductTypeByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                AnimalTypes.Chicken => ProductTypes.Egg,
                AnimalTypes.Cow => ProductTypes.Milk,
                AnimalTypes.Sheep => ProductTypes.Wool,
                _ => ProductTypes.Unknown
            };
        }

        private string GetProductNameByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                AnimalTypes.Chicken => ProductNames.Egg,
                AnimalTypes.Cow => ProductNames.Milk,
                AnimalTypes.Sheep => ProductNames.Wool,
                _ => ProductNames.Unknown
            };
        }

        private decimal GetProductPriceByAnimal(string animalType)
        {
            return animalType.ToLower() switch
            {
                AnimalTypes.Chicken => GameSettings.EggPrice,
                AnimalTypes.Cow => GameSettings.MilkPrice,
                AnimalTypes.Sheep => GameSettings.WoolPrice,
                _ => 0m
            };
        }

        public async Task<decimal> SellProductsAsync()
        {
            try
            {
                decimal totalEarnings = 0m;
                var unsoldProducts = Products.Where(p => !p.IsSold).ToList();

                foreach (var product in unsoldProducts)
                {
                    totalEarnings += product.Price * product.Quantity;
                    product.IsSold = true;
                    _dataAccess.UpdateProduct(product);
                }

                if (unsoldProducts.Any())
                {
                    Cash.Amount += totalEarnings;
                    _dataAccess.UpdateCash(Cash);
                    await _dataAccess.SaveChangesAsync();
                }

                return totalEarnings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error selling products: {ex.Message}", ex);
            }
        }

        public decimal GetAnimalPrice(string animalType)
        {
            return animalType.ToLower() switch
            {
                AnimalTypes.Chicken => GameSettings.ChickenPrice,
                AnimalTypes.Cow => GameSettings.CowPrice,
                AnimalTypes.Sheep => GameSettings.SheepPrice,
                _ => 0m
            };
        }

        public Animal CreateAnimal(string animalType, string name)
        {
            var animal = new Animal
            {
                Name = name,
                Type = animalType,
                Gender = new Random().Next(0, 2) == 0 ? Genders.Male : Genders.Female,
                DateOfBirth = DateTime.Now
            };

            switch (animalType.ToLower())
            {
                case AnimalTypes.Chicken:
                    animal.MaxAge = GameSettings.ChickenMaxAge;
                    animal.ProductProductionTime = GameSettings.ChickenProductionTime;
                    break;
                case AnimalTypes.Cow:
                    animal.MaxAge = GameSettings.CowMaxAge;
                    animal.ProductProductionTime = GameSettings.CowProductionTime;
                    break;
                case AnimalTypes.Sheep:
                    animal.MaxAge = GameSettings.SheepMaxAge;
                    animal.ProductProductionTime = GameSettings.SheepProductionTime;
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
            try
            {
                await _dataAccess.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving changes: {ex.Message}", ex);
            }
        }

        public async Task<Product> CollectProductFromAnimalAsync(Animal animal)
        {
            try
            {
                if (animal != null && animal.IsAlive && animal.CanProduce && animal.ProductProductionProgress >= 100)
                {
                    var product = CreateProductForAnimal(animal);
                    await AddProductAsync(product);

                    animal.ProductProductionProgress = 0;
                    await UpdateAnimalAsync(animal);

                    return product;
                }
                return new Product();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error collecting product: {ex.Message}", ex);
            }
        }

        public async Task<List<Product>> CollectAllProductsAsync()
        {
            try
            {
                var collectedProducts = new List<Product>();

                foreach (var animal in Animals.Where(a => a.IsAlive && a.CanProduce && a.ProductProductionProgress >= 100).ToList())
                {
                    var product = await CollectProductFromAnimalAsync(animal);
                    if (product != null && product.Name != ProductNames.Unknown)
                    {
                        collectedProducts.Add(product);
                    }
                }

                return collectedProducts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error collecting all products: {ex.Message}", ex);
            }
        }

        public async Task<int> DeleteSoldProductsAsync()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Error deleting sold products: {ex.Message}", ex);
            }
        }

        public void ClearTimeAccumulator()
        {
            _animalTimeAccumulator.Clear();
        }
    }
}

