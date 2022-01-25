using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.Models.SistemaWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SistemaWebCtxDB
{
    public class SistemaWebdbcontext:DbContext
    {
        private String _Constring = String.Empty;
        public SistemaWebdbcontext(DbContextOptions<SistemaWebdbcontext> options, String pConstring)
            : base(options)
        {
            _Constring = pConstring;
        }

        public SistemaWebdbcontext(String pConstring)
        {
            _Constring = pConstring;
        }

        public SistemaWebdbcontext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_Constring);
            //var lf = new LoggerFactory();
            //lf.AddProvider(new LoggerProvider());
            //optionsBuilder.UseLoggerFactory(lf);
            //Personalización para el servicio
        }
        //PARA TGRABJAR CON LAS TABLAS SQL
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Personalización para el servicio
            //modelBuilder.Entity<Especialidad>().HasKey(y => new { y.CodEspecialidad, y.CodTipoCuenta });
            //modelBuilder.Entity<Entidades.Mediaccess.Ubicacion.Ubicacion>().HasKey(y => new { y.CodCuenta, y.CodUbicacion });
          

        }
        public override int SaveChanges()
        {
            var entidades = ChangeTracker.Entries();
            if (entidades != null)
            {
                foreach (var entidad in entidades.Where(c => c.State != EntityState.Unchanged))
                {
                    //if (entidad.Entity is Entidades.DocumentacionEnvio)
                    //{
                    //    Auditar(entidad);
                    //}
                }
            }
            return base.SaveChanges();
        }

        private void Auditar(EntityEntry entidad)
        {

        }
    }
}
