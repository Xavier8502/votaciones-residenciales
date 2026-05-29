using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class VotacionConfiguration : IEntityTypeConfiguration<Votacion>
    {
        public void Configure(EntityTypeBuilder<Votacion> builder)
        {
            builder.ToTable("Votaciones");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.Descripcion)
                .HasMaxLength(1000);

            builder.Property(v => v.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(v => v.TipoPeso)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(v => v.QuorumRequerido)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(v => v.FechaInicio)
                .IsRequired();

            builder.Property(v => v.FechaFin)
                .IsRequired();

            builder.Property(v => v.MostrarResultadosParciales)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(v => v.ActaUrl)
                .HasMaxLength(500);

            builder.HasMany(v => v.Preguntas)
                .WithOne()
                .HasForeignKey(p => p.VotacionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.Votos)
                .WithOne()
                .HasForeignKey(vo => vo.VotacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
