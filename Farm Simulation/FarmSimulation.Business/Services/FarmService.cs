using FarmSimulation.Data;
using FarmSimulation.Data.Models;
using System.Collections.Generic;

namespace FarmSimulation.Business.Services
{
    public class FarmService
    {
        private readonly FarmDbContext _context;

        public FarmService()
        {
            _context = new FarmDbContext();
        }

        // Hayvan ekleme sadece DB'ye kayıt yapıyor
        public void AddAnimal(AnimalBase animal)
        {
            _context.Animals.Add(animal);
            _context.SaveChanges();
        }

        // Hayvanları DB'den listeleme
        public List<AnimalBase> GetAnimals()
        {
            return _context.Animals.ToList();
        }
    }
}