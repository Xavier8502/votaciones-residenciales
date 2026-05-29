using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class InmuebleConfiguration : IEntityTypeConfiguration<Inmueble>
    {
        public void Configure(EntityTypeBuilder<Inmueble> builder)
        {
            builder.ToTable("Inmuebles");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Numero)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(i => i.Tipo)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(i => i.Coeficiente)
                .IsRequired()
                .HasPrecision(10, 4);

            builder.Property(i => i.Torre)
                .HasMaxLength(20);

            builder.Property(i => i.Piso)
                .HasMaxLength(10);

            builder.Property(i => i.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Numero único por conjunto
            builder.HasIndex(i => new { i.ConjuntoId, i.Torre, i.Numero })
                .IsUnique();

            builder.HasMany(i => i.Residentes)
                .WithOne(r => r.Inmueble)
                .HasForeignKey(r => r.InmuebleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
