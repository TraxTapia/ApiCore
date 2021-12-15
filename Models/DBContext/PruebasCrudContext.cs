using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DBContext
{
    public class PruebasCrudContext : DbContext
    {
        private String _Constring = String.Empty;
        public PruebasCrudContext(DbContextOptions<PruebasCrudContext> options, string pConstring)
            :base(options)
        {
            _Constring = pConstring;
        }

       public PruebasCrudContext(string pConstring)
        {
            _Constring = pConstring;
        }

        public PruebasCrudContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_Constring)
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(new LoggerFactory());
        }
        public DbSet<Productos> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productos>()
                .Property(e => e.Producto)
                .IsUnicode(false);
        }
    }
}
