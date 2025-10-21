﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using FarmSimulation.Data;
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

        // Ürünleri isme göre grupla
        public Dictionary<string, List<Product>> GetProductsByType()
        {
            return _context.Products
                .GroupBy(p => p.Ad)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Ürün istatistikleri
        public Dictionary<string, int> GetProductStatistics()
        {
            return _context.Products
                .GroupBy(p => p.Ad)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Miktar));
        }

        // Toplam ürün değeri
        public decimal GetTotalProductValue()
        {
            return _context.Products.Sum(p => p.ToplamTutar);
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
            // Önce mevcut ürünleri birleştir (veritabanında aynı türdeki farklı satırları tek satıra indir)
            var allProducts = _context.Products.ToList();
            var groupedProducts = allProducts.GroupBy(p => p.Ad).ToList();
            
            foreach (var group in groupedProducts)
            {
                if (group.Count() > 1)
                {
                    // Birden fazla satır varsa, hepsini birleştir
                    var firstProduct = group.First();
                    var otherProducts = group.Skip(1).ToList();
                    
                    foreach (var otherProduct in otherProducts)
                    {
                        firstProduct.Miktar += otherProduct.Miktar;
                        firstProduct.ToplamTutar += otherProduct.ToplamTutar;
                        _context.Products.Remove(otherProduct);
                    }
                    
                    firstProduct.HayvanId = null; // HayvanId'yi temizle
                    _context.Products.Update(firstProduct);
                }
                else
                {
                    // Tek satır varsa sadece HayvanId'yi temizle
                    var singleProduct = group.First();
                    singleProduct.HayvanId = null;
                    _context.Products.Update(singleProduct);
                }
            }
            
            _context.SaveChanges();
            
            // Şimdi yeni ürünleri topla
            var animals = _context.Animals.ToList();

            foreach (var animal in animals)
            {
                // Lifecycle bilgisini yükle
                animal.Lifecycle = _context.AnimalLifecycles.FirstOrDefault(l => l.AnimalId == animal.Id);
                
                // Yaşam süresi kontrolü ile ürün toplama
                var product = animal.ÜrünÜret();
                if (product != null)
                {
                    // Aynı türdeki ürünü bul (HayvanId'ye bakmadan, sadece Ad'a göre)
                    var existingProduct = _context.Products
                        .FirstOrDefault(p => p.Ad == product.Ad);
                    
                    if (existingProduct != null)
                    {
                        existingProduct.Miktar += product.Miktar;
                        existingProduct.ToplamTutar += product.ToplamTutar;
                        _context.Products.Update(existingProduct);
                    }
                    else
                    {
                        // Yeni ürün ekle - HayvanId null olsun
                        product.HayvanId = null;
                        _context.Products.Add(product);
                    }
                }
            }

            _context.SaveChanges();
        }

        // Tüm ürünleri satma
        public void SellAllProducts()
        {
            var products = _context.Products.ToList();
            decimal total = products.Sum(p => p.ToplamTutar);

            // Kasa güncelle
            var cash = _context.Cash.FirstOrDefault();
            if (cash == null)
            {
                cash = new Cash { Tutar = total };
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

        // Seçili ürünleri sat
        public void SellSelectedProducts(List<int> productIds)
        {
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToList();
            decimal total = products.Sum(p => p.ToplamTutar);

            // Kasa güncelle
            var cash = _context.Cash.FirstOrDefault();
            if (cash == null)
            {
                cash = new Cash { Tutar = total };
                _context.Cash.Add(cash);
            }
            else
            {
                cash.Tutar += total;
                _context.Cash.Update(cash);
            }

            // Seçili ürünleri sil
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

        // Seçili hayvanları sil
        public void DeleteSelectedAnimals(List<int> animalIds)
        {
            var animals = _context.Animals.Where(a => animalIds.Contains(a.Id)).ToList();
            
            if (animals.Any())
            {
                _context.Animals.RemoveRange(animals);
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