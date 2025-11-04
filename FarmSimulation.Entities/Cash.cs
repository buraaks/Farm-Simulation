using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Entities
{
    /// <summary>
    /// Represents cash in the farm simulation with properties matching the database structure
    /// </summary>
    public class Cash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Don't auto-generate ID for Cash
        public int Id { get; set; } = 1; // Cash table will have only one record with Id = 1
        
        public decimal Amount { get; set; }

        public Cash()
        {
            Amount = 1000m; // Start with 1000 cash
        }

        /// <summary>
        /// Adds money to the cash amount
        /// </summary>
        /// <param name="value">Amount to add</param>
        public void Add(decimal value)
        {
            if (value > 0) 
                Amount += value;
        }

        /// <summary>
        /// Subtracts money from the cash amount
        /// </summary>
        /// <param name="value">Amount to subtract</param>
        /// <returns>True if subtraction successful, false if insufficient funds</returns>
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