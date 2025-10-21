﻿﻿﻿﻿﻿﻿﻿﻿﻿using FarmSimulation.Data;
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
        public void AddAnimal(string selectedType, int age, Sex sex)
        {
            AnimalBase animal = selectedType switch
            {
                "Tavuk" => new Chicken { Ad = "Tavuk", Yaş = age, Cinsiyet = sex },
                "İnek" => new Cow { Ad = "İnek", Yaş = age, Cinsiyet = sex },
                "Koyun" => new Sheep { Ad = "Koyun", Yaş = age, Cinsiyet = sex },
                "Keçi" => new Goat { Ad = "Keçi", Yaş = age, Cinsiyet = sex },
                _ => throw new ArgumentException("Geçersiz tür")
            };

            _context.Animals.Add(animal);
            _context.SaveChanges();

            // Lifecycle kaydı oluştur
            int maksimumYaş = selectedType switch
            {
                "Tavuk" => 15,
                "İnek" => 25,
                "Koyun" => 20,
                "Keçi" => 18,
                _ => 15
            };

            var lifecycle = new AnimalLifecycle
            {
                AnimalId = animal.Id,
                MaksimumYaş = maksimumYaş,
                SonYaşArtışZamanı = DateTime.Now,
                Ölü = false
            };

            _context.AnimalLifecycles.Add(lifecycle);
            _context.SaveChanges();
        }

        // Hayvan listeleme
        public List<AnimalBase> GetAnimals()
        {
            var animals = _context.Animals.ToList();
            
            // Lifecycle bilgilerini yükle
            foreach (var animal in animals)
            {
                animal.Lifecycle = _context.AnimalLifecycles.FirstOrDefault(l => l.AnimalId == animal.Id);
            }
            
            return animals;
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
            return cash?.Tutar ?? 0;
        }

        // Ürün toplama
        public void CollectProducts()
        {
            var animals = _context.Animals.ToList();

            foreach (var animal in animals)
            {
                // Lifecycle bilgisini yükle
                animal.Lifecycle = _context.AnimalLifecycles.FirstOrDefault(l => l.AnimalId == animal.Id);
                
                // Yaşam süresi kontrolü ile ürün toplama
                var product = animal.ÜrünÜret();
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
            decimal total = products.Sum(p => p.Tutar * p.Miktar);

            // Kasa güncelle
            var cash = _context.Cash.FirstOrDefault();
            if (cash == null)
            {
                cash = new Cash { Tutar = (int)total };
                _context.Cash.Add(cash);
            }
            else
            {
                cash.Tutar += total;
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
        public void ResetCash()
        {
            var cash = _context.Cash.FirstOrDefault();
            if (cash == null)
            {
                // Eğer kayıt yoksa sıfır tutarlı bir kayıt ekle
                cash = new Cash { Tutar = 0m };
                _context.Cash.Add(cash);
            }
            else
            {
                cash.Tutar = 0m;
                _context.Cash.Update(cash);
            }
            _context.SaveChanges();
        }

        // Ölü hayvanları temizle
        public int RemoveDeadAnimals()
        {
            var deadAnimals = _context.Animals.ToList()
                .Where(a => !a.Yaşıyor())
                .ToList();

            if (deadAnimals.Any())
            {
                _context.Animals.RemoveRange(deadAnimals);
                _context.SaveChanges();
            }

            return deadAnimals.Count;
        }

        // Yaşayan hayvan sayısı
        public int GetAliveAnimalCount()
        {
            return _context.Animals.ToList()
                .Count(a => a.Yaşıyor());
        }

        // Ölü hayvan sayısı
        public int GetDeadAnimalCount()
        {
            return _context.Animals.ToList()
                .Count(a => !a.Yaşıyor());
        }

        // Tüm hayvanların yaşını artır
        public void IncreaseAllAnimalsAge()
        {
            var animals = _context.Animals.ToList();
            
            foreach (var animal in animals)
            {
                // Lifecycle bilgisini yükle
                animal.Lifecycle = _context.AnimalLifecycles.FirstOrDefault(l => l.AnimalId == animal.Id);
                
                // Yaşı artır
                animal.YaşArtır();
            }
            
            _context.SaveChanges();
        }
    }
}