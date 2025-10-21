namespace FarmSimulation.Data.Models
{
    public class AnimalLifecycle
    {
        public int AnimalId { get; set; }
        public int MaksimumYaş { get; set; } = 15;
        public DateTime SonYaşArtışZamanı { get; set; } = DateTime.Now;
        public bool Ölü { get; set; } = false;

        // Navigation property
        public AnimalBase? Animal { get; set; }
    }
}
