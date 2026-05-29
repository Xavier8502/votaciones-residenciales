using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class ResidenteConfiguration : IEntityTypeConfiguration<Residente>
    {
        public void Configure(EntityTypeBuilder<Residente> builder)
        {
            builder.ToTable("Residentes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Cedula)
                .IsRequired()
                .HasMaxLength(12);

            builder.HasIndex(r => r.Cedula)
                .IsUnique();

            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.PinHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(r => r.Telefono)
                .HasMaxLength(20);

            builder.Property(r => r.Email)
                .HasMaxLength(150);

            builder.Property(r => r.EsPropietario)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(r => r.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Ignorar propiedad calculada
            builder.Ignore(r => r.NombreCompleto);
        }
    }
}
