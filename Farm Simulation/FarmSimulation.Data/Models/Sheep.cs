using System;
using FarmSimulation.Data.Models;

namespace FarmSimulation.Data.Models
{
    public class Sheep : AnimalBase
    {
        public Sheep(string name, int age, Sex sex)
            : base(name, age, sex)
        {
            Type = AnimalType.Sheep;
            LifeSpan = TimeSpan.FromDays(3000);           // Ortalama 3000 gün ömür
            ProduceInterval = TimeSpan.FromSeconds(20);   // 20 saniyede bir yün üretimi
        }

        public override Product Produce()
        {
            if (!CanProduce())
                return null;

            LastProduceTime = DateTime.Now;

            // Koyun her üretimde 1 birim yün verir, örnek değer 5 TL
            return new Product(ProductType.Wool, 1, 5.0m);
        }
    }
}
