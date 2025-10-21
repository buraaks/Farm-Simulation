namespace FarmSimulation.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";
        public int Miktar { get; set; }
        public decimal Tutar { get; set; } 
    }
}