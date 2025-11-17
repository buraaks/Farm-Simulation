using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Entities
{
    // Tarım simülasyonundaki nakit parayı temsil eder
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

        // Nakit miktarına para ekler
        public void Add(decimal value)
        {
            if (value > 0) 
                Amount += value;
        }

        // Nakit miktarından para çıkarır
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
