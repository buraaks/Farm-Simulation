namespace FarmSimulation.Data.Models
{
    public class Goat : AnimalBase
    {
        public Goat()
        {

        }

        public override Product? Produce()
        {
            return new Product
            {
                Ad = "Keçi Sütü",
                Miktar = 1,
                Tutar = 12m
            };
        }
    }
}