using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Entities
{
    // Nakit varlığı
    public class Cash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = 1;
        
        public decimal Amount { get; set; }

        public Cash()
        {
            Amount = GameSettings.InitialCash;
        }

        // Nakit ekle
        public void Add(decimal value)
        {
            if (value > 0) 
                Amount += value;
        }

        // Nakit düş
        public bool Subtract(decimal value)
        {
            if (Amount >= value)
            {
                Amount -= value;
                return true;
            }
            return false;
        }
    }
}
