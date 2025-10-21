﻿﻿﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Data.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Ad { get; set; } = "";
        
        public int Miktar { get; set; }
        
        public decimal Fiyat { get; set; }
        
        public decimal ToplamTutar { get; set; }
        
        public int? HayvanId { get; set; }
        
        // Navigation property
        [ForeignKey("HayvanId")]
        public AnimalBase? Hayvan { get; set; }
    }
}