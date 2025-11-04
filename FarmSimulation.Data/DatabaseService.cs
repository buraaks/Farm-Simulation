using Microsoft.EntityFrameworkCore;
using System;

namespace FarmSimulation.Data
{
    /// <summary>
    /// Tarım simülasyonu için bir veritabanı servisi temsil eder
    /// </summary>
    public class DatabaseService
    {
        private readonly DbContext _context;

        public DatabaseService(DbContext context)
        {
            _context = context;
        }

        public void InitializeDatabase()
        {
            _context.Database.EnsureCreated();
        }

        public DbContext GetContext()
        {
            return _context;
        }
    }
}