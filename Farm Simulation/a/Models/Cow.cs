namespace FarmSimulation.Data.Models
{
    public class Cow : AnimalBase
    {
        public Cow() { }

        public Cow(string name, int age, Sex sex) : base(name, age, sex) { }

        public override Product Produce()
        {
            return new Product
            {
                Name = "Süt",
                Quantity = 1,
                Price = 5
            };
        }
    }
}