using Microsoft.EntityFrameworkCore;
using System;

namespace FarmSimulation.Data
{
    /// <summary>
    /// Represents a database service for the farm simulation
    /// Uses Entity Framework with SQL Server
    /// </summary>
    public class DatabaseService
    {
        private readonly DbContext _context;

        public DatabaseService(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Initializes the database
        /// </summary>
        public void InitializeDatabase()
        {
            _context.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets the database context
        /// </summary>
        public DbContext GetContext()
        {
            return _context;
        }
    }
}