using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class AdminConjuntoConfiguration
    : IEntityTypeConfiguration<AdminConjunto>
    {
        public void Configure(EntityTypeBuilder<AdminConjunto> builder)
        {
            builder.ToTable("AdminesConjunto");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Cedula)
                .IsRequired()
                .HasMaxLength(12);

            builder.HasIndex(a => a.Cedula)
                .IsUnique();

            builder.Property(a => a.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.PinHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(a => a.Telefono)
                .HasMaxLength(20);

            builder.Property(a => a.Email)
                .HasMaxLength(150);

            builder.Ignore(a => a.NombreCompleto);

            builder.HasOne(a => a.Conjunto)
                .WithMany()
                .HasForeignKey(a => a.ConjuntoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
