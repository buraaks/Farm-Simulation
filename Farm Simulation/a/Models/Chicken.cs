namespace FarmSimulation.Data.Models
{
    public class Chicken : AnimalBase
    {
        public Chicken() { } // EF Core ve UI için gerekli

        public Chicken(string name, int age, Sex sex) : base(name, age, sex) { }

        public override Product Produce()
        {
            return new Product
            {
                Name = "Yumurta",
                Quantity = 1,
                Price = 2
            };
        }
    }
}