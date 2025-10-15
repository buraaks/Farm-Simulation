namespace FarmSimulation.Data.Models
{
    public class Goat : AnimalBase
    {
        public Goat()
        {
            AnimalType = "Goat";
        }

        public override Product? Produce()
        {
            return new Product
            {
                Name = "Goat Milk",
                Quantity = 1,
                Price = 12m
            };
        }
    }
}