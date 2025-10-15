using FarmSimulation.Data;
using FarmSimulation.Data.Models;
using static FarmSimulation.Data.Models.AnimalBase;

namespace FarmSimulation.Business.Services
{
    public class FarmService
    {
        private readonly FarmDbContext _context;

        public FarmService()
        {
            _context = new FarmDbContext();
        }

        // Hayvan ekleme
        public void AddAnimal(string type, int age, Sex sex)
        {
            AnimalBase? animal = type switch
            {
                "Chicken" => new Chicken { Name = "Tavuk", Age = age, Sex = sex },
                "Cow" => new Cow { Name = "İnek", Age = age, Sex = sex },
                "Sheep" => new Sheep { Name = "Koyun", Age = age, Sex = sex },
                "Goat" => new Goat { Name = "Keçi", Age = age, Sex = sex },
                _ => null
            };

            if (animal != null)
            {
                _context.Animals.Add(animal);
                _context.SaveChanges();
            }
        }

        // Hayvan listeleme
        public List<AnimalBase> GetAnimals()
        {
            return _context.Animals.ToList();
        }

        // Ürün listeleme
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        // Kasa bilgisi
        public decimal GetCash()
        {
            var cash = _context.Cash.FirstOrDefault();
            return cash?.Amount ?? 0;
        }

        // Ürün toplama
        public void CollectProducts()
        {
            var animals = _context.Animals.ToList();

            foreach (var animal in animals)
            {
                var product = animal.Produce();
                if (product != null)
                {
                    _context.Products.Add(product);
                }
            }

            _context.SaveChanges();
        }

        // Tüm ürünleri satma
        public void SellAllProducts()
        {
            var products = _context.Products.ToList();
            decimal total = products.Sum(p => p.Price * p.Quantity);

            // Kasa güncelle
            var cash = _context.Cash.FirstOrDefault();
            if (cash == null)
            {
                cash = new Cash { Amount = total };
                _context.Cash.Add(cash);
            }
            else
            {
                cash.Amount += total;
                _context.Cash.Update(cash);
            }

            // Ürünleri temizle
            _context.Products.RemoveRange(products);
            _context.SaveChanges();
        }

        // Hayvan silme
        public void DeleteAnimal(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                _context.SaveChanges();
            }
        }
    }
}