using FarmSimulation.Data.Models;
using System;

namespace FarmSim.Data.Models
{
    public class Goat : AnimalBase
    {
        public Goat(string name, int age, Sex sex)
            : base(name, age, sex)
        {
            Type = AnimalType.Goat;
            LifeSpan = TimeSpan.FromDays(4000);           // Ortalama 4000 gün ömür
            ProduceInterval = TimeSpan.FromSeconds(25);   // 25 saniyede bir süt üretimi
        }

        public override Product Produce()
        {
            if (!CanProduce())
                return null;

            LastProduceTime = DateTime.Now;

            // Keçi her üretimde 1 birim süt verir, örnek değer 8 TL
            return new Product(ProductType.Milk, 1, 8.0m);
        }
    }
}