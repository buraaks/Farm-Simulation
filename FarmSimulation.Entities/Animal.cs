using System;
using System.ComponentModel.DataAnnotations;

namespace FarmSimulation.Entities
{
    /// <summary>
    /// Represents an animal in the farm simulation with properties matching the database structure
    /// </summary>
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        [MaxLength(10)]
        public string Gender { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public int MaxAge { get; set; }
        
        public int ProductProductionTime { get; set; } // in seconds
        
        public int ProductProductionProgress { get; set; } // percentage
        
        public bool IsAlive { get; set; }
        
        public bool CanProduce { get; set; }
        
        public bool IsReadyToSell { get; set; }

        public Animal()
        {
            Name = string.Empty;
            Gender = string.Empty;
            Type = string.Empty;
            IsAlive = true;
            CanProduce = true;
            DateOfBirth = DateTime.Now;
        }
    }
}