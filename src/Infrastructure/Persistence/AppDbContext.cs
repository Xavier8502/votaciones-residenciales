using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Conjunto> Conjuntos => Set<Conjunto>();
        public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
        public DbSet<Residente> Residentes => Set<Residente>();
        public DbSet<Votacion> Votaciones => Set<Votacion>();
        public DbSet<Pregunta> Preguntas => Set<Pregunta>();
        public DbSet<Opcion> Opciones => Set<Opcion>();
        public DbSet<Voto> Votos => Set<Voto>();
        public DbSet<DetalleVoto> DetallesVoto => Set<DetalleVoto>();
        public DbSet<AdminConjunto> AdminesConjunto => Set<AdminConjunto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
