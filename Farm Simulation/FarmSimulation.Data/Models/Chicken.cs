﻿﻿﻿﻿namespace FarmSimulation.Data.Models
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
                Tutar = 2.5m
            };
        }
    }
}