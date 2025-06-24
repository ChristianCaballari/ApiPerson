using ApiPerson.Entidades;
using ApiPerson.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPerson
{
    public class AppDbContext: IdentityDbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        { 

        }

        // API FLUENTE
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persona>()
            .Property(e => e.Id).IsRequired();

            modelBuilder.Entity<Persona>()
            .Property(e => e.Nombre).IsRequired();

            modelBuilder.Entity<Persona>()
            .Property(e => e.Apellido).IsRequired();

            modelBuilder.Entity<Persona>()
            .Property(e => e.FechaNacimiento).IsRequired();

            modelBuilder.Entity<Persona>()
            .Property(e => e.Email).IsRequired();

            modelBuilder.Entity<Persona>()
            .Property(e => e.FechaRegistro)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Persona>()
            .Property(p => p.FechaNacimiento)
            .HasConversion(
              v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
             v => DateTime.SpecifyKind(v, DateTimeKind.Utc));


        }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Error> Errores { get; set; }
    }
}
