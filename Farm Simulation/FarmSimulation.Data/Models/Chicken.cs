using System;
using FarmSimulation.Data.Models;

namespace FarmSimulation.Data.Models
{
    public class Chicken : AnimalBase
    {
        public Chicken(string name, int age, Sex sex)
            : base(name, age, sex)
        {
            Type = AnimalType.Chicken;
            LifeSpan = TimeSpan.FromDays(1000);          // Ortalama 1000 gün ömür
            ProduceInterval = TimeSpan.FromSeconds(10);  // 10 saniyede bir yumurta
        }

        public override Product Produce()
        {
            if (!CanProduce())
                return null;

            LastProduceTime = DateTime.Now;

            // Tavuk her üretimde 1 yumurta verir, örnek değer 2 TL
            return new Product(ProductType.Egg, 1, 2.0m);
        }
    }
}