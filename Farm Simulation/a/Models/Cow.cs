namespace FarmSimulation.Data.Models
{
    public class Cow : AnimalBase
    {
        public Cow()
        {
            AnimalType = "Cow";
        }

        public override Product? Produce()
        {
            return new Product
            {
                Name = "Milk",
                Quantity = 1,
                Price = 10m
            };
        }
    }
}