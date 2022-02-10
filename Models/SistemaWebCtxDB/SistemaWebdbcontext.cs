using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.Models.SistemaWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SistemaWebCtxDB
{
    public class SistemaWebdbcontext : DbContext
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
            //CONFIGURACION PARA UTILIZAR EL INCLUDE CUANDO MANDA EL ERROR COLUMNAME INVALID
            modelBuilder.Entity<Usuarios>()
            .HasOne(p => p.Rol)
            .WithMany(b => b.Usuarios)
            .HasForeignKey(p => p.IdRol)
            .HasPrincipalKey(b => b.IdRol);
            //modelBuilder.Entity<Usuarios>(entity =>
            //{
            //    entity.HasKey(x => new { x.Id });
            //});
            //modelBuilder.Entity<Rol>(entity =>
            //{
            //    entity.HasKey(x => new { x.IdRol});
            //});
            //modelBuilder.Entity<Usuarios>(b =>
            //{
            //    b.HasMany(e => e.Nombre)
            //    .WithOne()
            //})
            //Personalización para el servicio
            //modelBuilder.Entity<Usuarios>().HasKey(y => new { y.IdRol });
            //modelBuilder.Entity<Rol>().HasKey(y => new {  y.IdRol });
            //modelBuilder.Entity<Entidades.Mediaccess.Ubicacion.Ubicacion>().HasKey(y => new { y.CodCuenta, y.CodUbicacion });
            //modelBuilder.HasDefaultSchema("database_schema");
            //modelBuilder.Entity<EntityNameInModel>().ToTable("table_in_database").HasKey(ats => ats.id);
            //modelBuilder.Entity<EntityNameInModel>().Property(ats => ats.user_name).HasColumnName("user_name");


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
