namespace FarmSimulation.Data.Models
{
    public class Cow : AnimalBase
    {
        public Cow()
        {

        }

        public override Product? Produce()
        {
            return new Product
            {
                Ad = "Süt",
                Miktar = 1,
                Tutar = 10m
            };
        }
    }
}