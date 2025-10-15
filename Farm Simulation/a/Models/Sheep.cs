using FarmSimulation.Data.Models;

namespace FarmSimulation.Data.Models
{
    public class Sheep : AnimalBase
    {
        public Sheep() { }

        public Sheep(string name, int age, Sex sex) : base(name, age, sex) { }

        public override Product Produce()
        {
            return new Product
            {
                Name = "Yün",
                Quantity = 1,
                Price = 3
            };
        }
    }
}