using System;
using FarmSimulation.Data.Models;

namespace FarmSimulation.Data.Models
{
    public class Cow : AnimalBase
    {
        public Cow(string name, int age, Sex sex)
            : base(name, age, sex)
        {
            Type = AnimalType.Cow;
            LifeSpan = TimeSpan.FromDays(5000);           // Ortalama 5000 gün ömür
            ProduceInterval = TimeSpan.FromSeconds(30);   // 30 saniyede bir süt üretimi
        }

        public override Product Produce()
        {
            if (!CanProduce())
                return null;

            LastProduceTime = DateTime.Now;

            // İnek her üretimde 1 birim süt verir, örnek değer 10 TL
            return new Product(ProductType.Milk, 1, 10.0m);
        }
    }
}