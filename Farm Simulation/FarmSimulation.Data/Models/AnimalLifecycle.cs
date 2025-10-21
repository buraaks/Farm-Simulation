using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Data.Models
{
    [Table("AnimalLifecycle")]
    public class AnimalLifecycle
    {
        [Key]
        [ForeignKey("Animal")]
        public int AnimalId { get; set; }
        
        public int MaksimumYaş { get; set; } = 15;
        
        public DateTime SonYaşArtışZamanı { get; set; } = DateTime.Now;
        
        public bool Ölü { get; set; } = false;

        // Navigation property
        public AnimalBase? Animal { get; set; }
    }
}
