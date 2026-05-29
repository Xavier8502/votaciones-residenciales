using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class VotoConfiguration : IEntityTypeConfiguration<Voto>
    {
        public void Configure(EntityTypeBuilder<Voto> builder)
        {
            builder.ToTable("Votos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.PesoAplicado)
                .IsRequired()
                .HasPrecision(10, 4);

            builder.Property(v => v.FechaHora)
                .IsRequired();

            // Restricción: un residente solo puede votar una vez por votación
            builder.HasIndex(v => new { v.VotacionId, v.ResidenteId })
                .IsUnique();

            builder.HasMany(v => v.Detalles)
                .WithOne()
                .HasForeignKey(d => d.VotoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
