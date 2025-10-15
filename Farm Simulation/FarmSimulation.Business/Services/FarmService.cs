using System;
using System.Collections.Generic;
using System.Linq;
using FarmSimulation.Data.Models;

namespace FarmSim.Business.Services
{
    public class FarmService
    {
        // Hayvan listesi
        private readonly List<AnimalBase> _animals = new List<AnimalBase>();

        // Ürün listesi
        private readonly List<Product> _products = new List<Product>();

        // Kasa (para)
        public decimal CashBalance { get; private set; }

        // Hayvan ekleme
        public void AddAnimal(AnimalBase animal)
        {
            if (animal == null) return;
            _animals.Add(animal);
        }

        // Hayvanları listeleme
        public IReadOnlyList<AnimalBase> GetAnimals() => _animals.AsReadOnly();

        // Ürünleri listeleme
        public IReadOnlyList<Product> GetProducts() => _products.AsReadOnly();

        // Hayvanlardan ürün toplama
        public void CollectProducts()
        {
            foreach (var animal in _animals.ToList())
            {
                if (!animal.IsAlive())
                {
                    _animals.Remove(animal); // yaşam süresi dolmuşsa kaldır
                    continue;
                }

                var product = animal.Produce();
                if (product != null)
                {
                    _products.Add(product);
                }
            }
        }

        // Ürün satışı
        public void SellProduct(Product product)
        {
            if (product == null || product.IsSold) return;

            product.IsSold = true;
            CashBalance += product.Value;
        }

        // Tüm satılmamış ürünleri sat
        public void SellAllProducts()
        {
            foreach (var product in _products.Where(p => !p.IsSold))
            {
                SellProduct(product);
            }
        }

        // Yeni hayvan satın alma (kasa kontrolü ile)
        public bool BuyAnimal(AnimalBase animal, decimal price)
        {
            if (CashBalance >= price)
            {
                CashBalance -= price;
                AddAnimal(animal);
                return true;
            }
            return false;
        }
    }
}