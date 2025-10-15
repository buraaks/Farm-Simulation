namespace FarmSimulation.Data.Models
{
    public class Chicken : AnimalBase
    {
        public Chicken()
        {
            AnimalType = "Chicken";
        }

        public override Product? Produce()
        {
            return new Product
            {
                Name = "Egg",
                Quantity = 1,
                Price = 2.5m
            };
        }
    }
}