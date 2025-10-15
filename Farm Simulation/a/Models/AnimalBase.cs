namespace FarmSimulation.Data.Models
{
    public abstract class AnimalBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string AnimalType { get; protected set; } = "";

        // Her hayvan kendi ürününü üretecek
        public abstract Product? Produce();
    }
}