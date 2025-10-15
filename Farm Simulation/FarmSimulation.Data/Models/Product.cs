using FarmSimulation.Data.Models;
using System;

namespace FarmSimulation.Data.Models
{
    public class Product
    {
        public Guid Id { get; private set; }              // Ürün Id
        public ProductType Type { get; set; }             // Ürün tipi (yumurta, süt, yün)
        public int Quantity { get; set; }                 // Ürün miktarı
        public decimal Value { get; set; }                // Ürünün parasal değeri
        public DateTime ProducedAt { get; private set; }  // Üretim zamanı
        public bool IsSold { get; set; }                  // Satıldı mı?

        public Product(ProductType type, int quantity, decimal value)
        {
            Id = Guid.NewGuid();
            Type = type;
            Quantity = quantity;
            Value = value;
            ProducedAt = DateTime.Now;
            IsSold = false;
        }
    }
}