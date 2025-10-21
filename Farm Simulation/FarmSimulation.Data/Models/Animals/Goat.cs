﻿﻿﻿﻿﻿namespace FarmSimulation.Data.Models
{
    public class Goat : AnimalBase
    {
        public Goat()
        {
        }

        protected override Product? Produce()
        {
            return new Product
            {
                Ad = "Keçi Sütü",
                Miktar = 1,
                Fiyat = 12m,
                ToplamTutar = 12m
            };
        }
    }
}
