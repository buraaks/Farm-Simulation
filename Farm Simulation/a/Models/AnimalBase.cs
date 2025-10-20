namespace FarmSimulation.Data.Models
{
    public abstract class AnimalBase
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";
        public int Yaş { get; set; }
        public Sex Cinsiyet { get; set; }
        public string Tür { get; protected set; } = "";

        // Her hayvan kendi ürününü üretecek
        public abstract Product? Produce();
    }
}