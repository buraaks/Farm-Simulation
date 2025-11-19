using System;
using System.ComponentModel.DataAnnotations;

namespace FarmSimulation.Entities
{
    // Ürün varlığı
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ProductType { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
        
        public DateTime DateProduced { get; set; }
        
        public bool IsSold { get; set; }
        
        [MaxLength(50)]
        public string ProducedByAnimalType { get; set; }

        public Product()
        {
            ProductType = string.Empty;
            Name = string.Empty;
            ProducedByAnimalType = string.Empty;
            DateProduced = DateTime.Now;
            IsSold = false;
        }
    }
}