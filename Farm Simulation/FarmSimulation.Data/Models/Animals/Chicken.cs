﻿﻿﻿﻿﻿﻿﻿﻿﻿namespace FarmSimulation.Data.Models
{
    public class Chicken : AnimalBase
    {
        public Chicken()
        {
        }

        protected override Product? Produce()
        {
            return new Product
            {
                Ad = "Yumurta",
                Miktar = 1,
                Fiyat = 2.5m,
                ToplamTutar = 2.5m
            };
        }
    }
}