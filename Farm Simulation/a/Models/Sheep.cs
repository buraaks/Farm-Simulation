namespace FarmSimulation.Data.Models
{
    public class Sheep : AnimalBase
    {
        public Sheep()
        {
            AnimalType = "Sheep";
        }

        public override Product? Produce()
        {
            return new Product
            {
                Name = "Wool",
                Quantity = 1,
                Price = 15m
            };
        }
    }
}