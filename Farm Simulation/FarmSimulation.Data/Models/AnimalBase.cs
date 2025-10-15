using FarmSimulation.Data.Models;
using System;

namespace FarmSimulation.Data.Models
{
    public abstract class AnimalBase
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public AnimalType Type { get; protected set; }

        public TimeSpan LifeSpan { get; protected set; }
        public TimeSpan ProduceInterval { get; protected set; }
        public DateTime BirthDate { get; private set; }
        public DateTime LastProduceTime { get; protected set; }

        protected AnimalBase(string name, int age, Sex sex)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
            Sex = sex;
            BirthDate = DateTime.Now;
            LastProduceTime = DateTime.Now;
        }

        public bool IsAlive() =>
            DateTime.Now - BirthDate < LifeSpan;

        public bool CanProduce() =>
            IsAlive() && (DateTime.Now - LastProduceTime >= ProduceInterval);

        public abstract Product Produce();

        public void IncreaseAge(int days) =>
            Age += days;
    }
}