using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Entities
{
    /// <summary>
    /// Tarım simülasyonundaki nakit parayı temsil eder
    /// </summary>
    public class Cash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = 1;
        
        public decimal Amount { get; set; }

        public Cash()
        {
            Amount = 1000m;
        }

        /// <summary>
        /// Nakit miktarına para ekler
        /// </summary>
        public void Add(decimal value)
        {
            if (value > 0) 
                Amount += value;
        }

        /// <summary>
        /// Nakit miktarından para çıkarır
        /// </summary>
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