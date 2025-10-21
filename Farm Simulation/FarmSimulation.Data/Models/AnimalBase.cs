﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmSimulation.Data.Models
{
    public abstract class AnimalBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Ad { get; set; } = "";
        
        public int Yaş { get; set; }
        
        public Sex Cinsiyet { get; set; }

        // Navigation property for lifecycle
        public AnimalLifecycle? Lifecycle { get; set; }

        protected AnimalBase()
        {
        }

        // Hayvan yaşıyor mu kontrolü
        public bool Yaşıyor()
        {
            if (Lifecycle == null)
                return true; // Lifecycle yoksa varsayılan olarak yaşıyor
            
            return !Lifecycle.Ölü && Yaş < Lifecycle.MaksimumYaş;
        }

        // Yaş artırma metodu
        public void YaşArtır()
        {
            if (Lifecycle == null || Lifecycle.Ölü)
                return;

            Yaş++;
            Lifecycle.SonYaşArtışZamanı = DateTime.Now;
            
            // Maksimum yaşa ulaştıysa öldür
            if (Yaş >= Lifecycle.MaksimumYaş)
            {
                Lifecycle.Ölü = true;
            }
        }

        // Ürün üretimi - sadece yaşayan hayvanlar üretebilir
        public Product? ÜrünÜret()
        {
            if (!Yaşıyor())
                return null;

            var product = Produce();
            return product;
        }

        // Her hayvan kendi ürününü üretecek
        protected abstract Product? Produce();
    }
}