namespace FarmSimulation.Data.Models
{
    public class Goat : AnimalBase
    {
        public Goat() { }

        public Goat(string name, int age, Sex sex) : base(name, age, sex) { }

        public override Product Produce()
        {
            return new Product
            {
                Name = "Keçi Sütü",
                Quantity = 1,
                Price = 4
            };
        }
    }
}