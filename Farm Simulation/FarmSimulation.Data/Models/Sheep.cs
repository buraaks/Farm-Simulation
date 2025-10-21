﻿﻿﻿namespace FarmSimulation.Data.Models
{
    public class Sheep : AnimalBase
    {
        public Sheep()
        {
        }

        protected override Product? Produce()
        {
            return new Product
            {
                Ad = "Yün",
                Miktar = 1,
                Tutar = 15m
            };
        }
    }
}